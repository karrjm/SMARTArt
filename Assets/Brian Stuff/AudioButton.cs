using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//DO NOT KNOW IF SCRIPT WILL BE IN FINAL VERSION OF APP

public class AudioButton : MonoBehaviour
{
    public AudioSource aud;

    public bool playing = false;
    public bool paused = false;

    public Sprite play;
    public Sprite pause;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AudButtonClick()
    {
        if (!playing && !paused)
        {
            playing = true;
            aud.Play();
            gameObject.GetComponent<Image>().sprite = pause;
        }
        else if (playing)
        {
            playing = false;
            paused = true;
            aud.Pause();
            gameObject.GetComponent<Image>().sprite = play;
        }
        else if (paused)
        {
            paused = false;
            playing = true;
            aud.Play();
            gameObject.GetComponent<Image>().sprite = pause;
        }
    }
}
