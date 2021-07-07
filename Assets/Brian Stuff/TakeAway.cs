using System.Collections;
using System.Collections.Generic;
using Brian_Stuff;
using TallahasseePrototype.Scripts;
using UnityEngine;
using UnityEngine.UI;

public class TakeAway : MonoBehaviour
{
    public GameObject cardStack;
    public int cardNum;
    
    
    

    //public GameObject startUI;
    public GameObject infoUI; //the gameobject variable holding the constant UI
    //public GameObject appManager; //the gameobject variable holding the app manager

    GameObject myGO; //the gameObject that creates the temporary canvas
    public GameObject testUI;
    
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
        
        if (!infoUI.activeSelf) //if the infoUI is currently inactive
        {
            
            infoUI.SetActive(true); //set the infoUI to active

            myGO = Instantiate(testUI);
            
            myGO.name = "TestCanvas";
            cardStack = myGO.transform.GetChild(0).gameObject;
            cardNum = gameObject.GetComponent<CardStack>().cards.Length;

            cardStack.GetComponent<CardStackTest>().cards = new Transform[cardNum];
            
            
            for (int i = 0; i < cardNum; i++)
            {
                cardStack.GetComponent<CardStackTest>().cards[i] = Instantiate(gameObject.transform.GetChild(cardNum - 1 - i));
                cardStack.GetComponent<CardStackTest>().cards[i].transform.parent = cardStack.transform;
                cardStack.GetComponent<CardStackTest>().cards[i].GetComponent<RectTransform>().localScale = new Vector3(1000f, 1000f, 1f);
            }
            
            myGO.SetActive(true);

            
        }

    }
    
}
