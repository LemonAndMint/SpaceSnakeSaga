using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ModuleManager;
using UnityEngine;
using UnityEngine.Events;

namespace ModuleManager
{
    public class ModuleContainer : MonoBehaviour
    {
        /// <summary>
        /// Bir modül oluşturulurken çalıştırılacak metotları tutan delegate.
        /// </summary>
        public UnityEvent onModuleCreation = new UnityEvent();
        public GameObject SnakePartPrefb;
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

                _changeModuleType(type);
                _modules.Add(snakeBodyGO);

                _waitForConstruct(snakeBodyGO);

                return snakeBodyGO;

            }
            else {

                _changeModuleType(type);

            }

            return null;


        }

        /// <summary>
        /// <paramref name="_moduleCreationSecond"/> saniye bekler. Modül oluşumu sırasında modül tipi birden fazla kere değişebileceği için bekleme süresi verildi.
        /// </summary>
        /// <param name="snakeBodyGO"></param>
        /// <returns></returns>
        private async Task _waitForConstruct(GameObject snakeBodyGO){

            await Task.Delay(_moduleCreationSecond * 1000);

            GameObject moduleGO = _createModule(_moduleType);

            if(moduleGO != null){

                moduleGO.transform.SetParent(snakeBodyGO.transform, false);

            }
            _moduleType = null;

            onModuleCreation?.Invoke();

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

            onModuleCreation?.Invoke();

            return snakeBodyGO;

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

            GameObject snakeBodyGO = Instantiate(SnakePartPrefb, transform.position, transform.rotation, transform);

            if (!snakeBodyGO.GetComponent<MarkerStorage>()) { snakeBodyGO.AddComponent<MarkerStorage>(); }

            if (!snakeBodyGO.GetComponent<Rigidbody2D>())
            {

                snakeBodyGO.AddComponent<Rigidbody2D>();
                snakeBodyGO.GetComponent<Rigidbody2D>().gravityScale = 0;

            }

            return snakeBodyGO;

        }

        /// <summary>
        /// Snake'teki modülleri yokeder.
        /// </summary>
        /// <param name="index"></param>
        public void RemoveModuleAt(int index){

            _modules.RemoveAt(index);

        }

    }

}
