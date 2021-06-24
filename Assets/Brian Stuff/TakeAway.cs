using System.Collections;
using System.Collections.Generic;
using Brian_Stuff;
using UnityEngine;
using UnityEngine.UI;

public class TakeAway : MonoBehaviour
{
    public GameObject cardStack;

    //public GameObject startUI;
    public GameObject infoUI; //the gameobject variable holding the constant UI
    //public GameObject appManager; //the gameobject variable holding the app manager

    GameObject myGO; //the gameObject that creates the temporary canvas

    public float resoX; //stores the x value of the current screen resolution
    public float resoY; //stores the y value of the current screen resolution

    //public int numOfThings;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeSlides()
    {
        if (!infoUI.activeSelf)
        {
            infoUI.SetActive(true);
            
            myGO = new GameObject(); //sets the myGO object as a new game object
            myGO.name = "TestCanvas"; //names the myGO object "TestCanvas" in the heirarchy
            myGO.AddComponent<CanvasScaler>();
            myGO.AddComponent<GraphicRaycaster>();
            myGO.GetComponent<Canvas>().renderMode = infoUI.GetComponent<Canvas>().renderMode;
            resoX = (float) Screen.currentResolution.width; //sets resoX to the current x value of the screen resolution
            resoY = (float) Screen.currentResolution.height; //sets resoY to the current y value of the screen resolution
            myGO.GetComponent<CanvasScaler>().uiScaleMode = infoUI.GetComponent<CanvasScaler>().uiScaleMode;
            myGO.GetComponent<CanvasScaler>().referenceResolution =
                new Vector2(resoX,
                    resoY); //sets the reference resolution of myGO canvas to the same x and y values as resoX and resoY

            GameObject spawnStack = Instantiate(cardStack.GetComponent<CardStackTest>()
                .cards[cardStack.GetComponent<CardStackTest>()._cardArrayOffset].GetChild(0).gameObject);

            spawnStack.transform.parent = GameObject.Find("TestCanvas").transform;
            spawnStack.GetComponent<RectTransform>().anchoredPosition = new Vector3(0f, -50f, 0f);
            spawnStack.GetComponent<RectTransform>().rotation = Quaternion.Euler(0f, 0f, 0f);
            spawnStack.GetComponent<RectTransform>().localScale = new Vector3(0.9f, 0.9f, 0.9f);

            spawnStack.AddComponent<Draggable>();
            spawnStack.AddComponent<PinchZoom>();
        }

    }

    /*
    private void OnMouseDown()
    {
        if (!infoUI.activeSelf)
        {
            gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().color = new Color(0f, 100f, 100f);
        }
    }

    private void OnMouseUp()
    {
        gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().color = new Color(255f, 255f, 255f);

        if(!infoUI.activeSelf)
        {
            //startUI.SetActive(false);
            //infoUI.SetActive(true); //sets the infoUI to active
            //appManager.GetComponent<GameManagerScript>().selectedPanel = this.gameObject;

            myGO = new GameObject(); //sets the myGO object as a new game object
            myGO.name = "TestCanvas"; //names the myGO object "TestCanvas" in the heirarchy
            myGO.AddComponent<CanvasScaler>();
            resoX = (float)Screen.currentResolution.width; //sets resoX to the current x value of the screen resolution
            resoY = (float)Screen.currentResolution.height; //sets resoY to the current y value of the screen resolution
            myGO.GetComponent<CanvasScaler>().referenceResolution = new Vector2(resoX, resoY);  //sets the reference resolution of myGO canvas to the same x and y values as resoX and resoY

            GameObject spawnStack = Instantiate(cardStack);
            spawnStack.transform.parent = GameObject.Find("TestCanvas").transform;

            //spawnStack.GetComponent<CanvasScaler>().uiScaleMode = infoUI.GetComponent<CanvasScaler>().uiScaleMode; //sets the scale mode of the temporary UI (TestCanvas) to the same mode as the constant UI

            // appManager.GetComponent<GameManagerScript>().tempUI = spawnStack;
        }
    }
    */
}
