using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace ModuleManager
{

    //https://www.youtube.com/watch?v=sPlcecIh3ik&ab_channel=RandomArtAttack
    //https://www.youtube.com/watch?v=A-SZDQIDXXI&ab_channel=RandomArtAttack

    [RequireComponent(typeof(ModuleContainer))]
    public class ModuleBuilder : MonoBehaviour
    {

        public ModuleContainer moduleContainer;

        public UnityAction<GameObject> onGameOver;
        
        [SerializeField] private float _distanceBetween = .2f;
        [SerializeField] private List<GameObject> _addingModuleParts = new List<GameObject>();

        float _countUp = 0;

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


        /// <summary>
        /// Yeni modül, GameObject olarak alınır ve eklenir. Eklenme işlemi hemen gerçekleşir.
        /// </summary>
        public void AddModulePart(GameObject modulePrefb)
        {
            
            _addingModuleParts.Add(modulePrefb);
        
        }

        /// <summary>
        /// Yeni modül, string olarak alınır GameObject olarak eklenir. Eklenme <paramref name="_moduleCreationSecond" />. saniye sonra yapılır.
        /// </summary>
        public void AddModulePart(string typeName)
        {
            Type tempType = _getType(typeName);
            if( tempType != null ){

                moduleContainer.AddModule(tempType);

            }
        
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

        private void _createModuleParts()
        {

            if(moduleContainer.Count == 0){ _instantiateModule(); }

            MarkerStorage markM = moduleContainer.GetModule(moduleContainer.Count - 1).GetComponent<MarkerStorage>();
            if(_countUp == 0) { markM.ClearMarkerList(); }

            _countUp += Time.deltaTime;

            if(_countUp >= _distanceBetween) {

                GameObject tempModule = _instantiateModule();
                
                tempModule.GetComponent<MarkerStorage>().ClearMarkerList();
                _countUp = 0;

            }

        }

        private void _manageSnakeBody()
        {

            if(_addingModuleParts.Count > 0){ _createModuleParts(); }

        }

        private GameObject _instantiateModule()
        {

            GameObject tempModuleGO = Instantiate(_addingModuleParts.First(), transform.position, transform.rotation);

            if(tempModuleGO.GetComponent<HeadModule>() && tempModuleGO.GetComponent<ModuleHealth>()){
                
                tempModuleGO.GetComponent<HeadModule>().moduleBuilder = this;
                tempModuleGO.GetComponent<ModuleHealth>().onDie.AddListener(onGameOver);
                
            }

            GameObject snakeBodyGO = moduleContainer.AddModule(tempModuleGO);
            _addingModuleParts.RemoveAt(0);

            return snakeBodyGO;

        }

    }

}

