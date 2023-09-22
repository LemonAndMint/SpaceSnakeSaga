using System;
using UnityEngine;

public class UnevirseHandler : MonoBehaviour
{
    // References to scene objects
    [SerializeField] private Camera mainCamera = null;
    [SerializeField] private GameObject ship = null;
    [SerializeField] private GameObject space = null;
    
    // The radius of a possible camera view
    private float _spaceCircleRadius = 0;

    // Original background object dimensions
    private float _backgroundOriginalSizeX = 0;
    private float _backgroundOriginalSizeY = 0;

    // Direction of travel
    private Vector3 _moveVector;
    // Turning speed in radians
    private float _rotationSpeed = 1f;

    // Helper Variables
    private bool _mousePressed = false;
    private float _halfScreenWidth = 0;
    
    void Start()
    {
        // Starting direction
        _moveVector = new Vector3(0, 1.5f, 0);
        // Used to determine the direction of rotation
        _halfScreenWidth = Screen.width / 2f;
        
        // Original Background Sizes
        SpriteRenderer sr = space.GetComponent<SpriteRenderer>();
        var originalSize = sr.size;
        _backgroundOriginalSizeX = originalSize.x;
        _backgroundOriginalSizeY = originalSize.y;

        // Camera height equal to the orthographic size
        float orthographicSize = mainCamera.orthographicSize;
        // Camera width equals to orthographic size multiplied by aspect ratio
        float screenAspect = (float)Screen.width / (float)Screen.height;
        // The radius of the circle describing the camera
        _spaceCircleRadius = Mathf.Sqrt(orthographicSize * screenAspect * orthographicSize * screenAspect + orthographicSize * orthographicSize);

        // The final background sprite size should allow you to move by one basic background texture size in any direction + overlap the radius of the camera also in all directions
        sr.size = new Vector2(_spaceCircleRadius * 2 + _backgroundOriginalSizeX * 2, _spaceCircleRadius * 2 + _backgroundOriginalSizeY * 2);
    }

    void Update()
    {
        // Changing the direction of movement by clicking the mouse button
        if (Input.GetMouseButtonDown (0)) {
            _mousePressed = true;
        }

        if (Input.GetMouseButtonUp(0))
        {
            _mousePressed = false;
        }
        
        if (_mousePressed)
        {
            // The direction of rotation is determined depending on the side of the screen on which the click occurred
            int rotation = Input.mousePosition.x >= _halfScreenWidth ? -1 : 1;

            // Calculation of rotation of the direction vector
            float xComp = (float)(_moveVector.x * Math.Cos(_rotationSpeed * rotation * Time.deltaTime) - _moveVector.y * Math.Sin(_rotationSpeed * rotation * Time.deltaTime));
            float yComp = (float) (_moveVector.x * Math.Sin(_rotationSpeed * rotation * Time.deltaTime) + _moveVector.y * Math.Cos(_rotationSpeed * rotation * Time.deltaTime));
            _moveVector = new Vector3(xComp, yComp,0);

            // Rotate the sprite of the ship and camera along the direction vector
            float rotZ = Mathf.Atan2(_moveVector.y, _moveVector.x) * Mathf.Rad2Deg;
            ship.transform.rotation = Quaternion.Euler(0f, 0f, rotZ - 90);
            mainCamera.transform.rotation = Quaternion.Euler(0f, 0f, rotZ - 90);
        }

        // Move the background in the opposite direction
        space.transform.Translate(-_moveVector.x * Time.deltaTime, -_moveVector.y * Time.deltaTime, 0);

        // When the background reaches a shift equal to the original size in any direction, we return it to the origin exactly in this direction
        if (space.transform.position.x >= _backgroundOriginalSizeX)
        {
            space.transform.Translate(-_backgroundOriginalSizeX, 0, 0);
        }
        if (space.transform.position.x <= -_backgroundOriginalSizeX)
        {
            space.transform.Translate(_backgroundOriginalSizeX, 0, 0);
        }
        if (space.transform.position.y >= _backgroundOriginalSizeY)
        {
            space.transform.Translate(0, -_backgroundOriginalSizeY, 0);
        }
        if (space.transform.position.y <= -_backgroundOriginalSizeY)
        {
            space.transform.Translate(0, _backgroundOriginalSizeY, 0);
        }
    }
    
    private void OnDrawGizmos()
    {
        // Circle describing the camera
        UnityEditor.Handles.color = Color.yellow;
        UnityEditor.Handles.DrawWireDisc(Vector3.zero , Vector3.back, _spaceCircleRadius);

        // Direction of travel
        UnityEditor.Handles.color = Color.green;
        UnityEditor.Handles.DrawLine(Vector3.zero, _moveVector);
    }
}