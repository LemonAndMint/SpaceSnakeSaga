using System.Collections;
using Cinemachine;
using UnityEngine;

public class CameraActions : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCamera;
    [SerializeField] private float _zoomQuantity = 0.05f;

    private void Start() {
        
        if(virtualCamera == null)
            virtualCamera = GetComponent<CinemachineVirtualCamera>();

    }

    public void ZoomOut(){

        StartCoroutine(_zoomOutCoroutine());

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
