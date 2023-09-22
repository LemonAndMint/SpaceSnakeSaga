using ModuleManager;
using UnityEngine;

//https://www.youtube.com/watch?v=sPlcecIh3ik&ab_channel=RandomArtAttack
//https://www.youtube.com/watch?v=A-SZDQIDXXI&ab_channel=RandomArtAttack

[RequireComponent(typeof(ModuleContainer))]
public class SnakeMovement : MonoBehaviour
{

    public ModuleContainer moduleContainer;
    public DynamicJoystick joystick;
    
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
        
        _move();

    }

    void _move()
    {
        if(moduleContainer.Count > 0)
        {

            moduleContainer.Get(0).GetComponent<Rigidbody2D>().velocity = moduleContainer.Get(0).transform.right * _speed * Time.deltaTime;
            if(joystick.Horizontal != 0)
            {

                moduleContainer.Get(0).transform.Rotate(new Vector3(0, 0, -_turnspeed * Time.deltaTime * joystick.Horizontal));

            }

            if(moduleContainer.Count > 1)
            {

                for(int i = 1; i < moduleContainer.Count; i++)
                {

                    MarkerStorage markM = moduleContainer.Get(i - 1).GetComponent<MarkerStorage>();
                    moduleContainer.Get(i).transform.position = markM.markerList[0].position;
                    moduleContainer.Get(i).transform.rotation = markM.markerList[0].rotation;
                    markM.markerList.RemoveAt(0);

                }

                //En sondaki modulun marker sayisini modüllerin en başına gelen marker sayisinda tutmak icin yazildi. Eger bu kisim yazilmasa marker sayisi kontrol edilemez rakamlara ulasiyor.

                if( moduleContainer.Get(moduleContainer.Count - 1).GetComponent<MarkerStorage>().markerList.Count > 
                    moduleContainer.Get(0).GetComponent<MarkerStorage>().markerList.Count + 1){

                    moduleContainer.Get(moduleContainer.Count - 1).GetComponent<MarkerStorage>().markerList.RemoveAt(0);

                }

            }

        }

    }
}
