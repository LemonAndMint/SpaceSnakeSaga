using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ModuleManager;

public class ModuleActionExecuter : MonoBehaviour
{

    public ModuleContainer moduleContainer;
    private Dictionary<BlankModule, Coroutine> actionCoroutines = new Dictionary<BlankModule, Coroutine>();

    private void _startAction(List<BlankModule> modules){

        foreach (BlankModule module in modules)
        {
            
            Coroutine actionCoroutine = StartCoroutine(_actionRepeater(module));
            actionCoroutines.Add(module, actionCoroutine);
            
        }

    }

    private void _stopAction(List<BlankModule> modules){

        foreach (BlankModule module in modules)
        {
            if(actionCoroutines.ContainsKey(module)){

                StopCoroutine(actionCoroutines[module]);
                actionCoroutines.Remove(module);

            }
            
        }

    }

    private IEnumerator _actionRepeater(BlankModule module){

        while(true){

            module.Action();
            yield return new WaitForSeconds(module.ActionCooldown);

        }

    }

    void Awake() //En önce listenerler atanmalı. Yoksa ilk eklenen modülün aksiyonu çalışmıyor.
    {

        moduleContainer.onActionModuleCreation.AddListener( _startAction);
        moduleContainer.onModuleDeletion.AddListener( _stopAction);


    }

    void Update()
    {
        
    }
}
