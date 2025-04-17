import os
import json
import numpy as np
from scipy.stats import gaussian_kde
from scipy.signal import find_peaks
import csv
from scipy.spatial.transform import Rotation as R

from sympy import false

DIR_DATA = '../data'
DIR_POSE = f'{DIR_DATA}/raw/pose'
DIR_HANDS = f'{DIR_DATA}/raw/hands'
DIR_PROCESS = f'{DIR_DATA}/processed'
DIR_ALT = f'{DIR_DATA}/alt'
DIR_UNITY = f'../../../isyaratNya/Assets/StreamingAssets/Frames'
VISIBILITY = 0.95
V_FORWARD = np.array([0, 0, 1])
SIDES = ['r', 'l']
LANDMARKS = ['u', 'l', 'h',
             't1', 't2', 't3', 't4',
             'i1', 'i2', 'i3', 'i4',
             'm1', 'm2', 'm3', 'm4',
             'r1', 'r2', 'r3', 'r4',
             'p1', 'p2', 'p3', 'p4', ]

def detect_clusters_kde(data, 
                        bw_method="silverman", height=0.2, distance=0.5):

    kde = gaussian_kde(data, bw_method=bw_method)
    x = np.linspace(min(data), max(data), 1000)
    kde_values = kde(x)

    peaks, _ = find_peaks(kde_values,
                          height=np.max(kde_values)*height, distance=distance * len(x))
    peak_positions = x[peaks]

    if len(peak_positions) == 0:
        return np.zeros_like(data), kde, peak_positions
    
    labels = np.zeros_like(data, dtype=int)
    for i, point in enumerate(data):
        closest_peak_idx = np.argmin(np.abs(peak_positions - point))
        labels[i] = closest_peak_idx

    return labels, kde, peak_positions

def open_process_csv(filename):
    f = open(f"{filename}.csv", mode='w', newline="")
    writer = csv.writer(f, delimiter=",")
    writer.writerow(["sign",
                     "frame_count",
                     "trim_wrist",
                     "body_rotations",
                     "trim_body"
                     ])

    return f, writer

def get_json_files():
    return [f for f in os.listdir(DIR_POSE) if os.path.isfile(os.path.join(DIR_POSE, f))]

def load_frames(sign):
    with open(f"{DIR_POSE}/{sign}", 'r') as f:
        frames_pose = json.load(f)
        frames_pose = frames_pose['frames']
        # frames_pose = np.array(frames_pose)
    with open(f"{DIR_HANDS}/{sign}", 'r') as f:
        frames_hands = json.load(f)
        frames_hands = frames_hands['frames']
        # frames_hands = np.array(frames_hands)

    return frames_pose, frames_hands

def pop_frame(frames_pose, frames_hands, frame_idx):
    frames_pose.pop(frame_idx)
    frames_hands.pop(frame_idx)

def trim_null(frames_pose, frames_hands):

    frame_idx = 0
    while frame_idx < len(frames_pose):

        pose = frames_pose[frame_idx]['pose']
        right_hand = frames_hands[frame_idx]['right_hand']
        left_hand = frames_hands[frame_idx]['left_hand']

        if not pose:
            pop_frame(frames_pose, frames_hands, frame_idx)
            continue

        if not right_hand and not left_hand:
            pop_frame(frames_pose, frames_hands, frame_idx)
            continue

        right_wrist_visibility = pose['15']['visibility']
        left_wrist_visibility = pose['16']['visibility']
        if right_wrist_visibility < VISIBILITY and left_wrist_visibility < VISIBILITY:
            pop_frame(frames_pose, frames_hands, frame_idx)
            continue

        frame_idx += 1

def landmark_to_np(landmark):
    return np.array([landmark['x'], landmark['y'], landmark['z']])

def get_axis(v1, v2):
    axis = np.cross(v1, v2)
    axis /= np.linalg.norm(axis)
    return axis

def get_angle(v1, v2):
    return np.arccos(np.clip(np.dot(v1, v2), -1.0, 1.0))

def get_plane_angle(v1, v2):
    return np.clip(np.dot(v1, v2), -1.0, 1.0)

def get_body_angle_axis(r_upper_arm, l_upper_arm, xz = True):
    body_axis = get_axis(r_upper_arm, l_upper_arm)
    if xz: body_axis[1] = 0.

    body_angle = get_plane_angle(V_FORWARD, body_axis)

    return body_angle, body_axis

def get_max_count_label(labels):
    unique_labels, counts = np.unique(labels, return_counts=True)
    max_label = unique_labels[np.argmax(counts)]

    return max_label

def trim_frames_body_label(frames_pose, frames_hands, labels, max_label):
    frames_pose = np.array(frames_pose)
    frames_pose = frames_pose[labels == max_label].tolist()

    frames_hands = np.array(frames_hands)
    frames_hands = frames_hands[labels == max_label].tolist()

    return frames_pose, frames_hands

def trim_body_rotation(frames_pose, frames_hands):

    np_angle = np.empty(len(frames_pose))

    frame_idx = 0
    while frame_idx < len(frames_pose):

        pose = frames_pose[frame_idx]['pose']
        if not pose:
            continue

        r_upper_arm = landmark_to_np(pose['11'])
        l_upper_arm = landmark_to_np(pose['12'])

        body_angle, body_axis = get_body_angle_axis(r_upper_arm, l_upper_arm)
        np_angle[frame_idx] = body_angle

        frame_idx += 1

    if not frames_pose:
        return [], [] , 0

    labels, kde, peak_positions = detect_clusters_kde(np_angle)

    # two peaks
    if len(peak_positions) == 2:

        # delta between peaks
        if np.abs(peak_positions[0] - peak_positions[1]) > 0.02:
            max_label = get_max_count_label(labels)
            frames_pose, frames_hands = trim_frames_body_label(frames_pose, frames_hands, labels, max_label)



    return frames_pose, frames_hands, len(peak_positions)

def get_pose_side(pose):

    right_pose = {0: pose['11'],
                  1: pose['13'],
                  2: pose['15'],}

    left_pose = {0: pose['12'],
                 1: pose['14'],
                 2: pose['16'],}

    return right_pose, left_pose

def populate_pose(np_side, pose):

    if not pose:
        return

    for i, landmark in pose.items():
        np_landmark = landmark_to_np(landmark)
        np_side[int(i)] = np_landmark

def populate_hand(np_side, hand):

    if not hand:
        return

    wrist = landmark_to_np(hand['0'])
    for i, landmark in list(hand.items())[1:]:
        np_landmark = landmark_to_np(landmark)
        np_side[int(i) + 2] = np_side[2] + (np_landmark - wrist)

def populate_np_side(np_side, pose, hand):

    # populate from pose
    populate_pose(np_side, pose)

    # populate from hand
    populate_hand(np_side, hand)

def combine_landmarks_side(frame_pose, frame_hands):

    np_right = np.zeros((23, 3))
    np_left = np.zeros((23, 3))

    right_pose, left_pose = get_pose_side(frame_pose['pose'])
    right_hand = frame_hands['right_hand']
    left_hand = frame_hands['left_hand']

    populate_np_side(np_right, right_pose, right_hand)
    populate_np_side(np_left, left_pose, left_hand)

    return np_right, np_left

def normalize_vector(vector):
    return vector / np.linalg.norm(vector)

def get_quat_rotation_from_axis(axis):

    rotation_axis = get_axis(axis, V_FORWARD)
    rotation_axis = normalize_vector(rotation_axis)
    rotation_angle = get_angle(rotation_axis, V_FORWARD)

    quat = np.append(np.sin(rotation_angle / 2) * axis, np.cos(rotation_angle / 2))
    quat = normalize_vector(quat)
    print(quat)
    rotation = R.from_quat(quat)

    return rotation

def apply_rotation(np_right, np_left, body_rotation):
    rotation_matrix = body_rotation.as_matrix()
    np_right = rotation_matrix @ np_right.T
    np_left = rotation_matrix @ np_left.T

    return np_right.T, np_left.T

def rotate_body(np_right, np_left):


    # apply rotation

    return np_right, np_left

def cleanup(np_side):
    out = {}
    for i in range(len(np_side)):
        np_landmark = np_side[i]

        if np.all(np_landmark):
            out[LANDMARKS[i]] = np.round(np_landmark, 3).tolist()

    return out

def calculate_body_rotation(r, l):
    shoulder_mid = (r + l) * 0.5

    # Shoulder direction (right axis)
    shoulder_dir = r - l ##TODO
    body_right = shoulder_dir / np.linalg.norm(shoulder_dir)

    # World up vector (adjust if needed)
    world_up = np.array([0, 1, 0])

    # Body's up vector (orthogonal to shoulders and world up)
    body_up = np.cross(shoulder_dir, world_up)
    body_up = body_up / np.linalg.norm(body_up)

    # Body's forward vector (orthogonal to up and right)
    body_forward = np.cross(body_up, body_right)
    body_forward = body_forward / np.linalg.norm(body_forward)

    # Construct rotation matrix
    rotation_matrix = np.column_stack((body_right, body_up, body_forward))

    # Convert to quaternion (w, x, y, z)
    trace = np.trace(rotation_matrix)
    if trace > 0:
        S = np.sqrt(trace + 1.0) * 2
        qw = 0.25 * S
        qx = (rotation_matrix[2, 1] - rotation_matrix[1, 2]) / S
        qy = (rotation_matrix[0, 2] - rotation_matrix[2, 0]) / S
        qz = (rotation_matrix[1, 0] - rotation_matrix[0, 1]) / S
    else:
        # Handle other cases for numerical stability
        if rotation_matrix[0, 0] > rotation_matrix[1, 1] and rotation_matrix[0, 0] > rotation_matrix[2, 2]:
            S = np.sqrt(1.0 + rotation_matrix[0, 0] - rotation_matrix[1, 1] - rotation_matrix[2, 2]) * 2
            qw = (rotation_matrix[2, 1] - rotation_matrix[1, 2]) / S
            qx = 0.25 * S
            qy = (rotation_matrix[0, 1] + rotation_matrix[1, 0]) / S
            qz = (rotation_matrix[0, 2] + rotation_matrix[2, 0]) / S
        elif rotation_matrix[1, 1] > rotation_matrix[2, 2]:
            S = np.sqrt(1.0 + rotation_matrix[1, 1] - rotation_matrix[0, 0] - rotation_matrix[2, 2]) * 2
            qw = (rotation_matrix[0, 2] - rotation_matrix[2, 0]) / S
            qx = (rotation_matrix[0, 1] + rotation_matrix[1, 0]) / S
            qy = 0.25 * S
            qz = (rotation_matrix[1, 2] + rotation_matrix[2, 1]) / S
        else:
            S = np.sqrt(1.0 + rotation_matrix[2, 2] - rotation_matrix[0, 0] - rotation_matrix[1, 1]) * 2
            qw = (rotation_matrix[1, 0] - rotation_matrix[0, 1]) / S
            qx = (rotation_matrix[0, 2] + rotation_matrix[2, 0]) / S
            qy = (rotation_matrix[1, 2] + rotation_matrix[2, 1]) / S
            qz = 0.25 * S

    return np.array([qw, qx, qy, qz])

def rotate_nps():
    pass

def process(frames_pose, frames_hands, frame_count):

    frames = []

    # loop
    i = 0
    while i < frame_count:
        np_right, np_left = combine_landmarks_side(frames_pose[i], frames_hands[i])

        frame = {
            'r': cleanup(np_right),
            'l': cleanup(np_left),
        }
        frames.append(frame)

        i += 1

    return frames

def save_frames(sign_name, frames, to_utils = True, to_unity = false):

    sign_frames = {
        's': sign_name,
        'f': frames,
    }

    if to_utils:
        with open(f"{DIR_ALT}/{sign_name}.json", "w") as f:
            f.write(json.dumps(sign_frames, indent=4))

        with open(f"{DIR_PROCESS}/{sign_name}.json", "w") as f:
            f.write(json.dumps(sign_frames, separators=(',', ':')))

    if to_unity:
        with open(f"{DIR_UNITY}/{sign_name}.json", "w") as f:
            f.write(json.dumps(sign_frames, separators=(',', ':')))

def main():
    signs = get_json_files()
    process_csv, process_writer = open_process_csv("process_delta")

    # signs = ["Ayun.json"]

    for sign in signs:
        sign_name = sign.split('.')[0]
        print(f"---{sign_name}---")

        # load frames
        frames_pose, frames_hands = load_frames(sign)
        init_frame_count = len(frames_pose)

        print(f"Trimming")
        # trim frames when both wrist not detected in pose
        trim_null(frames_pose, frames_hands)
        trim_wrist_frame_count = len(frames_pose)

        # trim frames when show sign from two sides (ex. ayun)
        frames_pose, frames_hands, body_rotations = trim_body_rotation(frames_pose, frames_hands)
        trim_body_frame_count = len(frames_pose)
        print(f"{init_frame_count} -> {trim_wrist_frame_count} -> {trim_body_frame_count}")


        # for analyzing
        process_writer.writerow([sign_name, init_frame_count, trim_wrist_frame_count, body_rotations, trim_body_frame_count, ])

        # process data
        print(f"Processing")
        frames = process(frames_pose, frames_hands, trim_body_frame_count)

        print(f"Saving")
        save_frames(sign_name, frames, True, True)

        print(f"---{sign_name}---\n")

    process_csv.close()

if __name__ == '__main__':
    main()