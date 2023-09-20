using System.Collections;
using ModuleManager;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public CameraActions cameraActions;
    public GameObject snakeManagerPrefb;

    public static bool isDone = false;

    private ModuleBuilder _moduleBuilder;

    private void Awake() {
        
        StartCoroutine(_constructGame());

    }

    private IEnumerator _constructGame(){

        GameObject snakeManagerGO = Instantiate(snakeManagerPrefb, Vector3.zero, Quaternion.identity);

        if(snakeManagerGO.GetComponent<ModuleBuilder>())
            snakeManagerGO.GetComponent<ModuleBuilder>().cameraActions = cameraActions;

        _moduleBuilder = snakeManagerGO.GetComponent<ModuleBuilder>();
        _moduleBuilder.onGameOver = (x) => GameOver();

        //Oyuncunun kamera odak noktası.

        yield return new WaitUntil(() => snakeManagerGO.GetComponent<ModuleContainer>().Count > 0);
        yield return new WaitUntil(() => EntityBuilder.Instance != null);
        
        EntityBuilder.Instance.AnchorPoint = snakeManagerGO.GetComponent<ModuleContainer>().Get(0).transform;
        EntityBuilder.Instance.StartEntityBuilder();

    }

    public void GameOver(){

        Debug.Log("Game Over");

    }

}
