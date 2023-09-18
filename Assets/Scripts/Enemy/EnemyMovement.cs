using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private float _speed = 10f;
    [SerializeField] private float _counter = 2f;
    

    public Vector3 targetDirect;
    private Vector3 _lastTargetDirect;


    private Rigidbody2D rb2d;
    private float _runCounter = 0f;

    private void Start() {
        
        rb2d = GetComponent<Rigidbody2D>();
        _lastTargetDirect = transform.right;

    }

    private void FixedUpdate() {
        
        _move();

    }


    private void _move(){

        Vector3 directionVector = Vector3.zero;

        if(targetDirect != Vector3.zero){

            directionVector += targetDirect;
            _lastTargetDirect = targetDirect;

            _runCounter = 0;

        }
        else{

            if(_runCounter < _counter){

                directionVector += _lastTargetDirect;
                _runCounter += Time.deltaTime;

            }
            else{

                //#TODO fonksiyon yaz.

            }
            
            
        }

        
        
        rb2d.velocity = directionVector * _speed * Time.deltaTime;

    }

    
}
