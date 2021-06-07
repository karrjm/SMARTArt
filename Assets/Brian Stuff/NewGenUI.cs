using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewGenUI : MonoBehaviour
{
    public Canvas ui;

    public GameObject startUI;
    public GameObject infoUI; //the gameobject variable holding the constant UI
    public GameObject appManager; //the gameobject variable holding the app manager

    GameObject myGO; //the gameObject that crteates the temporary canvas

    public float resoX; //stores the x value of the current screen resolution
    public float resoY; //stores the y value of the current screen resolution

    public int numOfThings;


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
            gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().color = new Color(0f, 100f, 100f);
        }
    }

    private void OnMouseUp()
    {
        gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().color = new Color(255f, 255f, 255f);

        if(!infoUI.activeSelf)
        {
            startUI.SetActive(false);
            infoUI.SetActive(true); //sets the infoUI to active
            appManager.GetComponent<GameManagerScript>().selectedPanel = this.gameObject;

            myGO = new GameObject(); //sets the myGO object as a new game object
            myGO.name = "TestCanvas"; //names the myGO object "TestCanvas" in the heirarchy

            Canvas childUI = Instantiate(ui);
            childUI.transform.parent = GameObject.Find("TestCanvas").transform;

            childUI.GetComponent<CanvasScaler>().uiScaleMode = infoUI.GetComponent<CanvasScaler>().uiScaleMode; //sets the scale mode of the temporary UI (TestCanvas) to the same mode as the constant UI

            resoX = (float)Screen.currentResolution.width; //sets resoX to the current x value of the screen resolution
            resoY = (float)Screen.currentResolution.height; //sets resoY to the current y value of the screen resolution

            childUI.GetComponent<CanvasScaler>().referenceResolution = new Vector2(resoX, resoY); //sets the reference resolution of myGO canvas to the same x and y values as resoX and resoY

            appManager.GetComponent<GameManagerScript>().tempUI = childUI;
        }
    }
}
