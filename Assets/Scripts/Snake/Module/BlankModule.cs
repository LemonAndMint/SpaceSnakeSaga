using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Module;
using Unity.VisualScripting;
using System.IO.Compression;
using ModuleManager;
using System.Threading.Tasks;
using System;

public class BlankModule : MonoBehaviour, IModule
{
    [SerializeField] private int _health;
    [SerializeField] private float _actionCooldown;

    public float ActionCooldown{

        get{

            return _actionCooldown;

        }

    }

    [SerializeField] private int _moduleCreationSecond;


    private int _currentHealth;
    private bool _isCreated = false;

    //protected bool isReady = true;

    private void Start() {

        _currentHealth = _health;
    
    }

    /// <summary>
    /// Modül oluşturur. <paramref name="_moduleCreationTime"/> saniye kadar bekler.
    /// </summary>
    /// <param name="creationFunction">Oluşturulduktan sonra ekstradan çalıştırılacak metodlar gönderilebilir</param>
    public void ModuleCreation(Action creationFunction){

        StartCoroutine(_moduleCreation(creationFunction));

    }

    private IEnumerator _moduleCreation(Action creationFunction){

        yield return new WaitForSeconds(_moduleCreationSecond);
        _isCreated = true;
        creationFunction();

        //#TODO diger islemler

    }

    /// <summary>
    /// Modül aksiyonları uygulanır. Metodu cooldown değerinden bağımsızdır.
    /// </summary>
    public virtual void Action(){ } //Bos modul icin aksiyon yok

    /// <summary>
    /// Modüllerin hasar almasını sağlar.
    /// </summary>
    /// <param name="damage">Verilecek hasarın değeri.</param>
    public virtual void GetHit(int damage)
    {
        if(!_isCreated){ Die(); }

        _currentHealth -= damage;
        if(_currentHealth < 0){ Die(); }

    }

    public virtual void Die()
    {
        
        Debug.Log("DIE");

    }

    public virtual void OnTriggerEnter2D(Collider2D other) {
        
        if(other.tag == "Bullet"){

            //GetHit(0);
            Debug.Log("Bullet");

        }

    }

}
