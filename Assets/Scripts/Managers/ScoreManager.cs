using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    private int _score;

    private static ScoreManager _scoreManager;
    public static ScoreManager Instance {

        get{

            if(_scoreManager == null){

                _scoreManager = Instantiate(new ScoreManager()); //#FIXME

            }
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

    public int GetScore(){

        return _score;

    }

}
