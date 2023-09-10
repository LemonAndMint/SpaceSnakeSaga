using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ModuleManager;

public class ModuleActionExecuter : MonoBehaviour
{

    //public List<BlankModule> module;

    private void _startAction(){

        //Debug.Log("Action birtch");
        //#TODO _actionRepeater yaz


    }

    /*private IEnumerator _actionRepeater(){



    }*/


    void Start()
    {

        ModuleContainer.onModuleCreation.AddListener( _startAction);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
