using System.Collections;
using System.Collections.Generic;
using ModuleManager;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public ModuleBuilder moduleBuilder;

    public GameObject enemyGOPrefb;

    public List<GameObject> energyGOPrefbList;

    private void Awake() {
        
        moduleBuilder.onGameOver = (x) => GameOver();

    }

    public void GameOver(){

        Debug.Log("Game Over");

    }

    private void _setUpEnv(){




    }

    //private void 
}
