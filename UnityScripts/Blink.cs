using UnityEngine;

public class Blink : MonoBehaviour
{
    
    [Header("Blink Settings")]
    public float minTimeBetweenBlinks = 2f;
    public float maxTimeBetweenBlinks = 4f;
    public float blinkDuration = 0.15f;
    
    private float _blinkTimer;
    private float _nextBlinkTime;
    private float _blinkProgress;
    
    private SkinnedMeshRenderer _mesh;
    
    void Start()
    {
        GetBlendShape();
    }

    private void GetBlendShape()
    {
        _mesh = transform.GetComponent<SkinnedMeshRenderer>();
    }


    void LateUpdate()
    {
        BlinkBlink();
    }

    private void BlinkBlink()
    {
        _blinkTimer += Time.deltaTime;
        
        if (_blinkTimer >= _nextBlinkTime)
        {
            _blinkProgress += Time.deltaTime / blinkDuration;
            
            float blinkValue = Mathf.Clamp(Mathf.Sin(_blinkProgress * Mathf.PI) * 100f, 0f, 100f);
            
            _mesh.SetBlendShapeWeight(17, blinkValue);

            if (_blinkProgress >= 1f)
            {
                _blinkProgress = 0f;
                _blinkTimer = 0f;
                SetNextBlinkTime();
            }
        }
    }
    
    private void SetNextBlinkTime()
    {
        _nextBlinkTime = Random.Range(minTimeBetweenBlinks, maxTimeBetweenBlinks);
    }
    
    private void GetBlendShapeNames ()
    {
        SkinnedMeshRenderer head = transform.GetComponent<SkinnedMeshRenderer>();
        Mesh m = head.sharedMesh;
        string[] arr;
        arr = new string [m.blendShapeCount];
        for (int i= 0; i < m.blendShapeCount; i++)
        {
            string s = m.GetBlendShapeName(i);
            Debug.Log("Blend Shape: " + i + " " + s); // Blend Shape: 4 FightingLlamaStance
        }
    }
}
