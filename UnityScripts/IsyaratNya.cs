using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Sign;
using Snowball;
using TMPro;
using UniHumanoid;
using Unity.VisualScripting;
using UnityEngine.UI;

public class IsyaratNya : MonoBehaviour
{
    [Header("Sign")]
    private SignFrame _currentSign;
    public string signName;
    private Frame _currentFrame;
    private FrameData _rData;
    private FrameData _lData;
    
    [Header("Frames")]
    public int frameIdx = 0;
    public int frameCount;
    [Range(0f, 1f)] public float midPoint;
    [Range(0f, 1f)] public float sharpness;
    public float gaussian;
    
    [Header("Humanoid")]
    private Humanoid _humanoid;
    private HumanoidController _humanoidController;
    
    [Header("RotationTarget")]
    private BodyRotation _bodyRotation;
    private SideRotation _rSideRotation;
    private SideRotation _lSideRotation;
    
    [Header("States")]
    private bool _inGesture = false;
    [Range(0.2f, 3f)] public float signDuration;
    public float frameDuration;
    public float t;
    
    [Header("Parse")]
    private Parser _parser;
    private Queue<string> _signQueue;
    
    
    [Header("UI")]
    public Button enter;
    public TMP_InputField input;
    public Slider slider;
    
    void Start()
    {
        InitHumanoid();
        InitRotation();
        enter.onClick.AddListener(OnEnterButtonPressed);
        slider.onValueChanged.AddListener(OnSliderValueChanged);
        _parser = new Parser();
    }
    
    void LateUpdate()
    {
        RenderFrame();
    }
    
    #region init
    private void InitHumanoid()
    {
        _humanoid = transform.parent.GetComponent<Humanoid>();
        _humanoidController = new HumanoidController(
            new BodyController(_humanoid), 
            new SideController(_humanoid, Side.Right), 
            new SideController(_humanoid, Side.Left));
    }
    private void InitRotation()
    {
        t = 8;
        gaussian = 0.5f;
        _bodyRotation = new BodyRotation();
        _rSideRotation = new SideRotation(Side.Right);
        _lSideRotation = new SideRotation(Side.Left);
    }
    private void OnEnterButtonPressed()
    {
        if (input.text == "" || _inGesture) return;
        
        _signQueue = _parser.Parse(input.text);
        
        StartGesture();
    }
    private void OnSliderValueChanged(float value) => signDuration = value;
    #endregion

    #region gesture
    private void LoadJson()
    {
        var filePath = Resources.Load<TextAsset>("Frames/" + signName);
        _currentSign = JsonUtility.FromJson<SignFrame>(filePath.text);
        frameCount = _currentSign.f.Length;
        frameDuration = signDuration / frameCount;
        
        frameIdx = 0;
        _currentFrame = _currentSign.f[frameIdx];
        _rData = _currentFrame.r;
        _lData = _currentFrame.l;
    }
    private void StartGesture()
    {
        if (_inGesture) return;
        
        signName = _signQueue.Dequeue();
        LoadJson();
        StartCoroutine(Gesture());
    }

    private void StopGesture()
    {
        _inGesture = false;
        frameIdx = 0;
        StopCoroutine(Gesture());
    }

    private void InvokeGesture()
    {
        _inGesture = true;
        while (_inGesture && frameIdx < frameCount)
        {
            Invoke(nameof(InvokeFrame), frameDuration);
        }
        _inGesture = false;
        if (_signQueue.Count > 0)
        {
            StartGesture();
        }
    }

    private void InvokeFrame()
    {
        LoadFrame();
        UpdateRotation();
        IterFrame();
    }
    
    private IEnumerator Gesture()
    {
        _inGesture = true;
        while (_inGesture && frameIdx < frameCount)
        {
            LoadFrame();
            UpdateRotation();
            IterFrame();
            yield return new WaitForSecondsRealtime(frameDuration);
        }
        _inGesture = false;
        if (_signQueue.Count > 0)
        {
            StartGesture();
        }
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
    private void UpdateRotation()
    {
        _bodyRotation.UpdateRotation(_rData, _lData);
        _rSideRotation.UpdateRotation(_rData);
        _lSideRotation.UpdateRotation(_lData);
    }
    private void GaussianCurve()
    {
        var x = Mathf.Clamp01((float)frameIdx / frameCount);
        gaussian = Mathf.Exp(-Mathf.Pow(x - midPoint, 2) / (2 * sharpness * sharpness));
        t = (gaussian * 7f) + 1f;
    }
    private void RenderFrame()
    {
        if (_inGesture) GaussianCurve(); else t = 6;
        _humanoidController.Update(_bodyRotation, _rSideRotation, _lSideRotation, Time.deltaTime * t, _inGesture);
    }
    private void IterFrame(bool next = true) => frameIdx += next ? 1 : -1;
    #endregion
    
    
}
