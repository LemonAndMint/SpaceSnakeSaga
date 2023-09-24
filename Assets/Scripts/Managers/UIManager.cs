using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameManager gm;

    public GameObject menuCanvas;
    public GameObject endMenuCanvas;
    public GameObject inGameCanvas;
    public GameObject pauseCanvas;
    public TMP_Text timer;
    public TMP_Text score;
    public GameObject joystick;
    public TMP_Text timerMenu;
    public TMP_Text scoreMenu;
    
    public void CloseMainMenu(){

        menuCanvas.SetActive(false);

    }

    public void OpenMainMenu(){

        List<int> highscores = gm.GetHighscore();

        scoreMenu.text = highscores[1].ToString();
        timerMenu.text = _floatToStringTime(highscores[2]);

        menuCanvas.SetActive(true);
        
    }

    public void OpenInGameCanvas(){

        inGameCanvas.SetActive(true);

    }

    public void CloseInGameCanvas(){

        inGameCanvas.SetActive(false);
    
    }

    public void OpenPauseMenu(){

        gm.PauseGame();
        pauseCanvas.SetActive(true);

    }

    public void ClosePaseMenu(){

        gm.ResumeGame();
        pauseCanvas.SetActive(false);

    }

    public void OpenEndMenu(){

        endMenuCanvas.SetActive(true);

    }

    public void CloseEndMenu(){

        endMenuCanvas.SetActive(false);

    }

    private void Update() {

        float remainingTime = gm.GetGameTime();
        string niceTime = _floatToStringTime(remainingTime);

        timer.text = niceTime;
        score.text = string.Format("{0}/{1}", ScoreManager.Instance.GetScore(), gm.TargetScore);

    }

    //https://discussions.unity.com/t/making-a-timer-00-00-minutes-and-seconds/14318
    private string _floatToStringTime(float timeSecond){

        int minutes = Mathf.FloorToInt(timeSecond / 60F);
        int seconds = Mathf.FloorToInt(timeSecond - minutes * 60);

        return string.Format("{0:0}:{1:00}", minutes, seconds);

    }

}
