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

    public Slider CreateHPSlider()
    {
        GameObject hpobject = GameObject.Instantiate(HPGuagePrefabs);
        Slider slider = hpobject.GetComponent<Slider>();
        return slider;
    }
}
