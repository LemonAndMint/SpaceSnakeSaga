using UnityEngine;
using UnityEngine.Events;

public class Bullet : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Vector3 targetLastPosition; 
    public int damage; 

    public GameObject particleGOPrefb;

    public UnityEvent<GameObject> onHit = new UnityEvent<GameObject>();

    public int GetDamage(){ return damage; }

    public void OnImpact(){ onHit?.Invoke(this.gameObject); }

    private void Start() {
        
        transform.right = targetLastPosition - transform.position;
        onHit.AddListener( (GameObject x) => _startParticle() );

    }

    private void _startParticle(){

        if(particleGOPrefb != null){

            GameObject particleGO = Instantiate(particleGOPrefb, transform.position, Quaternion.identity);
            Destroy(particleGO, particleGO.GetComponent<ParticleSystem>().main.startLifetime.constantMax + 0.5f);
        
        }
    }

    void Update()
    {
        
        _move();

    }

    private void _move(){

        if(targetLastPosition != null){
        
            transform.position = Vector3.MoveTowards(transform.position, targetLastPosition, Time.deltaTime * moveSpeed);
            if( Vector3.Distance(transform.position, targetLastPosition) < 0.05f ){

                OnImpact();

            }
        }

    }

}
