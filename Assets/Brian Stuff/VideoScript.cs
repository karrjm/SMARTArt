using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;

public class VideoScript : MonoBehaviour
{

    public VideoPlayer video;
    private bool playing = false;

    public GameObject button;

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

    public void PlayPause()
    {
        if (!playing)
        {
            video.Play();
            playing = true;
            button.GetComponent<Image>().sprite = pause;
        }
        else
        {
            video.Pause();
            playing = false;
            button.GetComponent<Image>().sprite = play;
        }
    }
}
