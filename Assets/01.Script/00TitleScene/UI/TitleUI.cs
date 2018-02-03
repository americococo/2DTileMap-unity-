using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleUI : MonoBehaviour
{
    public Text UpdateKey;


    // Use this for initialization
    void Start()
    {

    }

    bool blank = true;
    float deltaUpdate = 0.0f;
    float updateTime = 1.0f;

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown)
        {
            SceneManager.LoadScene("01MainGame");
            return;
        }

        deltaUpdate += Time.deltaTime;  
        if (deltaUpdate > updateTime)
        {
            ReverseText();
            deltaUpdate = 0.0f;
        }

    }
    void ReverseText()
    {
        blank = (!blank);
        UpdateKey.gameObject.SetActive(blank);
    }
}
