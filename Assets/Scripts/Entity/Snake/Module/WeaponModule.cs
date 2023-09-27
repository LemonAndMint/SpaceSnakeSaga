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

        GameObject tempEnemy = EntityDetectUtil.GetNearestEntity(gameObject, enemy, range);

        if(tempEnemy != null){

            _currEnemy = tempEnemy;

            GameObject bullet = Instantiate(bulletPrefab, cannonBarrelTransform.transform.position, Quaternion.identity);
            
            if(!bullet.GetComponent<Bullet>()){ bullet.AddComponent<Bullet>(); }

            Vector3 tempTargetPos = new Vector3( _currEnemy.transform.position.x + Random.insideUnitSphere.x * _accuracyDeflection, 
                                                 _currEnemy.transform.position.y + Random.insideUnitSphere.y * _accuracyDeflection,
                                                 0);

            bullet.GetComponent<Bullet>().targetLastPosition = tempTargetPos;
            bullet.GetComponent<Bullet>().damage = _damageValue;
            bullet.GetComponent<Bullet>().onHit.AddListener(Destroy);

            if(GetComponentInChildren<AudioSource>() != null)
                GetComponentInChildren<AudioSource>().Play();

        }
        else{

            _currEnemy = null;

        }

    }
  
}
