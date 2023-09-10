using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.Events;

namespace ModuleManager
{
    public static class ModuleContainer
    {
        /// <summary>
        /// Bir modül oluşturulurken çalıştırılacak metotları tutan delegate.
        /// </summary>
        public static UnityEvent onModuleCreation = new UnityEvent();
        private static List<GameObject> _modules = new List<GameObject>();
        public static int Count{

            get { return _modules.Count; }

        }
        
        /// <summary>
        /// Snake'ten modül alır.
        /// </summary>
        /// <param name="index"></param>
        /// <returns> <paramref name = "index"/> noktasındaki modülün GameObject'ini döndürür. </returns>
        public static GameObject GetModule(int index){

            return _modules[index];

        }

        /// <summary>
        /// Snake'teki modüllerin GameObject bilgilerini tutar.
        /// </summary>
        public static void AddModule(GameObject obj){

            _modules.Add(obj);

            if(obj.GetComponent<BlankModule>() == null){ obj.AddComponent<BlankModule>(); }
            obj.GetComponent<BlankModule>().ModuleCreation( () =>  onModuleCreation?.Invoke() ); 

        }

        /// <summary>
        /// Snake'teki modülleri yokeder.
        /// </summary>
        /// <param name="index"></param>
        public static void RemoveModuleAt(int index){

            _modules.RemoveAt(index);

        }

    }

    //https://www.youtube.com/watch?v=sPlcecIh3ik&ab_channel=RandomArtAttack
    //https://www.youtube.com/watch?v=A-SZDQIDXXI&ab_channel=RandomArtAttack

    public class ModuleBuilder : MonoBehaviour
    {
        
        [SerializeField] private float _distanceBetween = .2f;
        [SerializeField] private List<GameObject> _addingModuleParts = new List<GameObject>();

        /// <summary>
        /// Yeni modül eklemek için çağrılır. Oluşturulacak olan modülün prefabı bir oluşturucu listesine girer. Ardından modül oluşturulma animasyonu başlar.
        /// </summary>
        /// <param name="modulGO">Oluşturulacak modülün GameObject'i </param>
        public void AddModuleParts(GameObject modulGO)
        {
            
            _addingModuleParts.Add(modulGO);
        
        }
        
        float _countUp = 0;
        void Start()
        {

            _CreateModuleParts();

        }

        private void Update()
        {

            _ManageSnakeBody();

        }

        private void _CreateModuleParts()
        {

            if(ModuleContainer.Count == 0){ _InstantiateModule(); }

            MarkerStorage markM = ModuleContainer.GetModule(ModuleContainer.Count - 1).GetComponent<MarkerStorage>();
            if(_countUp == 0) { markM.ClearMarkerList(); }

            _countUp += Time.deltaTime;

            if(_countUp >= _distanceBetween) {

                GameObject tempModule = _InstantiateModule();
                tempModule.GetComponent<MarkerStorage>().ClearMarkerList();
                _countUp = 0;

            }


        }

        private void _ManageSnakeBody()
        {

            if (_addingModuleParts.Count > 0){ _CreateModuleParts(); }

            for (int i = 0; i < ModuleContainer.Count; i++)
            {

                if (ModuleContainer.GetModule(i) == null){ ModuleContainer.RemoveModuleAt(i); i--; }

            }
            if(ModuleContainer.Count == 0) { Destroy(this); }

        }

        private GameObject _InstantiateModule()
        {

            GameObject temp = Instantiate(_addingModuleParts.First(), transform.position, transform.rotation, transform);

            if (!temp.GetComponent<MarkerStorage>()) { temp.AddComponent<MarkerStorage>(); }

            if (!temp.GetComponent<Rigidbody2D>())
            {

                temp.AddComponent<Rigidbody2D>();
                temp.GetComponent<Rigidbody2D>().gravityScale = 0;

            }

            _addingModuleParts.RemoveAt(0);
            ModuleContainer.AddModule(temp);

            return temp;

        }

    }

}

