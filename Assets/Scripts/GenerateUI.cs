using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GenerateUI : MonoBehaviour
{
    public GameObject infoUI;
    public GameObject appManager;
    public int NumOfTxt; //number of text scripts on the object
    public int NumOfImg; //number of image scripts on the object
    public int NumOfAud; //number of audio scripts on the object
    public int NumOfVid; //number of video scripts on the object

    GameObject myGO;
    Canvas myCanvas;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        if (!infoUI.activeSelf)
        {
            infoUI.SetActive(true);
        }

        myGO = new GameObject();
        myGO.name = "TestCanvas";
        myGO.AddComponent<Canvas>();

        myCanvas = myGO.GetComponent<Canvas>();
        myCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
        myGO.AddComponent<CanvasScaler>();
        myGO.AddComponent<GraphicRaycaster>();

        appManager.GetComponent<GameManagerScript>().tempUI = myGO;


        if (NumOfTxt > 0)
        {
            CreateText();

        }

        if (NumOfImg > 0)
        {

        }

        if (NumOfAud > 0)
        {

        }

        if (NumOfVid > 0)
        {

        }
    }

    GameObject CreateText()
    {
        GameObject UItextGO = new GameObject("Text2");
        UItextGO.transform.SetParent(myGO.GetComponent<Canvas>().transform);

        RectTransform trans = UItextGO.AddComponent<RectTransform>();
        trans.anchoredPosition = new Vector2(gameObject.GetComponent<TextData>().x, gameObject.GetComponent<TextData>().y);

        Text text = UItextGO.AddComponent<Text>();
        text.text = gameObject.GetComponent<TextData>().Text;
        text.font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
        text.fontSize = 24;
        text.color = new Color(0, 0, 0);

        return UItextGO;
    }
}
