using System.Collections;
using ModuleManager;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public CameraActions cameraActions;
    public GameObject snakeManagerPrefb;
    public UIManager uiManager;
    public BackgroundMovement bgMovement;

    private ModuleBuilder _moduleBuilder;
    private GameObject _snakeGO = null;
    private bool firstTime = true;
    private bool _isLevelCleared = false;


    private int _levelCount;
    private float _gameTime = 0f;

    [SerializeField] private float _targetScore; 
    public float TargetScore { get { return _targetScore; } }
    [SerializeField] private float _increasedTargetScore; 
    [SerializeField] private float _targetGameTime; 
    [SerializeField] private float _decreasedTargetGameTime; 
    [SerializeField] private float _waitTime; 

    private void Start() {
        
        uiManager.OpenMainMenu();
        uiManager.CloseEndMenu();
        uiManager.CloseInGameCanvas();

    }

    public void StartNewGame(){

        StartCoroutine(_constructGame());

    }

    public void NextLevel(){

        EntityBuilder.Instance.AddEntity(5 , 1, 3);
        EntityBuilder.Instance.RepositionEntities();

        _targetGameTime -= _decreasedTargetGameTime;
        _gameTime = _targetGameTime;

        _targetScore += _increasedTargetScore;

        _isLevelCleared = false;

    }

    private void Update() {

        if(!firstTime){

            _gameTime -= Time.deltaTime;   

            if(_gameTime < 0){

                GameOver();

            }
            if(ScoreManager.Instance.GetScore() >= _targetScore && !_isLevelCleared){

                Win();
                _isLevelCleared = true;

            }     

        }

    }

    public float GetGameTime(){

        return _gameTime;

    }

    private IEnumerator _constructGame(){

        _gameTime = _targetGameTime;

        _snakeGO = Instantiate(snakeManagerPrefb, Vector3.zero, Quaternion.identity);

        if(_snakeGO.GetComponent<ModuleBuilder>())
            _snakeGO.GetComponent<ModuleBuilder>().cameraActions = cameraActions;

        if(_snakeGO.GetComponent<SnakeMovement>())
            _snakeGO.GetComponent<SnakeMovement>().joystick = uiManager.joystick.GetComponent<DynamicJoystick>();

        _moduleBuilder = _snakeGO.GetComponent<ModuleBuilder>();
        _moduleBuilder.onGameOver = (x) => GameOver();

        //Oyuncunun kamera odak noktası.

        yield return new WaitUntil(() => _snakeGO.GetComponent<ModuleContainer>().Count > 0);
        yield return new WaitUntil(() => EntityBuilder.Instance != null);
        
        bgMovement.playerTransform = _snakeGO.GetComponent<ModuleContainer>().Get(0).transform;
        EntityBuilder.Instance.AnchorPoint = _snakeGO.GetComponent<ModuleContainer>().Get(0).transform;

        if(firstTime){
         
            EntityBuilder.Instance.StartEntityBuilder();
            firstTime = false;

        }
        else{

            EntityBuilder.Instance.RepositionEntities();

        }

        uiManager.OpenInGameCanvas();


    }

    private IEnumerator _nextLevel(){

        yield return new WaitForSeconds(_waitTime);
        NextLevel();

    }
    public void Win(){

        Debug.Log("Win");
        _levelCount++;
        StartCoroutine(_nextLevel());

    }
    
    public void GameOver(){

        Debug.Log("Game Over");

        PlayerPrefs.SetInt("level", _levelCount);
        PlayerPrefs.SetInt("score", ScoreManager.Instance.GetScore());
        PlayerPrefs.SetInt("time", (int)_gameTime);

        uiManager.OpenEndMenu();
        uiManager.CloseInGameCanvas();
        Destroy(_snakeGO);

    }

}
