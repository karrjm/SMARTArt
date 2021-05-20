using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
    public GameObject infoUI; //the variable holding the infoUI, or the constant UI that stays the same
    public GameObject tempUI; //the variable holding the tempUI, or the variable UI that changes 
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Dismiss()//declaration of the Dismiss function
    {
        Destroy(GameObject.Find("TestCanvas")); //destroy the game object in scene called "TestCanvas"
        infoUI.SetActive(false); //set the infoUI object to inactive
    }
}
