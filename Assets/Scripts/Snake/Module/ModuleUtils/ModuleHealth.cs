using System.Collections.Generic;
using EntityEnum;
using UnityEngine;
using UnityEngine.Events;

public class ModuleHealth : MonoBehaviour
{
    public List<SpriteRenderer> GFXList;

    /// <summary>
    /// Parametre olarak yok edilen modülün GameObjecti gönderilir.
    /// </summary>
    public UnityEvent<GameObject> onDie = new UnityEvent<GameObject>();
    public UnityEvent<GameObject> onGetHit = new UnityEvent<GameObject>();
    [SerializeField] private Entity enemy; 
    [SerializeField] private int _health;
    private int _currentHealth;
    private float _changeColorAmount;
    public GameObject particleGOPrefb;

    private Vector3 _diePosition;

    public bool isCollisionProof;

    private void Start() {

        Restore();
        _changeColorAmount = 1 / (float)_health;

        onDie.AddListener((GameObject x) => _startParticle() );
        
    }

    private void _startParticle(){

        if(particleGOPrefb != null){

            GameObject particleGO = Instantiate(particleGOPrefb, _diePosition, Quaternion.identity, null);
            Destroy(particleGO, particleGO.GetComponent<ParticleSystem>().main.startLifetime.constantMax + 2f);
        
        }
    }

    private void Update() {
        
        _isDead();

    }

    private void _isDead(){

        if(_currentHealth <= 0){ Die(); }

    }

    /// <summary>
    /// Modüllerin hasar almasını sağlar.
    /// </summary>
    /// <param name="damage">Verilecek hasarın değeri.</param>
    public void GetHit(int damage)
    {

        _currentHealth -= damage;

        foreach (SpriteRenderer GFX in GFXList)
        {
            GFX.color = Color.Lerp(GFX.color, Color.red, _changeColorAmount);
        }

        onGetHit?.Invoke(this.transform.root.gameObject);

    }

    public void Die()
    {
        
        _diePosition = transform.position;
        onDie?.Invoke(this.transform.root.gameObject);

    }

    public void Restore(){

        _currentHealth = _health;
        foreach (SpriteRenderer GFX in GFXList)
        {
            GFX.color = Color.white;
        }

    }

    public void OnTriggerEnter2D(Collider2D other) {

        if((other.tag.Contains(EntityUtil.EnumToString(enemy)) || other.tag.Contains("General")) && other.GetComponent<Bullet>()){

            GetHit(other.GetComponent<Bullet>().GetDamage());
            other.GetComponent<Bullet>().OnImpact();

        }

        //Eğer çarptığı obje bir modül/ düşman/ engel ise direkt bulunduğumuz modülü/ düşmanı yoket
        if(!isCollisionProof && other.GetComponentInChildren<ModuleHealth>())
            Die();

    }
}
