using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(SnakeMovement))]
public class ModuleBuilder : MonoBehaviour
{
    public SnakeMovement snakeMovement;
    
    [SerializeField] private float _distanceBetween = .2f;
    [SerializeField] private List<GameObject> _addingModuleParts = new List<GameObject>();
    
    float _countUp = 0;
    void Start()
    {
        if(snakeMovement == null){ GetComponent<SnakeMovement>(); }
        _CreateModuleParts();

    }

    private void Update()
    {

        _ManageSnakeBody();

    }

    private void _CreateModuleParts()
    {
        if(snakeMovement.SnakeBody.Count == 0){ _InstantiateModule(); }

        MarkerStorage markM = snakeMovement.SnakeBody[snakeMovement.SnakeBody.Count - 1].GetComponent<MarkerStorage>();

        if(_countUp == 0) { markM.ClearMarkerList(); }
        _countUp += Time.deltaTime;

        if(_countUp >= _distanceBetween) {

            GameObject tempModule = _InstantiateModule();
            tempModule.GetComponent<MarkerStorage>().ClearMarkerList();
            _countUp = 0;

        }


    }

    private void _ManageSnakeBody()
    {

        if (_addingModuleParts.Count > 0){ _CreateModuleParts(); }

        for (int i = 0; i < snakeMovement.SnakeBody.Count; i++)
        {

            if (snakeMovement.SnakeBody[i] == null){ snakeMovement.SnakeBody.RemoveAt(i); i--; }

        }
        if(snakeMovement.SnakeBody.Count == 0) { Destroy(this); }

    }

    private GameObject _InstantiateModule()
    {
        GameObject temp = Instantiate(_addingModuleParts.First(), transform.position, transform.rotation, transform);

        if (!temp.GetComponent<MarkerStorage>()) { temp.AddComponent<MarkerStorage>(); }

        if (!temp.GetComponent<Rigidbody2D>())
        {

            temp.AddComponent<Rigidbody2D>();
            temp.GetComponent<Rigidbody2D>().gravityScale = 0;

        }

        snakeMovement.SnakeBody.Add(temp);
        _addingModuleParts.RemoveAt(0);

        return temp;
    }

    public void AddModuleParts(GameObject obj)
    {

        _addingModuleParts.Add(obj);
    
    }

}
