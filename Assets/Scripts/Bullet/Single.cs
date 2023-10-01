using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Single : Bullet
{
    private void Awake() {
        
        onHit.AddListener( (GameObject x) => _startParticle() );

    }

    private void Start() {
        
        transform.right = targetLastPosition - transform.position;

    }

    private void _startParticle(){

        if(particleGOPrefb != null){

            GameObject particleGO = Instantiate(particleGOPrefb, transform.position, Quaternion.identity);
            Destroy(particleGO, particleGO.GetComponent<ParticleSystem>().main.startLifetime.constantMax + 0.5f);
        
        }
    }

    void Update()
    {
        
        _move();

    }

    private void _move(){

        if(targetLastPosition != null){
        
            transform.position = Vector3.MoveTowards(transform.position, targetLastPosition, Time.deltaTime * moveSpeed);
            if( Vector3.Distance(transform.position, targetLastPosition) < 0.05f ){

                OnImpact();

            }
        }

    }
}
