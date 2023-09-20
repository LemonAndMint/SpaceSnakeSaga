using System.Collections;
using ModuleManager;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public CameraActions cameraActions;
    public GameObject snakeManagerPrefb;

    public static bool isDone = false;

    private ModuleBuilder _moduleBuilder;
    private GameObject _snakeGO = null;

    public void StartNewGame(){

        StartCoroutine(_constructGame());

    }

    public void EndGame(){



    }

    private IEnumerator _constructGame(){

        _snakeGO = Instantiate(snakeManagerPrefb, Vector3.zero, Quaternion.identity);

        if(_snakeGO.GetComponent<ModuleBuilder>())
            _snakeGO.GetComponent<ModuleBuilder>().cameraActions = cameraActions;

        _moduleBuilder = _snakeGO.GetComponent<ModuleBuilder>();
        _moduleBuilder.onGameOver = (x) => GameOver();

        //Oyuncunun kamera odak noktası.

        yield return new WaitUntil(() => _snakeGO.GetComponent<ModuleContainer>().Count > 0);
        yield return new WaitUntil(() => EntityBuilder.Instance != null);
        
        EntityBuilder.Instance.AnchorPoint = _snakeGO.GetComponent<ModuleContainer>().Get(0).transform;
        EntityBuilder.Instance.StartEntityBuilder();

    }

    public void GameOver(){

        Debug.Log("Game Over");

    }

}
