using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEngine;
using Sign;
using Snowball;
using TMPro;
using UniHumanoid;
using Unity.VisualScripting;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Whisper;
using Whisper.Utils;
using Debug = UnityEngine.Debug;

public class IsyaratNya : MonoBehaviour
{
    [Header("Frames")]
    private SignFrame _currentSign;
    public string signName;
    private Frame _currentFrame;
    private FrameData _rData;
    private FrameData _lData;
    public int frameIdx = 0;
    public int frameCount;
    
    [Header("Humanoid")]
    private Humanoid _humanoid;
    private Hum _rHum;
    private Hum _lHum;
    
    [Header("RotationTarget")]
    private FrameRotation _rRotation;
    private FrameRotation _lRotation;

    [Header("States")]
    private bool _inGesture = false;
    [Range(0.5f, 2f)] public float signDuration;
    public float frameDuration;
    public float t;
    private float _t;

    [Header("Whisper")]
    public WhisperManager whisper;
    public MicrophoneRecord microphone;
    private string _buffer;
    public bool streamSegments;
    
    [Header("Snowball")]
    private IndonesianStemmer _stemmer;
    
    [Header("UI")]
    public Button record;
    public TMP_Text recordText;
    public Button enter;
    public TMP_InputField input;

    void Awake()
    {
        InitWhisper();
    }

    void InitWhisper()
    {
        whisper.OnNewSegment += OnNewSegment;
            
        microphone.OnRecordStop += OnRecordStop;
            
        record.onClick.AddListener(OnRecordButtonPressed);
        enter.onClick.AddListener(OnEnterButtonPressed);
    }

    private void OnEnterButtonPressed()
    {
        if (input.text == "") return;
        
        signName = input.text.Split(" ")[0];
        
        StartGesture();
    }
    
    private void OnRecordButtonPressed()
    {
        if (!microphone.IsRecording)
        {
            microphone.StartRecord();
            recordText.text = "Stop";
        }
        else
        {
            microphone.StopRecord();
            recordText.text = "Record Voice / Rekam Suara";
        }
    }
    
    private async void OnRecordStop(AudioChunk recordedAudio)
    {
        recordText.text = "Record Voice / Rekam Suara";
        _buffer = "";
            
        var res = await whisper.GetTextAsync(recordedAudio.Data, recordedAudio.Frequency, recordedAudio.Channels);
        if (res == null) 
            return;

        var text = res.Result;
        
        input.text = text;
    }
    
    private void OnNewSegment(WhisperSegment segment)
    {
        if (!streamSegments)
            return;

        _buffer += segment.Text;
        input.text = _buffer;
    }
    
    void Start()
    {
        InitHumanoid();
        InitRotation();
        LoadJson();
        t = 10f;
    }
    
    private void InitHumanoid()
    {
        _humanoid = transform.parent.GetComponent<Humanoid>();
        _rHum = new Hum(_humanoid, Side.Right);
        _lHum = new Hum(_humanoid, Side.Left);
    }
    
    private void InitRotation()
    {
        _rRotation = new FrameRotation(Side.Right);
        _lRotation = new FrameRotation(Side.Left);
    }
    
    void LateUpdate()
    {
        RenderFrame();
    }
    
    private void LoadJson()
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, "Frames", signName + ".json");
        string jsonData = File.ReadAllText(filePath);
        _currentSign = JsonUtility.FromJson<SignFrame>(jsonData);
        frameCount = _currentSign.f.Length;
        frameDuration = signDuration / frameCount;
        
        frameIdx = 0;
        _currentFrame = _currentSign.f[frameIdx];
        _rData = _currentFrame.r;
        _lData = _currentFrame.l;
    }
    
    
    #region gesture
    private void StartGesture()
    {
        if (_inGesture) return;
        LoadJson();

        StartCoroutine(Gesture());
    }

    private void StopGesture()
    {
        _inGesture = false;
        frameIdx = 0;
        StopCoroutine(Gesture());
    }
    
    private IEnumerator Gesture()
    {
        _inGesture = true;

        while (_inGesture && frameIdx < frameCount)
        {
            LoadFrame();
            UpdateTarget();
            
            IterFrame();
            
            yield return new WaitForSeconds(frameDuration);
        }
        _inGesture = false;
    }
    #endregion
    


    #region frame

    private void LoadFrame()
    {
        _currentFrame = _currentSign.f[frameIdx];
        // _currentFrame.SetBody();
        
        _rData = _currentFrame.r;
        _rData.SetSide(Side.Right);
        
        _lData = _currentFrame.l;
        _lData.SetSide(Side.Left);
    }

    private void UpdateTarget()
    {
        _rRotation.UpdateRotation(_rData);
        _lRotation.UpdateRotation(_lData);
    }
    
    private void RenderFrame()
    {
        _t = Time.deltaTime * t;
        _rHum.UpdateHum(_rRotation, _t);
        _lHum.UpdateHum(_lRotation, _t);
    }
    
    private void IterFrame(bool next = true)
    {
        frameIdx += next ? 1 : -1;
    }
    
    #endregion
    
    
}
