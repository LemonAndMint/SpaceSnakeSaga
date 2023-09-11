using System.Collections;
using System.Collections.Generic;
using ModuleManager;
using UnityEngine;

//https://www.youtube.com/watch?v=sPlcecIh3ik&ab_channel=RandomArtAttack
//https://www.youtube.com/watch?v=A-SZDQIDXXI&ab_channel=RandomArtAttack

public class SnakeMovement : MonoBehaviour
{
    
    //Fixed Update saniyede calisma miktari
    const int FIXED_FPS = 50;

    [SerializeField] private float _speed = 180f;
    [SerializeField] private float _turnspeed = 18f;

    void FixedUpdate()
    {
        
        Movement();

    }

    void Movement()
    {

        ModuleContainer.GetModule(0).GetComponent<Rigidbody2D>().velocity = ModuleContainer.GetModule(0).transform.right * _speed * Time.deltaTime;
        if(Input.GetAxis("Horizontal") != 0)
        {

            ModuleContainer.GetModule(0).transform.Rotate(new Vector3(0, 0, -_turnspeed * Time.deltaTime * Input.GetAxis("Horizontal")));

        }

        if(ModuleContainer.Count> 1)
        {

            for(int i = 1; i < ModuleContainer.Count; i++)
            {

                MarkerStorage markM = ModuleContainer.GetModule(i - 1).GetComponent<MarkerStorage>();
                ModuleContainer.GetModule(i).transform.position = markM.markerList[0].position;
                ModuleContainer.GetModule(i).transform.rotation = markM.markerList[0].rotation;
                markM.markerList.RemoveAt(0);

            }

            //En sondaki modulun marker sayisini modüllerin en başına gelen marker sayisinda tutmak icin yazildi. Eger bu kisim yazilmasa marker sayisi kontrol edilemez rakamlara ulasiyor.

            if( ModuleContainer.GetModule(ModuleContainer.Count - 1).GetComponent<MarkerStorage>().markerList.Count > 
                ModuleContainer.GetModule(0).GetComponent<MarkerStorage>().markerList.Count + 1){

                ModuleContainer.GetModule(ModuleContainer.Count - 1).GetComponent<MarkerStorage>().markerList.RemoveAt(0);

            }

        }

    }
}
