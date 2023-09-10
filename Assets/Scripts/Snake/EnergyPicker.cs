using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnergyPicker : MonoBehaviour
{
    
    private void OnTriggerEnter(Collider other) {
        
        if(other.tag == "Energy"){

            Destroy(other.gameObject);//#TODO enerji içerisindeki metodları kullan

        }

    }

}
