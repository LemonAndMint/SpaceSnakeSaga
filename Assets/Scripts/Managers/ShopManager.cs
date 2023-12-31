using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using ModuleManager;

public class ShopManager : MonoBehaviour
{

    public GameManager gm;
    public ScoreManager scoreManager;
    [Space(10f)]

    public List<ShipSkinScriptableObject> avaliableSkins;
    public GameObject shopPanel;
    public GameObject skinGOPrefb;
    public Image currentShipSkin;

    // Start is called before the first frame update
    void Start()
    {
        
        _initSkins();
        currentShipSkin.sprite = avaliableSkins[PlayerPrefs.GetInt("currSkin", 0)].shipSkin;

    }

    private void _initSkins(){

        int index = 0;

        foreach (ShipSkinScriptableObject skin in avaliableSkins)
        {

            GameObject skinGO = Instantiate(skinGOPrefb, shopPanel.transform, false);
            skinGO.GetComponentInChildren<Image>().sprite = skin.shipSkin;

            int tempIndex = index;
            skinGO.GetComponent<Button>().onClick.AddListener(() => _buyOrSetSkin(tempIndex));

            if(skin.isBought == false){

                skinGO.GetComponentInChildren<TMP_Text>().text = skin.price.ToString();
            
            }

            else{

                skinGO.GetComponentInChildren<TMP_Text>().gameObject.SetActive(false);

            }

            index++;

        }

    }
    
    private void _buyOrSetSkin(int index){

        if (avaliableSkins[index].isBought == true)
        {

            gm.snakeManagerPrefb.GetComponent<ModuleBuilder>().headSprite = avaliableSkins[index].shipSkin;
            currentShipSkin.sprite = avaliableSkins[index].shipSkin;
            
            if(shopPanel.transform.GetChild(index).GetComponentInChildren<TMP_Text>())
                shopPanel.transform.GetChild(index).GetComponentInChildren<TMP_Text>().gameObject.SetActive(false);

            PlayerPrefs.SetInt("currSkin", index);

        }
        else
        {
            if(scoreManager.Points >= avaliableSkins[index].price)
            {

                scoreManager.DecreasePoints(avaliableSkins[index].price);
                avaliableSkins[index].isBought = true;
                _buyOrSetSkin(index);

            }
        }


    }
}
