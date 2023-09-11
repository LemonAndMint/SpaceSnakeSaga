using System.Collections;
using System.Collections.Generic;
using ModuleManager;
using UnityEngine;

public class HeadModule : BlankModule
{
    public ModuleBuilder moduleBuilder;
    public override void OnTriggerEnter2D(Collider2D other) {
        
        base.OnTriggerEnter2D(other);

        if(other.tag == "Energy"){

            //Debug.Log("Picked");
            Action();

        }

    }

    public override void Action(){


        ScoreManager.Instance.IncreaseScore();
        //moduleBuilder.AddModuleParts();


    }

        
}
