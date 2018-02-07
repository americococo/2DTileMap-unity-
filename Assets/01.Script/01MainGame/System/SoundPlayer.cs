using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPlayer : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void Awake()
    {
        _audioSource = gameObject.AddComponent<AudioSource>();
    }


    static private SoundPlayer _Instance;
    public static SoundPlayer Instance
    {
        get
        {
            if (null == _Instance)
            {
                _Instance = FindObjectOfType<SoundPlayer>();
                if(null==_Instance)
                {
                    GameObject gameObject = new GameObject();
                    gameObject.name = "SoundPlayer";
                    _Instance = gameObject.AddComponent<SoundPlayer>();
                    DontDestroyOnLoad(gameObject);
                }
            }
            return _Instance;
        }
    }

    AudioSource _audioSource;

    public void PlayEffect(string soundName)
    {
        string filePath = "Sound/Effects/" + soundName;
        AudioClip clip = Resources.Load<AudioClip>(filePath);

        if(null!= clip)
        {
            _audioSource.clip = clip;
            _audioSource.Play();
        }
    }

}
