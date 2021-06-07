using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GenerateUI : MonoBehaviour
{
    public GameObject startUI;
    public GameObject infoUI; //the gameobject variable holding the constant UI
    public GameObject appManager; //the gameobject variable holding the app manager
    public int NumOfTxt; //number of text scripts on the object, max of 2
    public int NumOfImg; //number of image scripts on the object
    public int NumOfAud; //number of audio scripts on the object
    public int NumOfVid; //number of video scripts on the object
    public int totalNum = 0;

    GameObject myGO; //the gameObject that crteates the temporary canvas
    Canvas myCanvas; //the temporary canvas(UI) variable

    public Sprite whiteBox; //stores the sprite image for the white box
    public Sprite tl; //stores the sprite image for the top corner
    public Sprite br; //stores the sprite image for the bottom corner

    public float resoX; //stores the x value of the current screen resolution
    public float resoY; //stores the y value of the current screen resolution




    

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
            gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().color = new Color(0f, 255f, 255f);
        }
    }


    private void OnMouseUp() //occurs when the user clicks on the attached game object
    {
        gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().color = new Color(255f, 255f, 255f);
        //if the infoUI is not currently active
        if (!infoUI.activeSelf)
        {
            startUI.SetActive(false);
            infoUI.SetActive(true); //sets the infoUI to active
            appManager.GetComponent<GameManagerScript>().selectedPanel = this.gameObject;


            myGO = new GameObject(); //sets the myGO object as a new game object
            myGO.name = "TestCanvas"; //names the myGO object "TestCanvas" in the heirarchy
            myGO.AddComponent<Canvas>(); //adds a canvas to the myGO object
            myGO.AddComponent<CanvasScaler>(); //adds a canvas scaler to the myGO object
            myGO.GetComponent<CanvasScaler>().uiScaleMode = infoUI.GetComponent<CanvasScaler>().uiScaleMode; //sets the scale mode of the temporary UI (TestCanvas) to the same mode as the constant UI

            resoX = (float)Screen.currentResolution.width; //sets resoX to the current x value of the screen resolution
            resoY = (float)Screen.currentResolution.height; //sets resoY to the current y value of the screen resolution

            myGO.GetComponent<CanvasScaler>().referenceResolution = new Vector2(resoX, resoY); //sets the reference resolution of myGO canvas to the same x and y values as resoX and resoY



            myCanvas = myGO.GetComponent<Canvas>(); //sets myCanvas as the canvas attached to the myGO ("TestCanvas") game object
            myCanvas.renderMode = RenderMode.ScreenSpaceOverlay; //something for rendering purposes
            myGO.AddComponent<CanvasScaler>(); //adds a canvas scaler to the myGO object
            myGO.AddComponent<GraphicRaycaster>(); //adds a graphic raycaster to the myGO object

            appManager.GetComponent<GameManagerScript>().tempUI = myGO.GetComponent<Canvas>(); //has the app manager set its tempUI value within its game manager script to myGO

            //if the NumOfTxt int is greater than 0
            if (NumOfTxt > 0)
            {
                GameObject textBox = new GameObject("TextBox"); //create textBox game object and name it "TextBox" in the heirarchy. this will be later returned as the text object
                textBox.transform.SetParent(myGO.GetComponent<Canvas>().transform); //set the parent of textBox ("TextBox") as the canvas attached to the myGO ("TestCanvas") object
                textBox.AddComponent<RectTransform>(); //add a rectangle transform to the textBox object
                textBox.AddComponent<Draggable>(); //give the Draggable property script to the textBox object, making it draggable
                textBox.AddComponent<PinchZoom>(); //give the PinchZoom property script to the textBox object, making it enlargeable or reduceable

                CreateBox(textBox); //call the CreateBox function with textBox as the input
                CreateTl(textBox); //call the CreateTl function with textBox as the input
                CreateBr(textBox); //call the CreateBr function with textBox as the input
                CreateText(textBox); //call the CreateText function with textBox as the input
                textBox.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, 0f); //set the anchored position of the textBox object to 0, 0
                
                //if the NumOfText variable is greater than 1
                if (NumOfTxt > 1)
                {
                    GameObject textBox2 = new GameObject("TextBox2"); //create textBox2 game object and name it "TextBox2" in the heirarchy. this will be later returned as the text object
                    textBox2.transform.SetParent(myGO.GetComponent<Canvas>().transform); //set the parent of textBox2 ("TextBox2") as the canvas attached to the myGO ("TestCanvas") object
                    textBox2.AddComponent<RectTransform>(); //add a rectangle transform to textBox2
                    textBox2.AddComponent<Draggable>(); //give the Draggable property script to the textBox2 object, making it draggable
                    textBox2.AddComponent<PinchZoom>(); //give the PinchZoom property script to the textBox2 object, making it enlargeable or reduceable

                    CreateBox2(textBox2); //call the CreateBox2 function with textBox2 as the input
                    CreateTl2(textBox2); //call the CreateTl2 function with textBox2 as the input
                    CreateBr2(textBox2); //call the CreateBr2 function with textBox2 as the input
                    CreateText2(textBox2); //call the CreateText2 function with textBox2 as the input
                    textBox2.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, 0f); //set the anchored position of the textBox2 object to 0 , 0 
                }
            }

            totalNum += NumOfTxt;

            //if the NumOfImg int is greater than 0
            if (NumOfImg > 0)
            {
                GameObject image = new GameObject("image"); //create image game object and name it "image" in the heirarchy. this will be later returned as the text object
                image.transform.SetParent(myGO.GetComponent<Canvas>().transform); //set the parent of image ("image") as the canvas attached to the myGO ("TestCanvas") object
                image.AddComponent<RectTransform>(); //add a rectangle transform to image
                image.AddComponent<Draggable>(); //add the dragable property script to image
                image.AddComponent<PinchZoom>(); //add the pinch zoom property script to image

                CreateImage(image); //call the CreateImage function with image as the input
                image.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, 0f); //set the anchored position for image to 0 , 0 
            }

            totalNum += NumOfImg;

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

    GameObject CreateText(GameObject parent) //declaration of the CreateText function
    { 

        GameObject UItextGO = new GameObject("Text"); //create UItextGO game object and name it "Text" in the heirarchy. this will be later returned as the text object
        UItextGO.transform.SetParent(parent.transform); //set the parent of UItextGO ("Text2") as the canvas attached to the myGO ("TestCanvas") object
        

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

    GameObject CreateBox(GameObject parent) //declaration of the CreateBox function
    {
        GameObject UIboxGO = new GameObject("Box"); //create UIboxGO object, naming it "Box" in the heirarchy
        UIboxGO.transform.SetParent(parent.transform);
        

        RectTransform boxtrans = UIboxGO.AddComponent<RectTransform>(); //Create a rectTransform component for UItextGO, called trans
        boxtrans.anchoredPosition = new Vector2(gameObject.GetComponent<TextData>().boxPosX, gameObject.GetComponent<TextData>().boxPosY);

        Image box = UIboxGO.AddComponent<Image>(); //create a text component within UItextGO, naming it text
        box.sprite = whiteBox;
        boxtrans.sizeDelta = new Vector2(gameObject.GetComponent<TextData>().boxWidth, gameObject.GetComponent<TextData>().boxHeight);


        return UIboxGO;
    }

    GameObject CreateTl(GameObject parent)
    {
     
        GameObject UItlGO = new GameObject("tl");
        UItlGO.transform.SetParent(parent.transform);


        RectTransform tltrans = UItlGO.AddComponent<RectTransform>(); //Create a rectTransform component for UItextGO, called trans
        tltrans.anchoredPosition = new Vector2(gameObject.GetComponent<TextData>().tlx, gameObject.GetComponent<TextData>().tly);

        Image tlc = UItlGO.AddComponent<Image>(); //create a text component within UItextGO, naming it text
        tlc.sprite = tl;
        tltrans.localScale = new Vector2(2f, 2f);


        return UItlGO;
    }

    GameObject CreateBr(GameObject parent)
    {

        GameObject UIbrGO = new GameObject("br");
        UIbrGO.transform.SetParent(parent.transform);


        RectTransform brtrans = UIbrGO.AddComponent<RectTransform>(); //Create a rectTransform component for UItextGO, called trans
        brtrans.anchoredPosition = new Vector2(gameObject.GetComponent<TextData>().brx, gameObject.GetComponent<TextData>().bry);

        Image brc = UIbrGO.AddComponent<Image>(); //create a text component within UItextGO, naming it text
        brc.sprite = br;
        brtrans.localScale = new Vector2(2f, 2f);


        return UIbrGO;
    }




    GameObject CreateText2(GameObject parent) //CreateText2 works the exact same as CreateText, but instead utilizes the text, x, y, width, and height values from the TextData script meant for the second text box instead of the first
    {
        GameObject UItextGO = new GameObject("Text2");
        UItextGO.transform.SetParent(parent.transform);

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

    GameObject CreateBox2(GameObject parent)
    {
        GameObject UIboxGO = new GameObject("Box2");
        UIboxGO.transform.SetParent(parent.transform);


        RectTransform boxtrans = UIboxGO.AddComponent<RectTransform>(); //Create a rectTransform component for UItextGO, called trans
        boxtrans.anchoredPosition = new Vector2(gameObject.GetComponent<TextData>().boxPosX2, gameObject.GetComponent<TextData>().boxPosY2);

        Image box = UIboxGO.AddComponent<Image>(); //create a text component within UItextGO, naming it text
        box.sprite = whiteBox;
        boxtrans.sizeDelta = new Vector2(gameObject.GetComponent<TextData>().boxWidth2, gameObject.GetComponent<TextData>().boxHeight2);


        return UIboxGO;
    }

    GameObject CreateTl2(GameObject parent)
    {

        GameObject UItlGO = new GameObject("tl2");
        UItlGO.transform.SetParent(parent.transform);


        RectTransform tltrans = UItlGO.AddComponent<RectTransform>(); //Create a rectTransform component for UItextGO, called trans
        tltrans.anchoredPosition = new Vector2(gameObject.GetComponent<TextData>().tlx2, gameObject.GetComponent<TextData>().tly2);

        Image tlc = UItlGO.AddComponent<Image>(); //create a text component within UItextGO, naming it text
        tlc.sprite = tl;
        tltrans.localScale = new Vector2(2f, 2f);


        return UItlGO;
    }

    GameObject CreateBr2(GameObject parent)
    {

        GameObject UIbrGO = new GameObject("br2");
        UIbrGO.transform.SetParent(parent.transform);


        RectTransform brtrans = UIbrGO.AddComponent<RectTransform>(); //Create a rectTransform component for UItextGO, called trans
        brtrans.anchoredPosition = new Vector2(gameObject.GetComponent<TextData>().brx2, gameObject.GetComponent<TextData>().bry2);

        Image brc = UIbrGO.AddComponent<Image>(); //create a text component within UItextGO, naming it text
        brc.sprite = br;
        brtrans.localScale = new Vector2(2f, 2f);


        return UIbrGO;
    }


    GameObject CreateImage(GameObject parent)
    {
        GameObject UIimgGO = new GameObject("Img");
        UIimgGO.transform.SetParent(parent.transform);


        RectTransform imgtrans = UIimgGO.AddComponent<RectTransform>(); //Create a rectTransform component for UItextGO, called trans
        imgtrans.anchoredPosition = new Vector2(gameObject.GetComponent<ImageData>().xPos, gameObject.GetComponent<ImageData>().yPos);

        Image img = UIimgGO.AddComponent<Image>(); //create a text component within UItextGO, naming it text
        img.sprite = gameObject.GetComponent<ImageData>().image;
        imgtrans.sizeDelta = new Vector2(gameObject.GetComponent<ImageData>().width, gameObject.GetComponent<ImageData>().height);


        return UIimgGO;
    }


}
