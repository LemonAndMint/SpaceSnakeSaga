using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMovement : MonoBehaviour
{
    [SerializeField] private float _rotationSpeed = 1f;
    [SerializeField] private float _spaceCircleRadius = 1f;
    public Transform playerTransform;
    public Transform backgroundTransform;

    // Original background object dimensions
    private float _backgroundOriginalSizeX = 0;
    private float _backgroundOriginalSizeY = 0;
    // Start is called before the first frame update
    void Start()
    {

        SpriteRenderer sr = backgroundTransform.GetComponent<SpriteRenderer>();
        var originalSize = sr.size;
        _backgroundOriginalSizeX = originalSize.x;
        _backgroundOriginalSizeY = originalSize.y;

        sr.size = new Vector2(_spaceCircleRadius * 2 + _backgroundOriginalSizeX * 2, _spaceCircleRadius * 2 + _backgroundOriginalSizeY * 2);

    }

    // Update is called once per frame
    void Update()
    {
        
        if(playerTransform != null){

            float xComp = (float)(playerTransform.position.x * Math.Cos(_rotationSpeed * Time.deltaTime) - playerTransform.position.y * Math.Sin(_rotationSpeed * Time.deltaTime));
            float yComp = (float) (playerTransform.position.x * Math.Sin(_rotationSpeed * Time.deltaTime) + playerTransform.position.y * Math.Cos(_rotationSpeed* Time.deltaTime));
            
            backgroundTransform.Translate(xComp * Time.deltaTime, yComp * Time.deltaTime, 0);

            if (backgroundTransform.position.x >= _backgroundOriginalSizeX)
            {
                backgroundTransform.Translate(-_backgroundOriginalSizeX, 0, 0);
            }
            if (backgroundTransform.position.x <= -_backgroundOriginalSizeX)
            {
                backgroundTransform.Translate(_backgroundOriginalSizeX, 0, 0);
            }
            if (backgroundTransform.position.y >= _backgroundOriginalSizeY)
            {
                backgroundTransform.Translate(0, -_backgroundOriginalSizeY, 0);
            }
            if (backgroundTransform.position.y <= -_backgroundOriginalSizeY)
            {
                backgroundTransform.Translate(0, _backgroundOriginalSizeY, 0);
            }

        }

    }
}
