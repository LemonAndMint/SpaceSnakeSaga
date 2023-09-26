using UnityEngine;
using Module;

public class BlankModule : MonoBehaviour, IModule
{
    [SerializeField] private float _actionCooldown;

    public float ActionCooldown{

        get{

            return _actionCooldown;

        }

    }

    /// <summary>
    /// Modül aksiyonları uygulanır. Metodu cooldown değerinden bağımsızdır.
    /// </summary>
    public virtual void Action(){ } //Bos modul icin aksiyon yok

}

public class BlankPassiveModule : BlankModule

{

    //Pasif özellikler burada olacak.

}
