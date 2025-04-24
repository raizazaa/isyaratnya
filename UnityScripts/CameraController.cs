using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class CameraController : MonoBehaviour
{
    public Transform target;             // The object to orbit around
    public float rotationSpeed = 0.2f;
    public float zoomSpeed = 0.5f;
    public float minDistance;
    public float maxDistance;

    private float _currentDistance;
    private Vector2 _lastTouchPos;

    private Vector3 _defaultPosition;
    private Quaternion _defaultRotation;
    public Button reset;
    

    void Start()
    {
        if (target == null)
        {
            Debug.LogError("No target assigned to OrbitCameraController.");
            return;
        }

        _currentDistance = Vector3.Distance(transform.position, target.position);
        _defaultRotation = transform.rotation;
        _defaultPosition = transform.position;
        minDistance = _currentDistance * 0.8f;
        maxDistance = _currentDistance * 1.2f;
        
        reset.onClick.AddListener(ResetCamera);
    }

    void Update()
    {
        if (Input.touchCount == 1)
        {   
            Touch touch = Input.GetTouch(0);
            
            if (touch.phase == TouchPhase.Moved)
            {
                float rotX = -touch.deltaPosition.y * rotationSpeed;
                float rotY = touch.deltaPosition.x * rotationSpeed;
                
                transform.RotateAround(target.position, transform.right, rotX);
                transform.RotateAround(target.position, Vector3.up, rotY);
            }
        }
        
        if (Input.touchCount == 2)
        {
            Touch t0 = Input.GetTouch(0);
            Touch t1 = Input.GetTouch(1);

            float prevMag = (t0.position - t0.deltaPosition - (t1.position - t1.deltaPosition)).magnitude;
            float currMag = (t0.position - t1.position).magnitude;

            float diff = prevMag - currMag;

            _currentDistance += diff * zoomSpeed * Time.deltaTime;
            _currentDistance = Mathf.Clamp(_currentDistance, minDistance, maxDistance);

            // Move camera to new zoom distance
            Vector3 dir = (transform.position - target.position).normalized;
            transform.position = target.position + dir * _currentDistance;
        }
        
        transform.LookAt(target);
    }

    private void ResetCamera()
    {
        transform.rotation = _defaultRotation;
        transform.position = _defaultPosition;
    }
    
}
