using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioBar : MonoBehaviour
{
    private RectTransform currentPos;
    public float speed;

    public GameObject pos1;
    public GameObject pos2;

    public GameObject audioButton;

    private bool alreadyStarted = false;
    // Start is called before the first frame update
    void Start()
    {
        currentPos = gameObject.GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (currentPos.anchoredPosition.x >= pos1.GetComponent<RectTransform>().anchoredPosition.x &&
            currentPos.anchoredPosition.x < pos2.GetComponent<RectTransform>().anchoredPosition.x && audioButton.GetComponent<AudioButton>().playing)
        {
            currentPos.anchoredPosition += new Vector2(speed * Time.deltaTime, 0f);
        }
        else if (currentPos.anchoredPosition.x >= pos2.GetComponent<RectTransform>().anchoredPosition.x)
        {
            currentPos.anchoredPosition = pos1.GetComponent<RectTransform>().anchoredPosition - new Vector2(2f, 0f);
            audioButton.GetComponent<AudioButton>().playing = false;
            audioButton.GetComponent<AudioButton>().paused = false;
            alreadyStarted = false;
            audioButton.GetComponent<Image>().sprite = audioButton.GetComponent<AudioButton>().play;
        }
    }

    public void StartMoving()
    {
        if (!alreadyStarted)
        {
            currentPos.anchoredPosition = pos1.GetComponent<RectTransform>().anchoredPosition;
            alreadyStarted = true;
        }
    }
}
