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
      
        List<GameObject> distanceList = enemies.OrderBy(enemyGO => Vector3.Distance(transform.position, enemyGO.transform.position)).ToList();

        return distanceList.First();
       
    }

  
}
