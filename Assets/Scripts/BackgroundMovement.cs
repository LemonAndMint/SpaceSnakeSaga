using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Cinemachine.Utility;
using UnityEngine;

public class BackgroundMovement : MonoBehaviour
{
    [SerializeField] private float _spaceCircleRadius = 1f;
    public CinemachineVirtualCamera vCamera;
    public Transform playerTransform;
    public Transform backgroundTransform;
    public MarkerStorage playerMarkerStorage;

    // Original background object dimensions
    private float _backgroundOriginalSizeX = 0;
    private float _backgroundOriginalSizeY = 0;

    private float srX = 0;
    private float srY = 0;

    float worldHeight;
    float worldWidth;

    SpriteRenderer sr;

    Vector3 lastVect3;
    // Start is called before the first frame update
    void Start()
    {

        sr = backgroundTransform.GetComponent<SpriteRenderer>();
        var originalSize = sr.size;
        _backgroundOriginalSizeX = originalSize.x;
        _backgroundOriginalSizeY = originalSize.y;

        sr.size = new Vector2(_spaceCircleRadius * 2 + _backgroundOriginalSizeX * 2 , _spaceCircleRadius * 2 + _backgroundOriginalSizeY * 2 );
        srX = _spaceCircleRadius + _backgroundOriginalSizeX;
        srY = _spaceCircleRadius + _backgroundOriginalSizeY;

        float aspect = (float)Screen.width / Screen.height;
        worldHeight = vCamera.m_Lens.OrthographicSize * 2;
        worldWidth = worldHeight * aspect;

        lastVect3 = playerTransform.position;

        //StartCoroutine(_moveIE());

    }

    // Update is called once per frame
    void Update()
    {
        if(playerTransform != null){

            Vector2 srsize = (Vector2)playerTransform.position - (Vector2)lastVect3;
            srsize = new Vector2(Mathf.Abs(srsize.x) * Time.deltaTime * 150f, Mathf.Abs(srsize.y) * Time.deltaTime * 150f);
            
            sr.size += srsize;
            
            lastVect3 = playerTransform.position;

        }

    }

    private IEnumerator _moveIE(){

        while(true){

            if(playerTransform != null){

                Debug.Log( playerTransform.position.y + worldHeight / 2f + "   " + (backgroundTransform.position.y + ( _backgroundOriginalSizeY  / 2f )));

                if ((playerTransform.position.x + worldWidth / 2f) % (srX / 2f) >= _backgroundOriginalSizeX / 2f)
                {
                    Debug.Log("1");
                    backgroundTransform.position += new Vector3(_backgroundOriginalSizeX, 0, 0);
                }
                if ((playerTransform.position.x - worldWidth / 2f) % (srX / 2f) <= -_backgroundOriginalSizeX / 2f)
                {
                    Debug.Log("2");
                    backgroundTransform.position += new Vector3(-_backgroundOriginalSizeX, 0, 0);
                }
                if (( playerTransform.position.y + worldHeight / 2f ) >=  backgroundTransform.position.y + ( _backgroundOriginalSizeY  / 2f ))
                {
                    Debug.Log("3");
                    backgroundTransform.position += new Vector3(0, (_backgroundOriginalSizeY / 2f) - (worldHeight / 2f), 0);
                    
                    sr.size += new Vector2( 0 , _spaceCircleRadius + _backgroundOriginalSizeY );
                }
                if ((playerTransform.position.y - worldHeight / 2f) % (srY / 2f) <= - _backgroundOriginalSizeY / 2f)
                {
                    Debug.Log("4");
                    backgroundTransform.position += new Vector3(0, -_backgroundOriginalSizeY, 0);
                }

            }

            yield return new WaitForSeconds(0.1f);

        }


    }
}
