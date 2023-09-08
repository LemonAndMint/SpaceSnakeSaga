using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

//https://www.youtube.com/watch?v=sPlcecIh3ik&ab_channel=RandomArtAttack
//https://www.youtube.com/watch?v=A-SZDQIDXXI&ab_channel=RandomArtAttack

public class MarkerStorage : MonoBehaviour
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

    public List<Marker> markerList = new List<Marker>();

    void FixedUpdate()
    {

        UpdateMarkerList();

    }

    public void UpdateMarkerList()
    {

        markerList.Add(new Marker(transform.position, transform.rotation));

    }

    public void ClearMarkerList()
    {

        markerList.Clear();
        markerList.Add(new Marker(transform.position, transform.rotation));

    }

}
