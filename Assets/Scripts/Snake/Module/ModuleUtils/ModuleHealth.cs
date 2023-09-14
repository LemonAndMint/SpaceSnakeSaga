using EntityEnum;
using UnityEngine;
using UnityEngine.Events;

public class ModuleHealth : MonoBehaviour
{
    /// <summary>
    /// Parametre olarak yok edilen modülün GameObjecti gönderilir.
    /// </summary>
    public UnityEvent<GameObject> onDie = new UnityEvent<GameObject>();
    [SerializeField] private Entity enemy; 
    [SerializeField] private int _health;
    private int _currentHealth;

    public bool isCollisionProof;

    private bool _isCreated = false;
    
    /// <summary>
    /// Modülün yaratıldığını belirten <paramref name = "_isCreated"/> boolunu true yapar.
    /// </summary>
    public void ModuleCreated(){

        _isCreated = true;

    }

    private void Start() {

        _currentHealth = _health;
    
    }

    private void Update() {
        
        _isDead();

    }

    private void _isDead(){

        if(_currentHealth < 0){ Die(); }

    }

    /// <summary>
    /// Modüllerin hasar almasını sağlar.
    /// </summary>
    /// <param name="damage">Verilecek hasarın değeri.</param>
    public virtual void GetHit(int damage)
    {
        if(!_isCreated){ Die(); }

        _currentHealth -= damage;
        _isDead();

    }

    public virtual void Die()
    {
        onDie?.Invoke(this.transform.root.gameObject);

    }

    public virtual void OnTriggerEnter2D(Collider2D other) {
        
        if(other.tag.Contains(EntityUtil.EnumToString(enemy)) && other.GetComponent<Bullet>()){

            GetHit(other.GetComponent<Bullet>().GetDamage());
            Destroy(other.gameObject); //#FIXME ???? dogru mu 

        }

        //Eğer çarptığı obje bir modül/ düşman/ engel ise direkt bulunduğumuz modülü/ düşmanı yoket
        if(!isCollisionProof && other.GetComponentInChildren<ModuleHealth>())
            Die();

    }
}
