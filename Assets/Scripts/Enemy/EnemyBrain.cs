using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyBrain : MonoBehaviour
{
    
    public WeaponModule weaponModule;
    public EnemyMovement enemyMovement;

    [SerializeField]private int raysToShoot = 6;
    [SerializeField]private float _actionCooldown;
    [SerializeField]private float _detectCooldown;

    private Coroutine actionRepeater;
    private Coroutine detectRepeater;

    void Start()
    {
        
        if(weaponModule == null)
            this.AddComponent<WeaponModule>();

        actionRepeater = StartCoroutine(_actionRepeater());
        detectRepeater = StartCoroutine(_detectRepeater());
        
        if(GetComponent<ModuleHealth>()){

            GetComponent<ModuleHealth>().onDie.AddListener(_stopRepeaters);

        }

    }

    private IEnumerator _actionRepeater(){

        while(true){

            weaponModule.Action();
            yield return new WaitForSeconds(_actionCooldown);

        }

    }

    private IEnumerator _detectRepeater(){

        while(true){

            _detectEnviroment();
            yield return new WaitForSeconds(_detectCooldown);

        }

    }

    private void _stopRepeaters(GameObject modelGameObject){

        StopCoroutine(actionRepeater);
        StopCoroutine(detectRepeater);

        Destroy(modelGameObject);

    }

    //https://discussions.unity.com/t/raycasting-around-an-object-in-360-degrees/72772
    private void _detectEnviroment(){

        float angle = 0;

        List<Vector3> directions = new List<Vector3>();

        for(int i = 0; i < raysToShoot; i++) {

            float x = Mathf.Sin(angle);
            float y = Mathf.Cos(angle);

            angle += 2 * Mathf.PI / raysToShoot;

            Vector3 startPoint = new Vector3(transform.position.x + x * 0.35f, transform.position.y + y * 0.35f, 0);
            Vector3 dir = new Vector3(x * 2, y * 2, 0);
            
            Debug.DrawRay(startPoint, dir, Color.red);

            if(Physics2D.Raycast(startPoint, dir, 2f)) {

                Debug.DrawRay(startPoint, dir, Color.blue);
                directions.Add( -1 * dir );

            }

        }

        //Tüm yönler bir araya getirilip gidiş yönü hesaplanır. Hesaplama, 2 vektör arasındaki ara vektör bulma yöntemine dayanmaktadır.
        Vector3 moveDirection = directions.Count > 0 ? directions[0] : Vector3.zero;

        for(int i = 1; i < directions.Count; i += 2)
        {

            moveDirection += directions[i];
            moveDirection *= 0.5f;

        }

        enemyMovement.targetDirect = moveDirection;
        Debug.DrawRay(transform.position, moveDirection, Color.green);

    }

}
