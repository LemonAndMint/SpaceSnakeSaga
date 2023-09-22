using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameManager gm;

    public GameObject menuCanvas;
    public GameObject endMenuCanvas;
    public GameObject inGameCanvas;
    public TMP_Text timer;
    public TMP_Text score;
    public GameObject joystick;
    
    public void CloseMainMenu(){

        menuCanvas.SetActive(false);

    }

    public void OpenMainMenu(){

        menuCanvas.SetActive(true);

    }

    public void OpenInGameCanvas(){

        inGameCanvas.SetActive(true);

    }

    public void CloseInGameCanvas(){

        inGameCanvas.SetActive(false);

    }

    public void OpenEndMenu(){

        endMenuCanvas.SetActive(true);

    }

    public void CloseEndMenu(){

        endMenuCanvas.SetActive(false);

    }

    private void Update() {

        float remainingTime = gm.GetGameTime();

        int minutes = Mathf.FloorToInt(remainingTime / 60F);
        int seconds = Mathf.FloorToInt(remainingTime - minutes * 60);

        string niceTime = string.Format("{0:0}:{1:00}", minutes, seconds);

        timer.text = niceTime;

        score.text = string.Format("{0}/{1}", ScoreManager.Instance.GetScore(), gm.TargetScore);

    }

}
