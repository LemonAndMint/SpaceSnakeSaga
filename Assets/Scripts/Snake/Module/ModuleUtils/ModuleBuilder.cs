using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using Cinemachine;

namespace ModuleManager
{

    //https://www.youtube.com/watch?v=sPlcecIh3ik&ab_channel=RandomArtAttack
    //https://www.youtube.com/watch?v=A-SZDQIDXXI&ab_channel=RandomArtAttack

    [RequireComponent(typeof(ModuleContainer))]
    public class ModuleBuilder : MonoBehaviour
    {

        public ModuleContainer moduleContainer;
        public CameraActions cameraActions;

        public UnityAction<GameObject> onGameOver;
        
        [SerializeField] private float _distanceBetween = .2f;
        [SerializeField] private int _moduleCreationSecond = 1;

        [SerializeField] private List<GameObject> _addingModuleParts = new List<GameObject>();
        
        /// <summary>
        /// !Kesinlikle içinde bulundurduklaro GameObject'ler null olmamalı!
        /// </summary>
        public List<EnergyScriptableObject> energyDatas;

        public GameObject snakePartPrefb;
        public GameObject moduleCreationPrefb;

        private float _countUp = 0;
        private Type _moduleType = null;


        void Start()
        {

            if(moduleContainer == null)
                moduleContainer = GetComponent<ModuleContainer>();

            _createModuleParts();

        }

        private void Update()
        {

            _manageSnakeBody();

        }

// ---- TIMED MODULE CONSTRUCTION ----

        /// <summary>
        /// Yeni modül, string olarak alınır GameObject olarak eklenir. Eklenme <paramref name="_moduleCreationSecond" />. saniye sonra yapılır.
        /// Eklenecek olan modülü zaman bitmeden metodu yeniden çağırarak değiştirebiliriz. 
        /// </summary>
        /// <param name="moduleGO">Snake'e eklenecek modül.</param>
        /// <returns>Snake GameObject parçası.</returns>
        public void AddModulePart(string typeName)
        {
            
            if( _moduleType == null )
                _constructModule();
            
            _moduleType = _getType(typeName);
        
        }

        /// <summary>
        /// <paramref name="_moduleCreationSecond"/> saniye bekler. Modül oluşumu sırasında modül tipi birden fazla kere değişebileceği için bekleme süresi verildi.
        /// </summary>
        /// <param name="snakeBodyGO"></param>
        /// <returns></returns>
        private async Task _constructModule(){

            if(moduleContainer.Count > 0){

                MarkerStorage markM = moduleContainer.Get(moduleContainer.Count - 1).GetComponent<MarkerStorage>();
                markM.ClearMarkerList();

            }

            await Task.Delay((int)(_distanceBetween * 1000));

            GameObject snakeBodyGO = Instantiate(snakePartPrefb, transform.position, transform.rotation);

            if (!snakeBodyGO.GetComponent<MarkerStorage>()) { snakeBodyGO.AddComponent<MarkerStorage>(); }

            if (!snakeBodyGO.GetComponent<Rigidbody2D>())
            {

                snakeBodyGO.AddComponent<Rigidbody2D>();
                snakeBodyGO.GetComponent<Rigidbody2D>().gravityScale = 0;

            }

            GameObject moduleCreationAnimation = _createModuleCreationAnimation();
            _moduleSets(moduleCreationAnimation);

            moduleContainer.Add(snakeBodyGO);

            moduleCreationAnimation.transform.SetParent(snakeBodyGO.transform, false);

            await Task.Delay(_moduleCreationSecond * 1000);

            //Construction animasyonunu yok ediyoruz.
            Destroy(snakeBodyGO.transform.GetChild(snakeBodyGO.transform.childCount - 1).gameObject);

            GameObject moduleGO = _createModule(_moduleType);

            if(moduleGO != null){

                moduleGO.transform.SetParent(snakeBodyGO.transform, false);

            }

            _moduleType = null;

            if(moduleContainer.Count % 5 == 0 && cameraActions.GetComponent<CinemachineVirtualCamera>().m_Lens.OrthographicSize < 19)
                cameraActions.ZoomOut();

        }

        private Type _getType(string typeName){

            Type type = null;

            switch (typeName)
            {

                case "Standart Energy":
                    type = typeof(StandartModule);
                    break; 

                case "Weapon Energy":
                    type = typeof(WeaponModule);
                    break;

                default:
                    break;

            }

            return type;

        }

// ---- TIMED MODULE CONSTRUCTION ----

// ---- IMMEDIATE MODULE CONSTRUCTION ----

        private void _createModuleParts()
        {

            if(moduleContainer.Count == 0){ 
                
                GameObject snakeBodyGO = _createSnakeBody();
                _instantiateModule(snakeBodyGO); 
                
            }

            MarkerStorage markM = moduleContainer.Get(moduleContainer.Count - 1).GetComponent<MarkerStorage>();
            if(_countUp == 0) { markM.ClearMarkerList(); }

            _countUp += Time.deltaTime;

            if(_countUp >= _distanceBetween) {

                GameObject snakeBodyGO = _createSnakeBody();
                _instantiateModule(snakeBodyGO); 

                snakeBodyGO.GetComponent<MarkerStorage>().ClearMarkerList();
                _countUp = 0;

            }

        }

        private void _manageSnakeBody()
        {

            if(_addingModuleParts.Count > 0){ _createModuleParts(); }

        }

        /// <summary>
        /// Anında modül oluşturur.
        /// </summary>
        private void _instantiateModule(GameObject snakeBodyGO)
        {

            GameObject moduleGO = Instantiate(_addingModuleParts.First(), transform.position, transform.rotation);

            _moduleSets(moduleGO);
            moduleContainer.Add(snakeBodyGO);

            moduleGO.transform.SetParent(snakeBodyGO.transform, false);
            _addingModuleParts.RemoveAt(0);

        }

        private GameObject _createSnakeBody(){

            GameObject snakeBodyGO = Instantiate(snakePartPrefb, transform.position, transform.rotation);

            if (!snakeBodyGO.GetComponent<MarkerStorage>()) { snakeBodyGO.AddComponent<MarkerStorage>(); }

            if (!snakeBodyGO.GetComponent<Rigidbody2D>())
            {

                snakeBodyGO.AddComponent<Rigidbody2D>();
                snakeBodyGO.GetComponent<Rigidbody2D>().gravityScale = 0;

            }

            return snakeBodyGO;

        }

// ---- IMMEDIATE MODULE CONSTRUCTION ----

        private GameObject _createModuleCreationAnimation(){

            GameObject moduleCreatGO = Instantiate(moduleCreationPrefb, Vector3.zero, Quaternion.identity);

            if(moduleCreatGO.GetComponentInChildren<Animator>()){

                float animationSpeed = moduleCreatGO.GetComponentInChildren<Animator>().GetCurrentAnimatorClipInfo(0)[0].clip.length;

                moduleCreatGO.GetComponentInChildren<Animator>().SetFloat("Speed", animationSpeed / _moduleCreationSecond);

            }

            return moduleCreatGO;

        }

        private GameObject _createModule(Type moduleType){

            GameObject moduleGOPrefb = energyDatas.Where( x => { 

                                                                    if(x.modelGameObject.GetComponentInChildren(moduleType)){

                                                                        return true;

                                                                    } 
                                                            
                                                                    return false;
                                                        
                                                                }).ToList().First().modelGameObject;

            GameObject moduleGO = Instantiate(moduleGOPrefb, Vector3.zero, Quaternion.identity);

            _moduleSets(moduleGO);

            return moduleGO;

        } 

        private void _moduleSets(GameObject moduleGO){

            //Sadece BlankModule olan modüllerin aksiyonlarının otomatik olarak gerçekleştirilmesi için.
            if(moduleGO.GetComponentsInChildren<BlankModule>().Count() > 0 && moduleGO.GetComponentsInChildren<BlankPassiveModule>().Count() <= 0)
                moduleContainer.InvoleActionCreation(moduleGO.GetComponentsInChildren<BlankModule>().ToList());

            //Modül oluşturulurken tek bir canı vardır. Oluşturulduktan sonra orjinal canına döner.
            if(moduleGO.GetComponent<ModuleHealth>())
                moduleGO.GetComponentInChildren<ModuleHealth>().onDie.AddListener(moduleContainer.RemoveModuleGO);
                moduleGO.GetComponentInChildren<ModuleHealth>().onGetHit.AddListener( (GameObject x) => CameraActions.Instance.Shake() );


            //Head modülü için özel atamalar.
            if(moduleGO.GetComponent<HeadModule>() && moduleGO.GetComponent<ModuleHealth>()){
                
                moduleGO.GetComponent<HeadModule>().moduleBuilder = this;
                moduleGO.GetComponent<ModuleHealth>().onDie.AddListener(onGameOver);

                cameraActions.virtualCamera.Follow = moduleGO.transform; //Kamera takibi
                
            }

        }

        

    }

}

