using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioBar : MonoBehaviour
{
    private RectTransform currentPos;
    public float speed;

    public GameObject pos1;
    public GameObject pos2;
    // Start is called before the first frame update
    void Start()
    {
        currentPos = gameObject.GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (currentPos.anchoredPosition.x >= pos1.GetComponent<RectTransform>().anchoredPosition.x &&
            currentPos.anchoredPosition.x < pos2.GetComponent<RectTransform>().anchoredPosition.x)
        {
            currentPos.anchoredPosition += new Vector2(speed * Time.deltaTime, 0f);
        }
        else if (currentPos.anchoredPosition.x >= pos2.GetComponent<RectTransform>().anchoredPosition.x)
        {
            currentPos.anchoredPosition = pos1.GetComponent<RectTransform>().anchoredPosition - new Vector2(2f, 0f);
        }
    }

    public void StartMoving()
    {
        currentPos.anchoredPosition = pos1.GetComponent<RectTransform>().anchoredPosition;
    }
}
