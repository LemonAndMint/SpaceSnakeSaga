using UnityEngine;
using UnityEngine.Events;

public abstract class Bullet : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Vector3 targetLastPosition; 
    public int damage; 

    public GameObject particleGOPrefb;

    public UnityEvent<GameObject> onHit = new UnityEvent<GameObject>();

    public int GetDamage(){ return damage; }

    public void OnImpact(){ onHit?.Invoke(this.gameObject); }

}
