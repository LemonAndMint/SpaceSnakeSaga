using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ModuleManager;


public class ModuleActionExecuter : MonoBehaviour
{

    private void _startAction(){

        

    }


    void Start()
    {

        ModuleContainer.onModuleCreation.AddListener( _startAction);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
