using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenScaler : MonoBehaviour
{
    public float resoX; //stores the x value of the screen resolution
    public float resoY; //stores the y value of the screen resolution

    private CanvasScaler can; //stores the canvas scaler
    // Start is called before the first frame update
    void Start()
    {
        can = gameObject.GetComponent<CanvasScaler>(); //sets the canvas scaler can to the canvas scaler compnent attached to the game object
        SetInfo(); //call the SetInfo function
        if (gameObject.tag == "InfoUI")
        {
            gameObject.SetActive(false); //set the game object to inactive
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SetInfo() //declaration of the SetInfo function
    {
        resoX = (float)Screen.currentResolution.width; //set resoX to the screen width
        resoY = (float)Screen.currentResolution.height; //set resoY to the screen height

        can.referenceResolution = new Vector2(resoX, resoY); //set the reference resolution of the canvas on the object to the same values as resoX and ResoY
    }
}
