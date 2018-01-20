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
        GameObject hpobject = GameObject.Instantiate(HPGuagePrefabs);
        Slider slider = hpobject.GetComponent<Slider>();
        return slider;
    }

    public Slider CreateAttackCoolSlider()
    {
        GameObject AttackCoolTime = GameObject.Instantiate(AttackCoolGuagePrefabs);
        Slider slider = AttackCoolTime.GetComponent<Slider>();
        return slider;
    }
}
