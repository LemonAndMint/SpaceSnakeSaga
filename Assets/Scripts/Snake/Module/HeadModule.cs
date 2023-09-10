using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadModule : BlankModule
{
    /*public override void OnTriggerEnter(Collider other) {

        base.OnTriggerEnter(other);
        
        
    }*/
    public override void OnTriggerEnter2D(Collider2D other) {
        
        base.OnTriggerEnter2D(other);

        if(other.tag == "Energy"){

            Debug.Log("Picked");

        }

    }

        
}
