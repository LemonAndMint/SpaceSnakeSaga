using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Transform targetTransform;
    private Vector3 targetLastPosition; 

    private void Start() {
        
        targetLastPosition = targetTransform.position;

    }

    void Update()
    {
        
        _move();

    }

    private void _move(){

        if(targetTransform != null){
        
            transform.position = Vector3.MoveTowards(transform.position, targetTransform.position, Time.deltaTime * moveSpeed);
            targetLastPosition = targetTransform.position;
        
        }
        else{

            //Eger hedef transform mermi ulasamadan yok olursa mermi hedef transformun son konumuna gidip yok edilir.
            transform.position = Vector3.MoveTowards(transform.position, targetLastPosition, Time.deltaTime * moveSpeed);
            
            if( Vector3.Distance(transform.position, targetLastPosition) < 0.05f ){

                Destroy(gameObject);

            }
        }

    }
}
