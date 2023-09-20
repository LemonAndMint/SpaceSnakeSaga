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
        _rotate();

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

    void _rotate(){

        if( weaponModule.TargetedEnemy != null ){

            _rotateTo(weaponModule.TargetedEnemy.transform.position);

        }
        else{

            _rotateTo(transform.position + directionVector);

        }

        _resetRotation();

    }

    private void _rotateTo(Vector3 rotatePoint){ //https://youtu.be/1Oda2M4BoNs?t=86

        Vector3 look = transform.InverseTransformPoint( rotatePoint );

        //Cannonun sagda ya da solda olup olmadigi cannonun y pozisyonunun isaretine gore belirlendik.
        //Sag cannon asagi tarafta kaldigindan aci degerinin 90 derece ile toplanmasi gerekirken
        //Sol caoonon yukari tarafta kaldigindan aci degerinin 90 derece ile cikartilmasi gerek.
        //https://gamedev.stackexchange.com/questions/159581/why-do-i-need-to-offset-my-aim-rotation-angle-by-90-degrees#:~:text=1%20Answer&text=As%20per%20the%20unit%20circle,%2C%20and%20%2D90%20at%20right.

        float angle = Mathf.Atan2(look.y, look.x) * Mathf.Rad2Deg + 90;

        transform.Rotate(0 , 0, angle); 

    }

    private void _resetRotation(){ transform.Rotate( 0, 0, 0 ); }
    
}
