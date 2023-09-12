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

    private int _currentHealth;
    private bool _isCreated = false;

    //protected bool isReady = true;

    private void Start() {

        _currentHealth = _health;
    
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
