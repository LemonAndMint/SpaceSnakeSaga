using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace ModuleManager
{
    public class ModuleContainer : MonoBehaviour
    {
        /// <summary>
        /// Bir modül oluşturulurken çalıştırılacak metotları tutan delegate.
        /// </summary>
        public UnityEvent<List<BlankModule>> onModuleCreation = new UnityEvent<List<BlankModule>>();
        /// <summary>
        /// Bir modül yokedilirken çalıştırılacak metotları tutan delegate.
        /// </summary>
        public UnityEvent<List<BlankModule>> onModuleDeletion = new UnityEvent<List<BlankModule>>();

        public GameObject snakePartPrefb;
        public GameObject moduleCreationPrefb;
        
        /// <summary>
        /// !Kesinlikle içinde bulundurduklaro GameObject'ler null olmamalı!
        /// </summary>
        public List<EnergyScriptableObject> energyDatas;

        private List<GameObject> _modules = new List<GameObject>();
        [SerializeField] private int _moduleCreationSecond = 1;

        private Type _moduleType = null;

        public int Count{

            get { return _modules.Count; }

        }

        /// <summary>
        /// Snake'ten modül alır.
        /// </summary>
        /// <param name="index"></param>
        /// <returns> <paramref name = "index"/> noktasındaki modülün GameObject'ini döndürür. </returns>
        public GameObject GetModule(int index){

            return _modules[index];

        }

        /// <summary>
        /// Snake'e T tipli modülü <paramref name="_moduleCreationSecond"/> saniye sonra ekler. 
        /// Eklenecek olan modülü zaman bitmeden metodu yeniden çağırarak değiştirebiliriz. 
        /// </summary>
        /// <typeparam name="T">Blank Module ve diğer tipleri olmak zorunda.</typeparam>
        /// <param name="moduleGO">Snake'e eklenecek modül.</param>
        /// <returns>Snake GameObject parçası.</returns>
        public GameObject AddModule(Type type){


            if(_moduleType == null){

                GameObject snakeBodyGO = _createSnakeBody();
                GameObject moduleCreationAnimation = _createModuleCreationAnimation();

                _changeModuleType(type);
                _modules.Add(snakeBodyGO);

                moduleCreationAnimation.transform.SetParent(snakeBodyGO.transform);

                _waitForConstruct(snakeBodyGO);

                return snakeBodyGO;

            }
            else {

                _changeModuleType(type);

            }

            return null;


        }

        private GameObject _createModuleCreationAnimation(){

            return Instantiate(moduleCreationPrefb, Vector3.zero, Quaternion.identity);

        }

        /// <summary>
        /// <paramref name="_moduleCreationSecond"/> saniye bekler. Modül oluşumu sırasında modül tipi birden fazla kere değişebileceği için bekleme süresi verildi.
        /// </summary>
        /// <param name="snakeBodyGO"></param>
        /// <returns></returns>
        private async Task _waitForConstruct(GameObject snakeBodyGO){

            await Task.Delay(_moduleCreationSecond * 1000);

            Destroy(snakeBodyGO.transform.GetChild(snakeBodyGO.transform.childCount));

            GameObject moduleGO = _createModule(_moduleType);

            if(moduleGO != null){

                moduleGO.transform.SetParent(snakeBodyGO.transform, false);

            }

            _moduleType = null;

            _moduleSets(moduleGO);

        }

        /// <summary>
        /// Snake'e direkt bir modül eklenir.
        /// </summary>
        /// <param name="moduleGO">Eklenecek modülün GameObject'i.</param>
        /// <returns>Snake GameObject parçası.</returns>
        public GameObject AddModule(GameObject moduleGO){

            GameObject snakeBodyGO = _createSnakeBody();

            _modules.Add(snakeBodyGO);
            moduleGO.transform.SetParent(snakeBodyGO.transform);

            _moduleSets(moduleGO);

            return snakeBodyGO;

        }

        private void _moduleSets(GameObject moduleGO){

            //Sadece BlankModule olan modüllerin aksiyonlarının otomatik olarak gerçekleştirilmesi için.
            if(moduleGO.GetComponentsInChildren<BlankModule>().Count() > 0 && moduleGO.GetComponentsInChildren<BlankPassiveModule>().Count() <= 0)
                onModuleCreation?.Invoke(moduleGO.GetComponentsInChildren<BlankModule>().ToList());

             //Modül oluşturulurken tek bir canı vardır. Oluşturulduktan sonra orjinal canına döner.
            if(moduleGO.GetComponent<ModuleHealth>()){
             
                moduleGO.GetComponentInChildren<ModuleHealth>().onDie.AddListener(RemoveModuleGO);
                moduleGO.GetComponent<ModuleHealth>().ModuleCreated();

            }


        }

        private GameObject _createModule(Type moduleType){

            GameObject moduleGOPrefb = energyDatas.Where( x => { 

                                                                    if(x.modelGameObject.GetComponentInChildren(moduleType)){

                                                                        return true;

                                                                    } 
                                                            
                                                                    return false;
                                                        
                                                                }).ToList().First().modelGameObject;

            return Instantiate(moduleGOPrefb, Vector3.zero, Quaternion.identity);

        } 


        private void _changeModuleType(Type newModuleType){

            _moduleType = newModuleType;

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

        /// <summary>
        /// Snake'teki modülleri <paramref name="_modules"/> listesindeki indexlerine göre yokeder.
        /// </summary>
        /// <param name="index"></param>
        public void RemoveModuleAt(int index){

            onModuleDeletion?.Invoke(_modules[index].GetComponentsInChildren<BlankModule>().ToList());

            Destroy(_modules[index]);

            _modules[index] = null; 
            _removeOtherModules();

        }
        /// <summary>
        /// Snake'teki modülleri <paramref name="_modules"/> listesindeki GameObject'lerine göre yokeder.
        /// </summary>
        /// <param name="moduleGameObject"></param>
        public void RemoveModuleGO(GameObject moduleGameObject){

            onModuleDeletion?.Invoke(moduleGameObject.GetComponentsInChildren<BlankModule>().ToList());

            Destroy(moduleGameObject);

            _modules[_modules.IndexOf(moduleGameObject)] = null;
            _removeOtherModules();
            
        }

        private void _removeOtherModules() {

            for (int i = 0; i < _modules.Count; i++)
            {

                if (GetModule(i) == null)
                {
                    int oldCount = _modules.Count;
                    for (int k = i; k < oldCount; k++)
                    {

                        Destroy(_modules[i]);
                        _modules.RemoveAt(i); 

                    }
                    
                }

            }

        }

    }

}
