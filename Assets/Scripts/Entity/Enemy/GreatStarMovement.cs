using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class GreatStarMovement : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        
        _rotate();

    }

    private void _rotate(){

        transform.Rotate(Vector3.forward, 15f * Time.deltaTime);

    }

}
