using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using EntityEnum;

public class WeaponModule : BlankModule
{
    public float range = 10f;
    public GameObject bulletPrefab;
    public Transform cannonBarrelTransform;
    public Entity enemy;

    [SerializeField]private int _damageValue;
    [SerializeField]private float _accuracyDeflection;

    private GameObject _currEnemy;
    public GameObject TargetedEnemy{

        get{ return _currEnemy; }

    }

    public override void Action()
    {

        _shoot();

    }

    private void _shoot(){

        GameObject tempEnemy = _getNearestEnemy();

        if(_isInRange(tempEnemy)){

            _currEnemy = tempEnemy;

            GameObject bullet = Instantiate(bulletPrefab, cannonBarrelTransform.transform.position, Quaternion.identity);
            
            if(!bullet.GetComponent<Bullet>()){ bullet.AddComponent<Bullet>(); }

            Vector3 tempTargetPos = new Vector3( _currEnemy.transform.position.x + Random.insideUnitSphere.x * _accuracyDeflection, 
                                                 _currEnemy.transform.position.y + Random.insideUnitSphere.y * _accuracyDeflection,
                                                 0);

            bullet.GetComponent<Bullet>().targetLastPosition = tempTargetPos;
            bullet.GetComponent<Bullet>().damage = _damageValue;
            bullet.GetComponent<Bullet>().onHit.AddListener(Destroy);

        }
        else{

            _currEnemy = null;

        }

    }

    private bool _isInRange(GameObject enemy){

        if( enemy != null && Vector3.Distance(transform.position, enemy.transform.position) < range ){

            return true;

        }

        return false;

    }

    private GameObject _getNearestEnemy(){ 

        List<GameObject> enemies = GameObject.FindGameObjectsWithTag(EntityUtil.EnumToString(enemy)).ToList();
        
        if(enemies == null){

            return null;

        }
      
        List<GameObject> distanceList = enemies.OrderBy(enemyGO => Vector3.Distance(transform.position, enemyGO.transform.position)).ToList();

        if(distanceList != null && distanceList.Count > 0)
            return distanceList.First();

        return null;
       
    }
  
}
