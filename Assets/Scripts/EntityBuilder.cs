using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    [SerializeField] private int _asteroidCount;

    [Space(5f)]

    [SerializeField] private float _entityCreateRadius;
    [SerializeField] private float _entitySpacing;

    [Space(5f)]
    [SerializeField] private float _maxTries;

    [Space(10f)]

    [SerializeField] private List<GameObject> _enemyGOPrefbList;
    [SerializeField] private List<GameObject> _energyGOPrefbList;
    [SerializeField] private List<GameObject> _asteroidGOPrefbList;

    [Header("Distance")]
    [Space(10f)]

    [SerializeField] private float _maxDistanceFromAnchor;
    [SerializeField] private float _minDistanceFromAnchor;
    [SerializeField] private float _entityDistanceCheckTime;
    [SerializeField] private float _asteroidMaxDestinationRadius;
    [SerializeField] private float _asteroidSizeDeflection;

    private List<GameObject> _entityIngameGOList = new List<GameObject>();

    private void Awake() {

        if(_entityBuilder != null && _entityBuilder != this){

            Destroy(this.gameObject);

        }

        _entityBuilder = this;

    }

    /// <summary>
    /// Oyunun başında çevredeki nesneleri yaratır.
    /// </summary>
    public void StartEntityBuilder(){

        _entityBuilder = this;
    
        _setUp<ModuleHealth>(_enemyGOPrefbList, _enemyCount, _addDieListener);
        _setUp<Bullet>(_asteroidGOPrefbList, _asteroidCount, _asteroid);
        
        _setUp(_energyGOPrefbList, _energyCount);

        StartCoroutine(_checkDistance());


    }

    public void RepositionEntities(){

        foreach (GameObject entity in _entityIngameGOList)
        {
            
            ChangePosition(entity);

        }

    }

    public void ChangePosition(GameObject entityGO){

        _setRandomPosition(entityGO.transform);

    }

    public void DestroyAllObjects(){

        foreach (GameObject entityGO in _entityIngameGOList)
        {
            Destroy(entityGO);
            
        }

        _entityIngameGOList.RemoveRange(0, _entityIngameGOList.Count);

    }

    public void AddEntity(int enemyCount, int energyCount, int asteroidCount){

        _setUp<ModuleHealth>(_enemyGOPrefbList, enemyCount, _addDieListener);
        _setUp<Bullet>(_asteroidGOPrefbList, asteroidCount, _asteroid);
        
        _setUp(_energyGOPrefbList, energyCount);

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

    private void _setUp<T>(List<GameObject> entityPrefbList, int count, Action<T> creatFunc){

        for (int i = 0; i < count; i++)
        {
            
            GameObject tempEntityGO = Instantiate(entityPrefbList[UnityEngine.Random.Range(0, entityPrefbList.Count)]);
            _setRandomPosition(tempEntityGO.transform);

            if(tempEntityGO.GetComponent(typeof(T)))//#FIXME ?????
                creatFunc(tempEntityGO.GetComponent<T>());

            _entityIngameGOList.Add(tempEntityGO);

        }

    }

    //Enemy için can modülündeki onDie delegate ayarlanır. 
    private void _addDieListener(ModuleHealth entityHealth){ //#FIXME ?????

        entityHealth.onDie.AddListener(ChangePosition);
        entityHealth.onDie.AddListener( (GameObject x) => entityHealth.Restore() );

    }

    private void _asteroid(Bullet asteroid){

        asteroid.targetLastPosition = _randomPosition(_asteroidMaxDestinationRadius);  
        asteroid.onHit.AddListener(ChangePosition);    

        float sizeDeflection = UnityEngine.Random.Range(-_asteroidSizeDeflection, _asteroidSizeDeflection);

        asteroid.transform.localScale = new Vector3( asteroid.transform.localScale.x + sizeDeflection, 
                                                     asteroid.transform.localScale.y + sizeDeflection, 
                                                     asteroid.transform.localScale.y + sizeDeflection );


    }

    private Vector3 _randomPosition(float maxValue){


        float x = (UnityEngine.Random.insideUnitSphere.x * maxValue + _minDistanceFromAnchor) *  Mathf.Sign(UnityEngine.Random.Range(-1, 1));
        float y = (UnityEngine.Random.insideUnitSphere.y * maxValue + _minDistanceFromAnchor) *  Mathf.Sign(UnityEngine.Random.Range(-1, 1));

        //Anchor çevresine yerleştir
        Vector3 tempPosition = new Vector3( _anchorPoint.position.x + x, 
                                            _anchorPoint.position.y + y, 0 );
                                            

        return tempPosition;

    }

    private void _setRandomPosition(Transform placingEntity){

        float maxTries = 0;
        Vector3 tempPosition = Vector3.zero;

        while(maxTries < _maxTries){

            tempPosition = _randomPosition(_entityCreateRadius);

            //İki obje birbirine çok yakın olmaması gerek.
            if(_entityIngameGOList.FirstOrDefault( x => Vector3.Distance(x.transform.position, tempPosition) < _entitySpacing ) == null){

                placingEntity.position = tempPosition;
                placingEntity.rotation = Quaternion.identity;
                return;

            }

            maxTries++;

        }

        //Sonsuz döngüye girmemesi için maksimum deneme sayısı ayarladık, eğer hiçbir yer bulamadıysa rastgele bir yere atar.
        placingEntity.position = _randomPosition(_entityCreateRadius);
        placingEntity.rotation = Quaternion.identity;

    }
}
