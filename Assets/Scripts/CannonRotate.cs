using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(WeaponModule))] //#FIXME class genelleştirmesi yap
public class CannonRotate : MonoBehaviour
{
    public WeaponModule weaponModule;

    // Update is called once per frame
    void Update()
    {
        _rotate();
    }

    void _rotate(){

        if( weaponModule.TargetedEnemy != null ){

            _rotateTo();

        }

        _resetRotation();

    }

    private void _rotateTo(){ //https://youtu.be/1Oda2M4BoNs?t=86

        Vector3 look = transform.InverseTransformPoint( weaponModule.TargetedEnemy.transform.position );

        //Cannonun sagda ya da solda olup olmadigi cannonun y pozisyonunun isaretine gore belirlendik.
        //Sag cannon asagi tarafta kaldigindan aci degerinin 90 derece ile toplanmasi gerekirken
        //Sol caoonon yukari tarafta kaldigindan aci degerinin 90 derece ile cikartilmasi gerek.
        //https://gamedev.stackexchange.com/questions/159581/why-do-i-need-to-offset-my-aim-rotation-angle-by-90-degrees#:~:text=1%20Answer&text=As%20per%20the%20unit%20circle,%2C%20and%20%2D90%20at%20right.

        float angle = Mathf.Atan2(look.y, look.x) * Mathf.Rad2Deg - 90 * Mathf.Sign(transform.localPosition.y);

        transform.Rotate(0 , 0, angle); 

    }

    private void _resetRotation(){ transform.Rotate( 0, 0, 0 ); }
}
