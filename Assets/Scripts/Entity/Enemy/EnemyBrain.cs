using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyBrain : MonoBehaviour
{
    
    public List<WeaponModule> weaponModuleList;
    public EnemyMovement enemyMovement;

    [SerializeField]private int raysToShoot = 6;
    [SerializeField]private float _actionCooldown; //override weapon module cooldowns;
    [SerializeField]private float _maxCooldownDeflection;
    [SerializeField]private float _detectCooldown;
    [SerializeField]private float _detectRadius = 2f;

    void Start()
    {

        _statRandomizer();

        StartCoroutine(_actionRepeater());
        StartCoroutine(_detectRepeater());
        
    }

    private void _statRandomizer(){

        _actionCooldown = Random.Range(_actionCooldown - _maxCooldownDeflection, _actionCooldown + _maxCooldownDeflection);
        
        if(GetComponent<EnemyMovement>())
            GetComponent<EnemyMovement>().RandomSpeed();

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

    private IEnumerator _detectRepeater(){

        while(true){

            _detectEnviroment();
            yield return new WaitForSeconds(_detectCooldown);

        }

    }

    //https://discussions.unity.com/t/raycasting-around-an-object-in-360-degrees/72772
    private void _detectEnviroment(){

        float angle = 0;

        List<Vector3> directions = new List<Vector3>();

        for(int i = 0; i < raysToShoot; i++) {

            float x = Mathf.Sin(angle);
            float y = Mathf.Cos(angle);

            angle += 2 * Mathf.PI / raysToShoot;

            float startDistance = (GetComponent<BoxCollider2D>().size.x * Mathf.Sqrt(2) / 2f) + 0.05f;

            Vector3 startPoint = new Vector3(transform.position.x + x * startDistance, transform.position.y + y * startDistance, 0);
            Vector3 dir = new Vector3(x * _detectRadius, y * _detectRadius, 0);
            
            if(Physics2D.Raycast(startPoint, dir, _detectRadius)) {

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

    }

}
