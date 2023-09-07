using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeMovement : MonoBehaviour
{
    
    //Fixed Update saniyede calisma miktari
    const int FIXED_FPS = 50;

    [SerializeField] private float _speed = 180f;
    [SerializeField] private float _turnspeed = 18f;

    List<GameObject> _snakeBody = new List<GameObject>();

    public List<GameObject> SnakeBody { 
        
        get{

            return _snakeBody;

        } 
    
    }


    void FixedUpdate()
    {
        
        Movement();

    }

    void Movement()
    {

        _snakeBody[0].GetComponent<Rigidbody2D>().velocity = _snakeBody[0].transform.right * _speed * Time.deltaTime;
        if(Input.GetAxis("Horizontal") != 0)
        {

            _snakeBody[0].transform.Rotate(new Vector3(0, 0, -_turnspeed * Time.deltaTime * Input.GetAxis("Horizontal")));

        }

        if(_snakeBody.Count > 1)
        {

            for(int i = 1; i < _snakeBody.Count; i++)
            {

                MarkerStorage markM = _snakeBody[i - 1].GetComponent<MarkerStorage>();
                _snakeBody[i].transform.position = markM.markerList[0].position;
                _snakeBody[i].transform.rotation = markM.markerList[0].rotation;
                markM.markerList.RemoveAt(0);

            }

            //En sondaki modulun marker sayisini 50 FPS * distanceBetween degerinde tutma

            if( _snakeBody[_snakeBody.Count - 1].GetComponent<MarkerStorage>().markerList.Count > _snakeBody[0].GetComponent<MarkerStorage>().markerList.Count + 1){

                _snakeBody[_snakeBody.Count - 1].GetComponent<MarkerStorage>().markerList.RemoveAt(0);

            }

        }

    }
}
