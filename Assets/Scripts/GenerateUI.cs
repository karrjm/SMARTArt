using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GenerateUI : MonoBehaviour
{
    public GameObject infoUI; //the gameobject variable holding the constant UI
    public GameObject appManager; //the gameobject variable holding the app manager
    public int NumOfTxt; //number of text scripts on the object, max of 2
    public int NumOfImg; //number of image scripts on the object
    public int NumOfAud; //number of audio scripts on the object
    public int NumOfVid; //number of video scripts on the object

    GameObject myGO; //the gameObject that crteates the temporary canvas
    Canvas myCanvas; //the temporary canvas(UI) variable

    public Sprite whiteBox;
    public Sprite tl;
    public Sprite br;
    public Sprite tr;
    public Sprite bl;

    private CanvasScaler can;
    public float resoX;
    public float resoY;


    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown() //occurs when the user clicks on the attached game object
    {
        //if the infoUI is not currently active
        if (!infoUI.activeSelf)
        {
            infoUI.SetActive(true); //sets the infoUI to active


            myGO = new GameObject(); //sets the myGO object as a new game object
            myGO.name = "TestCanvas"; //names the object "TestCanvas"
            myGO.AddComponent<Canvas>(); //adds a canvas to the object
            myGO.AddComponent<CanvasScaler>();
            myGO.GetComponent<CanvasScaler>().uiScaleMode = infoUI.GetComponent<CanvasScaler>().uiScaleMode;

            resoX = (float)Screen.currentResolution.width;
            resoY = (float)Screen.currentResolution.height;

            myGO.GetComponent<CanvasScaler>().referenceResolution = new Vector2(resoX, resoY);



            myCanvas = myGO.GetComponent<Canvas>(); //sets myCanvas as the canvas attached to the myGO ("TestCanvas") game object
            myCanvas.renderMode = RenderMode.ScreenSpaceOverlay; //something for rendering purposes
            myGO.AddComponent<CanvasScaler>(); //adds a canvas scaler to the myGO object
            myGO.AddComponent<GraphicRaycaster>(); //adds a graphic raycaster to the myGO object

            appManager.GetComponent<GameManagerScript>().tempUI = myGO; //has the app manager set its tempUI value within its game manager script to myGO

            //if the NumOfTxt int is greater than 0
            if (NumOfTxt > 0)
            {
                CreateBox();
                CreateTl();
                CreateBr();
                CreateText(); //run the CreateText function
                
                //if the NumOfText variable is greater than 1
                if (NumOfTxt > 1)
                {
                    CreateBox2();
                    CreateTl2();
                    CreateBr2();
                    CreateText2(); //run the CreateText2 function
                }
            }

            //if the NumOfImg int is greater than 0
            if (NumOfImg > 0)
            {
                CreateImage();
            }

            //if the NumOfAud int is greater than 0
            if (NumOfAud > 0)
            {

            }

            //if the NumOfVid int is greater than 0
            if (NumOfVid > 0)
            {

            }
        }
    }

    GameObject CreateText() //declaration of the CreateText function
    { 

        GameObject UItextGO = new GameObject("Text"); //create UItextGO game object and name it "Text" in the heirarchy. this will be later returned as the text object
        UItextGO.transform.SetParent(myGO.GetComponent<Canvas>().transform); //set the parent of UItextGO ("Text2") as the canvas attached to the myGO ("TestCanvas") object

        RectTransform trans = UItextGO.AddComponent<RectTransform>(); //Create a rectTransform component for UItextGO, called trans
        trans.anchoredPosition = new Vector3(gameObject.GetComponent<TextData>().x, gameObject.GetComponent<TextData>().y, -0.1f); //set the anchored position of trans according to the x and y pos values in the TextData script

        Text text = UItextGO.AddComponent<Text>(); //create a text component within UItextGO, naming it text
        text.text = gameObject.GetComponent<TextData>().Text; //set the text value in text as the Text string in the TextData Script
        text.font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font; //set the font for text as Arial
        text.fontSize = gameObject.GetComponent<TextData>().fontSize; //set the font size of text as 24
        text.color = new Color(0, 0, 0); //set the color of text as black
        text.rectTransform.sizeDelta = new Vector2(gameObject.GetComponent<TextData>().width, gameObject.GetComponent<TextData>().height); //set the text boundaries to the width and height variables in the TextData script

        return UItextGO; //return UItextGO
    }

    GameObject CreateBox()
    {
        GameObject UIboxGO = new GameObject("Box");
        UIboxGO.transform.SetParent(myGO.GetComponent<Canvas>().transform);
        

        RectTransform boxtrans = UIboxGO.AddComponent<RectTransform>(); //Create a rectTransform component for UItextGO, called trans
        boxtrans.anchoredPosition = new Vector2(gameObject.GetComponent<TextData>().boxPosX, gameObject.GetComponent<TextData>().boxPosY);

        Image box = UIboxGO.AddComponent<Image>(); //create a text component within UItextGO, naming it text
        box.sprite = whiteBox;
        boxtrans.sizeDelta = new Vector2(gameObject.GetComponent<TextData>().boxWidth, gameObject.GetComponent<TextData>().boxHeight);


        return UIboxGO;
    }

    GameObject CreateTl()
    {
     
        GameObject UItlGO = new GameObject("tl");
        UItlGO.transform.SetParent(myGO.GetComponent<Canvas>().transform);


        RectTransform tltrans = UItlGO.AddComponent<RectTransform>(); //Create a rectTransform component for UItextGO, called trans
        tltrans.anchoredPosition = new Vector2(gameObject.GetComponent<TextData>().tlx, gameObject.GetComponent<TextData>().tly);

        Image tlc = UItlGO.AddComponent<Image>(); //create a text component within UItextGO, naming it text
        tlc.sprite = tl;
        tltrans.localScale = new Vector2(3f, 3f);


        return UItlGO;
    }

    GameObject CreateBr()
    {

        GameObject UIbrGO = new GameObject("br");
        UIbrGO.transform.SetParent(myGO.GetComponent<Canvas>().transform);


        RectTransform brtrans = UIbrGO.AddComponent<RectTransform>(); //Create a rectTransform component for UItextGO, called trans
        brtrans.anchoredPosition = new Vector2(gameObject.GetComponent<TextData>().brx, gameObject.GetComponent<TextData>().bry);

        Image brc = UIbrGO.AddComponent<Image>(); //create a text component within UItextGO, naming it text
        brc.sprite = br;
        brtrans.localScale = new Vector2(3f, 3f);


        return UIbrGO;
    }




    GameObject CreateText2() //CreateText2 works the exact same as CreateText, but instead utilizes the text, x, y, width, and height values from the TextData script meant for the second text box instead of the first
    {
        GameObject UItextGO = new GameObject("Text2");
        UItextGO.transform.SetParent(myGO.GetComponent<Canvas>().transform);

        RectTransform trans = UItextGO.AddComponent<RectTransform>();
        trans.anchoredPosition = new Vector3(gameObject.GetComponent<TextData>().x2, gameObject.GetComponent<TextData>().y2, -0.1f);

        Text text = UItextGO.AddComponent<Text>();
        text.text = gameObject.GetComponent<TextData>().Text2;
        text.font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
        text.fontSize = gameObject.GetComponent<TextData>().fontSize2;
        text.color = new Color(0, 0, 0);
        text.rectTransform.sizeDelta = new Vector2(gameObject.GetComponent<TextData>().width2, gameObject.GetComponent<TextData>().height2);

        return UItextGO;
    }

    GameObject CreateBox2()
    {
        GameObject UIboxGO = new GameObject("Box2");
        UIboxGO.transform.SetParent(myGO.GetComponent<Canvas>().transform);


        RectTransform boxtrans = UIboxGO.AddComponent<RectTransform>(); //Create a rectTransform component for UItextGO, called trans
        boxtrans.anchoredPosition = new Vector2(gameObject.GetComponent<TextData>().boxPosX2, gameObject.GetComponent<TextData>().boxPosY2);

        Image box = UIboxGO.AddComponent<Image>(); //create a text component within UItextGO, naming it text
        box.sprite = whiteBox;
        boxtrans.sizeDelta = new Vector2(gameObject.GetComponent<TextData>().boxWidth2, gameObject.GetComponent<TextData>().boxHeight2);


        return UIboxGO;
    }

    GameObject CreateTl2()
    {

        GameObject UItlGO = new GameObject("tl2");
        UItlGO.transform.SetParent(myGO.GetComponent<Canvas>().transform);


        RectTransform tltrans = UItlGO.AddComponent<RectTransform>(); //Create a rectTransform component for UItextGO, called trans
        tltrans.anchoredPosition = new Vector2(gameObject.GetComponent<TextData>().tlx2, gameObject.GetComponent<TextData>().tly2);

        Image tlc = UItlGO.AddComponent<Image>(); //create a text component within UItextGO, naming it text
        tlc.sprite = tl;
        tltrans.localScale = new Vector2(3f, 3f);


        return UItlGO;
    }

    GameObject CreateBr2()
    {

        GameObject UIbrGO = new GameObject("br2");
        UIbrGO.transform.SetParent(myGO.GetComponent<Canvas>().transform);


        RectTransform brtrans = UIbrGO.AddComponent<RectTransform>(); //Create a rectTransform component for UItextGO, called trans
        brtrans.anchoredPosition = new Vector2(gameObject.GetComponent<TextData>().brx2, gameObject.GetComponent<TextData>().bry2);

        Image brc = UIbrGO.AddComponent<Image>(); //create a text component within UItextGO, naming it text
        brc.sprite = br;
        brtrans.localScale = new Vector2(3f, 3f);


        return UIbrGO;
    }


    GameObject CreateImage()
    {
        GameObject UIimgGO = new GameObject("Img");
        UIimgGO.transform.SetParent(myGO.GetComponent<Canvas>().transform);


        RectTransform imgtrans = UIimgGO.AddComponent<RectTransform>(); //Create a rectTransform component for UItextGO, called trans
        imgtrans.anchoredPosition = new Vector2(gameObject.GetComponent<ImageData>().xPos, gameObject.GetComponent<ImageData>().yPos);

        Image img = UIimgGO.AddComponent<Image>(); //create a text component within UItextGO, naming it text
        img.sprite = gameObject.GetComponent<ImageData>().image;
        imgtrans.sizeDelta = new Vector2(gameObject.GetComponent<ImageData>().width, gameObject.GetComponent<ImageData>().height);


        return UIimgGO;
    }


}
