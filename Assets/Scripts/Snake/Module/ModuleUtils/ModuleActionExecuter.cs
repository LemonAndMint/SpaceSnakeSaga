using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ModuleManager;

public class ModuleActionExecuter : MonoBehaviour
{

    public List<Coroutine> actionCoroutines;

    private void _startAction(){

        //Debug.Log("Action birtch");
        //#TODO _actionRepeater yaz

        //Coroutine actionCoroutine = StartCoroutine();


    }

    private IEnumerator _actionRepeater(){

        while(true){

            


        }

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
