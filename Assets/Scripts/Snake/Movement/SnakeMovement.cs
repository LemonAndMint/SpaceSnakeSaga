using System.Collections;
using System.Collections.Generic;
using ModuleManager;
using UnityEngine;

//https://www.youtube.com/watch?v=sPlcecIh3ik&ab_channel=RandomArtAttack
//https://www.youtube.com/watch?v=A-SZDQIDXXI&ab_channel=RandomArtAttack

[RequireComponent(typeof(ModuleContainer))]
public class SnakeMovement : MonoBehaviour
{

    public ModuleContainer moduleContainer;
    
    //Fixed Update saniyede calisma miktari
    const int FIXED_FPS = 50;

    [SerializeField] private float _speed = 180f;
    [SerializeField] private float _turnspeed = 18f;

    private void Start() {
        
        if(moduleContainer == null)
                moduleContainer = GetComponent<ModuleContainer>();

    }

    void FixedUpdate()
    {
        
        Movement();

    }

    void Movement()
    {

        moduleContainer.GetModule(0).GetComponent<Rigidbody2D>().velocity = moduleContainer.GetModule(0).transform.right * _speed * Time.deltaTime;
        if(Input.GetAxis("Horizontal") != 0)
        {

            moduleContainer.GetModule(0).transform.Rotate(new Vector3(0, 0, -_turnspeed * Time.deltaTime * Input.GetAxis("Horizontal")));

        }

        if(moduleContainer.Count> 1)
        {

            for(int i = 1; i < moduleContainer.Count; i++)
            {

                MarkerStorage markM = moduleContainer.GetModule(i - 1).GetComponent<MarkerStorage>();
                moduleContainer.GetModule(i).transform.position = markM.markerList[0].position;
                moduleContainer.GetModule(i).transform.rotation = markM.markerList[0].rotation;
                markM.markerList.RemoveAt(0);

            }

            //En sondaki modulun marker sayisini modüllerin en başına gelen marker sayisinda tutmak icin yazildi. Eger bu kisim yazilmasa marker sayisi kontrol edilemez rakamlara ulasiyor.

            if( moduleContainer.GetModule(moduleContainer.Count - 1).GetComponent<MarkerStorage>().markerList.Count > 
                moduleContainer.GetModule(0).GetComponent<MarkerStorage>().markerList.Count + 1){

                moduleContainer.GetModule(moduleContainer.Count - 1).GetComponent<MarkerStorage>().markerList.RemoveAt(0);

            }

        }

    }
}
