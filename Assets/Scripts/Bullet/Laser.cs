using UnityEngine;
using DG.Tweening;
using System.Collections;

public class Laser : Bullet
{
    public GameObject laserBodyGO;
    public Quaternion weaponRotation;
    [SerializeField] private float _extendDuration;

    void Awake()
    {
        onHit.RemoveListener(Destroy);
    }

    private void Start() {
        
        transform.rotation = weaponRotation;
        StartCoroutine(_move());

    }

    private void _extendBody(){

        laserBodyGO.transform.DOScaleY(10f, moveSpeed);

    }

    private void _retractBody(){

        laserBodyGO.transform.DOScaleY(0f, moveSpeed).OnComplete( () => Destroy(this.gameObject));

    }

    private IEnumerator _move(){

        _extendBody();
        yield return new WaitForSeconds(_extendDuration);
        _retractBody();

    }
    
}
