using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainGameUI : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


    //play UI
    public GameObject HPGuagePrefabs;
    public GameObject AttackCoolGuagePrefabs;

    public Slider CreateHPSlider()
    {
        return CreateSlider(HPGuagePrefabs);
    }

    public Slider CreateAttackCoolSlider()
    {
        return CreateSlider(AttackCoolGuagePrefabs);
    }

    Slider CreateSlider(GameObject prefabs)
    {
        GameObject gameObject = GameObject.Instantiate(prefabs);
        Slider slider = gameObject.GetComponent<Slider>();
        return slider;
    }




    //OnClick Action
    
    public void OnAttack()
    {
        Character target = GameManger.Instance.TargetCharacter;

        ObjectMessageParam messageParam = new ObjectMessageParam();
        messageParam.sender = null;
        messageParam.receiver = target;
        messageParam.attackpoint = 100;
        messageParam.message = "ATTACK";

        messageSystem.Instance.Send(messageParam);


    }
}
