using UnityEngine;
using DG.Tweening;
using System.Collections;

public class Laser : Bullet
{
    public Quaternion weaponRotation;
    public float shootRange;
    [SerializeField] private float _extendDuration;

    private void Start() {
        
        transform.rotation = weaponRotation;
        StartCoroutine(_move());

    }

    private void _extendBody(){

        transform.DOScaleY(shootRange, moveSpeed);

    }

    private void _retractBody(){

        transform.DOScaleY(0f, moveSpeed);

    }

    private IEnumerator _move(){

        onHit.RemoveListener(Destroy);
        _extendBody();
        yield return new WaitForSeconds(_extendDuration);
        _retractBody();

    }
    
}
