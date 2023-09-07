using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Module;
using Unity.VisualScripting;

public class BlankModule : MonoBehaviour, IModule
{
    [SerializeField] private int _health;
    [SerializeField] private float _actionCooldown;

    private int _currentHealth;

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
        
        _currentHealth -= damage;
        if(_currentHealth < 0){ Die(); }

    }

    public virtual void Die()
    {
        


    }

    private void OnTriggerEnter(Collider other) {
        
        if(other.tag == "bullet"){

            GetHit(0);

        }

    }

}
