using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyMovement : MonoBehaviour
{

    [SerializeField] private float _speed = 10f;
    [SerializeField] private float _counter = 2f;
    [SerializeField] private float _maxDistanceFromTarget = 3f;

    public WeaponModule weaponModule;

    public Vector3 targetDirect;
    public Vector3 focusPoint;
    private Vector3 _lastTargetDirect;

    Vector3 directionVector = Vector3.zero;


    private Rigidbody2D rb2d;
    private float _runCounter = 0f;

    private void Start() {
        
        if(rb2d == null)
            rb2d = GetComponent<Rigidbody2D>();
        
        if(weaponModule == null)
            weaponModule = GetComponentInChildren<WeaponModule>();

        _lastTargetDirect = transform.right;
        focusPoint = transform.position; 

    }

    private void FixedUpdate() {
        
        _move();

    }


    private void _move(){


        if(weaponModule.TargetedEnemy != null){

            focusPoint = weaponModule.TargetedEnemy.transform.position;

        }
        else{

            focusPoint = transform.position;

        }

        if(targetDirect != Vector3.zero){

            directionVector += targetDirect;
            _lastTargetDirect = targetDirect;

            _runCounter = 0;

        }

        if(Vector3.Distance(transform.position, focusPoint) < _maxDistanceFromTarget){

            if(_runCounter < _counter){
                
                _runCounter += Time.deltaTime;

            }
            else{

                _lastTargetDirect = new Vector3( Random.insideUnitCircle.x, 
                                                 Random.insideUnitCircle.y, 0 );
                _runCounter = 0;

            }

            directionVector += _lastTargetDirect;
        
        }
        else if(Vector3.Distance(transform.position, focusPoint) >= _maxDistanceFromTarget && focusPoint != transform.position){

            directionVector = Vector3.MoveTowards(transform.position, focusPoint, 100f);

        }
        
        directionVector = directionVector.normalized;

        rb2d.velocity = directionVector * _speed * Time.deltaTime;

    }

    
}
