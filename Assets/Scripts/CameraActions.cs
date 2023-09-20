using System.Collections;
using Cinemachine;
using UnityEngine;

public class CameraActions : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCamera;
    [SerializeField] private float _zoomQuantity = 0.05f;

    private static CameraActions _cameraActions;
    public static CameraActions Instance {

        get{

            
            return _cameraActions;

        }

    }

    private void Start() {

        if(_cameraActions != null && _cameraActions != this){

            Destroy(gameObject);

        }

        _cameraActions = this;
        
        if(virtualCamera == null)
            virtualCamera = GetComponent<CinemachineVirtualCamera>();

    }

    public void ZoomOut(){

        StartCoroutine(_zoomOutCoroutine());

    }

    public void Shake(){

        StartCoroutine(_processShake(15, 1));

    }

    private IEnumerator _processShake(float shakeIntensity = 5f, float shakeTiming = 0.5f)
    {
        Noise(1, shakeIntensity);
        yield return new WaitForSeconds(shakeTiming);
        Noise(0, 0);
    }

    public void Noise(float amplitudeGain, float frequencyGain)
    {
        CinemachineBasicMultiChannelPerlin noise = virtualCamera.GetComponentInChildren<CinemachineBasicMultiChannelPerlin>();

        noise.m_AmplitudeGain = amplitudeGain;
        noise.m_AmplitudeGain = amplitudeGain;
        noise.m_AmplitudeGain = amplitudeGain;
            
        noise.m_FrequencyGain = frequencyGain;
        noise.m_FrequencyGain = frequencyGain;
        noise.m_FrequencyGain = frequencyGain;      

    }

    private IEnumerator _zoomOutCoroutine(){

        bool _isDone = false;
        float targetZoom = virtualCamera.m_Lens.OrthographicSize + 1;

        while(!_isDone){

            if(virtualCamera.m_Lens.OrthographicSize >= targetZoom){

                _isDone = true;
                continue;

            }

            virtualCamera.m_Lens.OrthographicSize += _zoomQuantity;

            yield return new WaitForFixedUpdate();

        }

    }

}
