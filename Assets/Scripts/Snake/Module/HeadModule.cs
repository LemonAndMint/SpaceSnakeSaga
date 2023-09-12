using System.Collections;
using System.Collections.Generic;
using ModuleManager;
using UnityEngine;

public class HeadModule : BlankModule
{
    public ModuleBuilder moduleBuilder;

    private string _energyType = null;

    public override void OnTriggerEnter2D(Collider2D other) {
        
        base.OnTriggerEnter2D(other);

        if(other.tag.Contains("Energy")){

            _energyType = other.tag;
            Action();

        }

    }

    public override void Action(){

        ScoreManager.Instance.IncreaseScore();
        moduleBuilder.AddModulePart(_energyType);

    }

        
}
