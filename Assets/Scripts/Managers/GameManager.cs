using System.Collections;
using System.Collections.Generic;
using ModuleManager;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public UIManager uiManager;
    public ScoreManager scoreManager;

    [Space(5f)]
    public BackgroundMovement bgMovement;
    public CameraActions cameraActions;

    [Space(10f)]
    public GameObject snakeManagerPrefb;
    private ModuleBuilder _moduleBuilder;
    private GameObject _snakeGO = null;
    private bool _inGame = false;
    private bool _isLevelCleared = false;

    private int _levelCount = 0;
    public float LevelCount { get { return _levelCount; } }
    private float _gameTime = 0f;
    public float GameTime { get { return _gameTime; } }

    [Space(10f)]
    [SerializeField] private float _targetScore; 
    public float TargetScore { get { return _targetScore; } }
    [SerializeField] private float _increasedTargetScore; 
    [SerializeField] private float _firstTargetGameTime; 
    [SerializeField] private float _decreasedTargetGameTime; 
    [SerializeField] private float _waitTime; 
    private float _targetGameTime; 

    private void Start() {
        
        uiManager.OpenMainMenu();
        uiManager.CloseEndMenu();
        uiManager.CloseInGameCanvas();
        uiManager.ClosePaseMenu();
        uiManager.CloseShopCanvas();

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
            if(scoreManager.GetScore() >= _targetScore && !_isLevelCleared){

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

        yield return new WaitUntil(() => _snakeGO.GetComponent<ModuleContainer>().Count > 0);
        yield return new WaitUntil(() => EntityBuilder.Instance != null);
        
        bgMovement.playerTransform = _snakeGO.GetComponent<ModuleContainer>().Get(0).transform;
        //Oyuncunun kamera odak noktası.
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

        uiManager.endgameScore.text = scoreManager.GetScore().ToString();

        _setHighscore();
        scoreManager.AddPoints();

        _resetGame();

        uiManager.OpenEndMenu();
        uiManager.CloseInGameCanvas();
       _resetEntities();

    }

    private void _resetGame(){

        _levelCount = 0;
        scoreManager.ResetScore();
        _gameTime = _firstTargetGameTime;

       _inGame = false;

    }

    private void _setHighscore(){

        if(PlayerPrefs.GetInt("level") <= _levelCount ){

            PlayerPrefs.SetInt("level", _levelCount);
            PlayerPrefs.SetInt("score", scoreManager.GetScore());
            PlayerPrefs.SetInt("time", (int)_gameTime);

        }

    }

    

}
