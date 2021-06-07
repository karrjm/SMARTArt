using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextData : MonoBehaviour
{
    public string Text; //stores the text data of 1st textbox
    public float x; //holds the x pos value of 1st textbox
    public float y; //holds the y pos value of 1st textbox

    // WIDTH AND HEIGHT VALUES ARE REQUIRED INPUTS, OTHERWISE YOUR TEXT WONT SHOW UP    
    public float width; //holds width of 1st textbox
    public float height; //holds height of 1st textbox
    public int fontSize; //stores the font size of the 1st textbox
    public float boxPosX; //stores the white box x pos of the first text box
    public float boxPosY; //stores the white box y pos of the first text box
    public float boxWidth; //stores the width of the 1st white box
    public float boxHeight; //stores the height of the 1st white box
    public float tlx; //stores the x pos of the first top corner
    public float brx; //stores the x pos of the first bottom corner
    public float tly; //stores the y pos of the first top corner
    public float bry; //stores the y pos of the first bottom corner


    //SECOND SET OF VARIABLES ARE ONLY USED IF THE INFO PANEL UTILIZES 2 TEXT BOXES, WHICH IS THE MAXIMUM
    public string Text2; //stores the text data of the 2nd text box
    public float x2; // holds the x pos value of 2nd textbox
    public float y2; //holds y pos value of 2nd textbox
    public float width2; //holds width of 2nd textbox
    public float height2; //holds height of 2nd textbox
    public int fontSize2; //stores the font size of the second text box
    public float boxPosX2; //stores the x pos of the 2nd white box
    public float boxPosY2; //stores the y pos of the 2nd white box
    public float boxWidth2; //stores the width of the 2nd white box
    public float boxHeight2; //stores the height of the 2nd white box
    public float tlx2; //stores the x pos of the 2nd top corner
    public float brx2; //stores the x pos of the 2nd bottom corner
    public float tly2; //stores the y pos of the 2nd top corner
    public float bry2; //stores the y pos of the 2nd bottom corner

    

    // Start is called before the first frame update
    void Start()
    {

    }

    private void Awake()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
}
