using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class TitleUI : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnStart()
    {
        SceneManager.LoadScene("01MainGame");
    }
}
