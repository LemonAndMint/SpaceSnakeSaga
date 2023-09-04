using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using UnityEngine;

public class SnakeManager : MonoBehaviour
{
    [SerializeField] float distanceBetween = .2f;
    [SerializeField] float speed = 180f;
    [SerializeField] float turnSpeed = 18f;

    [SerializeField] List<GameObject> bodyParts = new List<GameObject>();
    List<GameObject> snakeBody = new List<GameObject>();

    float countUp = 0;
    private void Start()
    {

        CreateBodyParts();

    }

    private void FixedUpdate()
    {

        ManageSnakeBody();
        SnakeMovement();

    }

    void ManageSnakeBody()
    {

        if (bodyParts.Count > 0){ CreateBodyParts(); }

        for (int i = 0; i < snakeBody.Count; i++)
        {

            if (snakeBody[i] == null){ snakeBody.RemoveAt(i); i--; }

        }
        if(snakeBody.Count == 0) { Destroy(this); }

    }

    void SnakeMovement()
    {

        snakeBody[0].GetComponent<Rigidbody2D>().velocity = snakeBody[0].transform.right * speed * Time.deltaTime;
        if(Input.GetAxis("Horizontal") != 0)
        {

            snakeBody[0].transform.Rotate(new Vector3(0, 0, -turnSpeed * Time.deltaTime * Input.GetAxis("Horizontal")));

        }

        if(snakeBody.Count > 1)
        {

            for(int i = 1; i < snakeBody.Count; i++)
            {

                MarkerManager markM = snakeBody[i - 1].GetComponent<MarkerManager>();
                snakeBody[i].transform.position = markM.markerList[0].position;
                snakeBody[i].transform.rotation = markM.markerList[0].rotation;
                markM.markerList.RemoveAt(0);

            }
             
        }

    }

    void CreateBodyParts()
    {
        if(snakeBody.Count == 0){ InstantiateModule(); }

        MarkerManager markM = snakeBody[snakeBody.Count - 1].GetComponent<MarkerManager>();

        if(countUp == 0) { markM.ClearMarkerList(); }
        countUp += Time.deltaTime;

        if(countUp >= distanceBetween) {

            GameObject tempModule = InstantiateModule();
            tempModule.GetComponent<MarkerManager>().ClearMarkerList();
            countUp = 0;

        }


    }

    GameObject InstantiateModule()
    {
        GameObject temp = Instantiate(bodyParts.First(), transform.position, transform.rotation, transform);

        //if (!temp.GetComponent<MarkerManager>()) { temp.AddComponent<MarkerManager>(); }

        if (!temp.GetComponent<Rigidbody2D>())
        {

            temp.AddComponent<Rigidbody2D>();
            temp.GetComponent<Rigidbody2D>().gravityScale = 0;

        }

        snakeBody.Add(temp);
        bodyParts.RemoveAt(0);

        return temp;
    }

    public void AddBodyParts(GameObject obj)
    {

        bodyParts.Add(obj);
    
    }

}
