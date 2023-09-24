using System.Collections;
using System.Collections.Generic;
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
    private bool _inGame = false;
    private bool _isLevelCleared = false;


    private int _levelCount;
    private float _gameTime = 0f;

    [SerializeField] private float _targetScore; 
    public float TargetScore { get { return _targetScore; } }
    [SerializeField] private float _increasedTargetScore; 
    [SerializeField] private float _firstTargetGameTime; 
    private float _targetGameTime; 
    [SerializeField] private float _decreasedTargetGameTime; 
    [SerializeField] private float _waitTime; 

    private void Start() {
        
        uiManager.OpenMainMenu();
        uiManager.CloseEndMenu();
        uiManager.CloseInGameCanvas();
        uiManager.ClosePaseMenu();

    }

    public void StartNewGame(){

        StartCoroutine(_constructGame());

    }

    public void NextLevel(){

        EntityBuilder.Instance.AddEntity(5 , 1, 3);

        _targetGameTime -= _decreasedTargetGameTime;
        _gameTime = _targetGameTime;

        _targetScore += _increasedTargetScore;

        _isLevelCleared = false;

    }

    private void Update() {

        if(_inGame){

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

    public void PauseGame(){

        Time.timeScale = 0;

    }

    public void ResumeGame(){

        Time.timeScale = 1;

    }

    public void ReturnMainMenu(){

        _resetEntities();
        _resetGame();

    }

    private void _resetEntities(){

        _snakeGO.GetComponent<ModuleBuilder>().DestroyAllModules();
        Destroy(_snakeGO);
        EntityBuilder.Instance.DestroyAllObjects();

    }

    private IEnumerator _constructGame(){

        _gameTime = _firstTargetGameTime;
        _targetGameTime = _firstTargetGameTime;

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

        EntityBuilder.Instance.StartEntityBuilder();
       
        uiManager.OpenInGameCanvas();

        _inGame = true;


    }

    private IEnumerator _nextLevel(){

        yield return new WaitForSeconds(_waitTime);
        NextLevel();

    }
    public void Win(){

        _levelCount++;

        _setHighscore();

        StartCoroutine(_nextLevel());

    }

    /// <summary>
    /// List içerisinde sırayla; level, score, time highscorelar bulunur.
    /// </summary>
    /// <returns></returns>
    public List<int> GetHighscore(){

        return new List<int> {  PlayerPrefs.GetInt("level"), PlayerPrefs.GetInt("score"), PlayerPrefs.GetInt("time") };

    }

    
    public void GameOver(){

        _setHighscore();

        _resetGame();

        uiManager.OpenEndMenu();
        uiManager.CloseInGameCanvas();
       _resetEntities();


    }

    private void _resetGame(){

        _levelCount = 0;
        ScoreManager.Instance.ResetScore();
        _gameTime = _firstTargetGameTime;

       _inGame = false;

    }

    private void _setHighscore(){

        if(PlayerPrefs.GetInt("level") <= _levelCount ){

            PlayerPrefs.SetInt("level", _levelCount);
            PlayerPrefs.SetInt("score", ScoreManager.Instance.GetScore());
            PlayerPrefs.SetInt("time", (int)_gameTime);

        }

    }

}
