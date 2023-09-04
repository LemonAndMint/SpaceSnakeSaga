using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class MyMarkerManager : MonoBehaviour
{
    public class Marker
    {

        public Vector3 position;
        public Quaternion rotation;

        public Marker(Vector3 pos, Quaternion rot)
        {

            position = pos;
            rotation = rot;

        }


    }

    public Marker lastMarker;

    /*public float timer;
    private float _timer;

    void FixedUpdate()
    {

        while(_timer <= timer){

            _timer -= Time.deltaTime;

        }
        UpdateMarker();

    }*/

    public void UpdateMarker()
    {

        lastMarker = new Marker(transform.position, transform.rotation);

    }

    public void ClearMarker()
    {

        lastMarker = null; //???
        //lastMarker = new Marker(transform.position, transform.rotation);

    }

}
