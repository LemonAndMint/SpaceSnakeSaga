using System.Collections;
using System.Collections.Generic;
using Module;
using UnityEngine;

public class ModulesActionExecuter : MonoBehaviour
{
    private List<dynamic> _modules = new List<dynamic>();

    /// <summary>
    /// Snake'e yeni modül ekleme. 
    /// </summary>
    /// <typeparam name="T">Herhangi bir modül tipi</typeparam>
    /// <param name="module"></param>
    public void AddModule<T>(T module) where T: IModule{

        _modules.Add(module);

    }

    private void _startAction(){

        

    }

    //#TODO Add remove

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
