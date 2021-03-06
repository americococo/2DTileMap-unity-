﻿using System.Collections;
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
    public GameObject LevelGuagePrefabs;

    public Slider CreateHPSlider()
    {
        return CreateSlider(HPGuagePrefabs);
    }

    public Slider CreateAttackCoolSlider()
    {
        return CreateSlider(AttackCoolGuagePrefabs);
    }

    public Slider CreateLevelSlider()
    {
        return CreateSlider(LevelGuagePrefabs);
    }

    Slider CreateSlider(GameObject prefabs)
    {
        GameObject gameObject = GameObject.Instantiate(prefabs);
        Slider slider = gameObject.GetComponent<Slider>();
        return slider;
    }


}
