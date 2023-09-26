using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    private int _score;
    private int _points;

    public int Points{

        get{

            return _points;

        }

    }

    private static ScoreManager _scoreManager;
    public static ScoreManager Instance {

        get{

            return _scoreManager;

        }

    }

    private void Start() {
        
        if(_scoreManager != null && _scoreManager != this){

            Destroy(this);

        }

        _scoreManager = this;
        _points = PlayerPrefs.GetInt("points", 0);

    }


    /// <summary>
    /// Skoru bir arttırır.
    /// </summary>
    public void IncreaseScore(){

        _score++;

    }

    public void ResetScore(){ _score = 0; }

    public int GetScore(){

        return _score;

    }

    public void AddPoints(){

        _points += GetScore();
        PlayerPrefs.SetInt("points", _points); 

    }

    public void DecreasePoints(int amount){

        _points -= amount;
        PlayerPrefs.SetInt("points", _points); 

    }

}
