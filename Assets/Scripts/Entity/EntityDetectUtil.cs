using System.Collections.Generic;
using System.Linq;
using EntityEnum;
using UnityEngine;

public class EntityDetectUtil : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static GameObject GetNearestEntity(GameObject currentEntity, Entity entityType ,float range){

        GameObject tempEnemy = _getNearestEntity(currentEntity.transform, entityType);

        if(_isInRange(currentEntity.transform, tempEnemy.transform, range)){

            return tempEnemy;

        }

        return null;

    }


    private static bool _isInRange(Transform currentEntity, Transform targetEntity, float range){

        if( targetEntity != null && Vector3.Distance(currentEntity.position, targetEntity.position) < range ){

            return true;

        }

        return false;

    }

    private static GameObject _getNearestEntity(Transform currentEntity, Entity entityType){ 

        List<GameObject> enemies = GameObject.FindGameObjectsWithTag(EntityUtil.EnumToString(entityType)).ToList();
        
        if(enemies == null){

            return null;

        }
      
        List<GameObject> distanceList = enemies.OrderBy(enemyGO => Vector3.Distance(currentEntity.position, enemyGO.transform.position)).ToList();

        if(distanceList != null && distanceList.Count > 0)
            return distanceList.First();

        return null;
       
    }
}
