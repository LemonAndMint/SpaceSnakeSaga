using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyBrain : MonoBehaviour
{
    
    public WeaponModule weaponModule;
    [SerializeField]private float _actionCooldown;
    private Coroutine actionRepeater;

    void Start()
    {
        
        if(weaponModule == null)
            this.AddComponent<WeaponModule>();

        actionRepeater = StartCoroutine(_actionRepeater());
        
        if(GetComponent<ModuleHealth>()){

            GetComponent<ModuleHealth>().onDie.AddListener(_stopRepeater);
            GetComponent<ModuleHealth>().ModuleCreated();

        }

    }

    private IEnumerator _actionRepeater(){

        while(true){

            weaponModule.Action();
            yield return new WaitForSeconds(_actionCooldown);

        }

    }

    private void _stopRepeater(GameObject modelGameObject){

        StopCoroutine(actionRepeater);
        Destroy(modelGameObject);

    }

}