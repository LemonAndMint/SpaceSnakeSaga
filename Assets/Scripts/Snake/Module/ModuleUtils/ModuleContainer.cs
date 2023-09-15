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
        public UnityEvent<List<BlankModule>> onActionModuleCreation = new UnityEvent<List<BlankModule>>();

        public void InvoleActionCreation(List<BlankModule> modules){

            onActionModuleCreation?.Invoke(modules);

        }

        /// <summary>
        /// Bir modül yokedilirken çalıştırılacak metotları tutan delegate.
        /// </summary>
        public UnityEvent<List<BlankModule>> onModuleDeletion = new UnityEvent<List<BlankModule>>();

        public ModuleBuilder moduleBuilder;

        private List<GameObject> _modules = new List<GameObject>();


        public int Count{

            get { return _modules.Count; }

        }

        /// <summary>
        /// Snake'e modül ekler. Eklenen modül, SnakeBody GameObject'i dir.
        /// </summary>
        /// <param name="snakeBodyGO"></param>
        public void Add(GameObject snakeBodyGO){

            _modules.Add(snakeBodyGO);

        }

        /// <summary>
        /// Snake'ten modül alır.
        /// </summary>
        /// <param name="index"></param>
        /// <returns> <paramref name = "index"/> noktasındaki modülün SnakeBody GameObject'ini döndürür. </returns>
        public GameObject Get(int index){

            return _modules[index];

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
        /// <param name="snakeBodyGO"></param>
        public void RemoveModuleGO(GameObject snakeBodyGO){

            onModuleDeletion?.Invoke(snakeBodyGO.GetComponentsInChildren<BlankModule>().ToList());

            Destroy(snakeBodyGO);

            _modules[_modules.IndexOf(snakeBodyGO)] = null;
            _removeOtherModules();
            
        }

        private void _removeOtherModules() {

            for (int i = 0; i < _modules.Count; i++)
            {

                if (Get(i) == null)
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
