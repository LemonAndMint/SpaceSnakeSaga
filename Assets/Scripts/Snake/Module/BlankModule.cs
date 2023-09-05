using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Module;
using Unity.VisualScripting;

public class BlankModule : MonoBehaviour, IModule
{
    private int health;
    private int currentHealth;
    private float actionCooldown;
    //protected bool isReady = true;

    public virtual void Action(){ } //Bos modul icin aksiyon yok

    public virtual void GetHit(int damage)
    {
        
        currentHealth -= damage;
        if(currentHealth < 0){ Die(); }

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
