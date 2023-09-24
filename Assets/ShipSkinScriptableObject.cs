using UnityEngine;

[CreateAssetMenu(menuName = "Ship Skin", order = 2, fileName = "Ship Skin")]
public class ShipSkinScriptableObject : ScriptableObject
{
    public Sprite shipSkin;
    public int price;
    public bool isBought;
}
