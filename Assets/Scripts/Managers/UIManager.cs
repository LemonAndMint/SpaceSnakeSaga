using UnityEngine;

public class UIManager : MonoBehaviour
{

    public GameObject menuCanvas;
    public GameObject endMenuCanvas;
    
    public void CloseMainMenu(){

        menuCanvas.SetActive(false);

    }

    public void OpenMainMenu(){

        menuCanvas.SetActive(true);

    }

    public void OpenEndMenu(){

        endMenuCanvas.SetActive(true);

    }

    public void CloseEndMenu(){

        endMenuCanvas.SetActive(false);

    }

}
