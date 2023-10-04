using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreatStarBrain : MonoBehaviour //#FIXME make brains child?
{
    public List<WeaponModule> weaponModuleList;
    [SerializeField]private float _actionCooldown; //override weapon module cooldowns
    // Start is called before the first frame update
    void Start()
    {
        
        StartCoroutine(_actionRepeater());

    }

    // Update is called once per frame
    void Update()
    {
        
        

    }

    private IEnumerator _actionRepeater(){

        while(true){

            foreach (WeaponModule weaponModule in weaponModuleList)
            {

                weaponModule.Action();
                yield return new WaitForSeconds(0.2f);
                
            }
            yield return new WaitForSeconds(_actionCooldown);

        }

    }

    


}
