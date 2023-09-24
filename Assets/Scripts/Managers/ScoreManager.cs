using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    private int _score;

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

}
