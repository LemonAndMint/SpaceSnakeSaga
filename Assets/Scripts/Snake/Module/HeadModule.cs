using ModuleManager;
using UnityEngine;

public class HeadModule : BlankPassiveModule
{
    public ModuleBuilder moduleBuilder;

    private string _energyType = null;

    public void OnTriggerEnter2D(Collider2D other) {
        
        if(other.tag.Contains("Energy")){

            _energyType = other.tag;
            EntityBuilder.Instance.ChangePosition(other.gameObject);
            Action();

        }

    }

    public override void Action(){

        ScoreManager.Instance.IncreaseScore();
        moduleBuilder.AddModulePart(_energyType);

    }

        
}
