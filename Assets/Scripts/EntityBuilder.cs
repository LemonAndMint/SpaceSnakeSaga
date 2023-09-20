using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class EntityBuilder : MonoBehaviour
{

    private static EntityBuilder _entityBuilder;
    public static EntityBuilder Instance {

        get{

            
            return _entityBuilder;

        }

    }

    [SerializeField] private Transform _anchorPoint;
    public Transform AnchorPoint {

        get{

            return _anchorPoint;

        }

        set{

            if(_anchorPoint == null){

                _anchorPoint = value;

            }

        }

    }

    [Header("Creation")]
    [Space(10f)]

    [SerializeField] private int _enemyCount;
    [SerializeField] private int _energyCount;
    [SerializeField] private float _entityCreateRadius;
    [SerializeField] private float _entitySpacing;

    [Space(10f)]

    [SerializeField] private List<GameObject> _enemyGOPrefbList;
    [SerializeField] private List<GameObject> _energyGOPrefbList;

    [Header("Distance")]
    [Space(10f)]

    [SerializeField] private float _maxDistanceFromAnchor;
    [SerializeField] private float _entityDistanceCheckTime;

    private bool isStarted = false;

    private List<GameObject> _entityIngameGOList = new List<GameObject>();

    private void Awake() {

        if(_entityBuilder != null && _entityBuilder != this){

            Destroy(this.gameObject);

        }

        _entityBuilder = this;

    }

    /// <summary>
    /// Bir kere çalışır. Oyunun başında çevredeki nesneleri yaratır.
    /// </summary>
    public void StartEntityBuilder(){

        if(!isStarted){

            _entityBuilder = this;
        
            _setUp(_enemyGOPrefbList, _enemyCount, _addDieListener);
            _setUp(_energyGOPrefbList, _energyCount);

            StartCoroutine(_checkDistance());

            isStarted = true;

        }

    }

    public void ChangePosition(GameObject entityGO){

        _setRandomPosition(entityGO.transform);

    }

    private IEnumerator _checkDistance(){

        while(true){

            List<GameObject> farAwayEntities = _entityIngameGOList.Where( x => Vector3.Distance(x.transform.position, _anchorPoint.position) > _maxDistanceFromAnchor).ToList();
            if(farAwayEntities.Count > 0){

                foreach( GameObject entity in farAwayEntities ){

                    _setRandomPosition(entity.transform);

                }

            }            
            
            yield return new WaitForSeconds(_entityDistanceCheckTime);

        }

    }

    //Bir kere çalışır ve oyundaki tüm nesneleri oluşturur.
    private void _setUp(List<GameObject> entityPrefbList, int count){

        for (int i = 0; i < count; i++)
        {
            
            GameObject tempEntityGO = Instantiate(entityPrefbList[UnityEngine.Random.Range(0, entityPrefbList.Count)]);
            _setRandomPosition(tempEntityGO.transform);

            _entityIngameGOList.Add(tempEntityGO);

        }

    }

    private void _setUp(List<GameObject> entityPrefbList, int count, Action<ModuleHealth> creatFunc){

        for (int i = 0; i < count; i++)
        {
            
            GameObject tempEntityGO = Instantiate(entityPrefbList[UnityEngine.Random.Range(0, entityPrefbList.Count)]);
            _setRandomPosition(tempEntityGO.transform);

            if(tempEntityGO.GetComponent<ModuleHealth>())
                creatFunc(tempEntityGO.GetComponent<ModuleHealth>());

            _entityIngameGOList.Add(tempEntityGO);

        }

    }

    //Enemy için can modülündeki onDie delegate ayarlanır. 
    private void _addDieListener(ModuleHealth entityHealth){

        entityHealth.onDie.AddListener(ChangePosition);
        entityHealth.onDie.AddListener( (GameObject x) => entityHealth.Restore());

    }

    private void _setRandomPosition(Transform placingEntity){

        bool isFound = false;
        Vector3 tempPosition = Vector3.zero;

        while(!isFound){

            //Anchor çevresine yerleştir
            tempPosition = new Vector3( _anchorPoint.position.x + UnityEngine.Random.insideUnitSphere.x * _entityCreateRadius, 
                                        _anchorPoint.position.y + UnityEngine.Random.insideUnitSphere.y * _entityCreateRadius, 0);

            //İki obje birbirine çok yakın olmaması gerek.
            if(_entityIngameGOList.FirstOrDefault( x => Vector3.Distance(x.transform.position, tempPosition) < _entitySpacing ) == null){

                isFound = true;
                placingEntity.position = tempPosition;
                placingEntity.rotation = Quaternion.identity;

            }

        }

    }
}
