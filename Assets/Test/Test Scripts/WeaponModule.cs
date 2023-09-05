using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Module;
using Unity.Mathematics;
using System.Linq;
using Unity.VisualScripting;

public class WeaponModule : BlankModule
{
    public float range = 10f;
    public GameObject BulletPrefab;
    public Transform tempGO;

    public Transform CannonBarrelTransform;

    private GameObject _currEnemy;
    public GameObject TargetedEnemy{

        get{ return _currEnemy; }

        set{}

    }

    private void Update() {
        
        Action();

    }


    public override void Action()
    {

        _shoot();

    }

    private void _shoot(){

        GameObject tempEnemy = _getNearestEnemy();

        _currEnemy = tempEnemy;

        if(_isInRange(tempEnemy)){

            _currEnemy = tempEnemy;

            /*GameObject bullet = Instantiate(BulletPrefab, CannonBarrelTransform.transform.position, Quaternion.identity);
            
            if(!bullet.GetComponent<Bullet>()){ bullet.AddComponent<Bullet>(); }

            bullet.GetComponent<Bullet>().targetTransform = _currEnemy.transform;*/

        }

    }

    private bool _isInRange(GameObject enemy){

        if( enemy != null && Vector3.Distance( transform.position, enemy.transform.position) < range ){

            return true;

        }

        return false;

    }

    private GameObject _getNearestEnemy(){ 

        List<GameObject> enemies = GameObject.FindGameObjectsWithTag("Enemy").ToList();
        
        if(enemies == null){

            return null;

        }

        List<GameObject> angleList = enemies.Where(enemyGO =>   {
                                                                    //https://forum.unity.com/threads/get-angle-in-degress-between-two-vectors.929190/
                                                                    float angle = Mathf.Rad2Deg * Mathf.Atan2( enemyGO.transform.position.y - transform.position.y, 
                                                                                                               enemyGO.transform.position.x - transform.position.x ) - 90;
                                                                    Debug.Log((90 * Mathf.Sign(transform.localPosition.y)) + (tempGO.rotation.eulerAngles.z % 90));
                                                                    Debug.Log(enemyGO.name + " angle " + angle);

                                                                    //Cannonun sagda ya da solda olup olmadigi cannonun y pozisyonunun isaretine gore belirlendik.
                                                                    //Bu sekilde tek satirda iki tarafin da acilarini kontrol edebildik. 
                                                                    //Sag taraf icin 90 < angle < -90 iken sol taraf icin -90 < angle < 90 dir.
                                                                    //Debug.Log(transform.position.y)        ;
                                                                    if(angle < (90 * Mathf.Sign(transform.localPosition.y)) + (tempGO.rotation.eulerAngles.z % 90)){

                                                                        return true;

                                                                    }
                                                                    return false;

                                                                }).ToList();

        List<GameObject> distanceList = angleList.OrderBy(enemyGO => Vector3.Distance(transform.position, enemyGO.transform.position)).ToList();


        //Debug.Log(" object: "+ name + " distance " + distanceList.First().name + " angle " + angleList.First().name + "  ");

        return distanceList.First();
       
    }

  
}
