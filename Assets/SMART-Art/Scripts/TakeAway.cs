using TallahasseePrototype.Scripts;
using UnityEngine;
using UnityEngine.Serialization;

namespace TallahasseePrototype.Merge
{
    public class TakeAway : MonoBehaviour
    {
        public GameObject cardStack;
        public int cardNum;
        private int cardOffset;
    
    
    

        //public GameObject startUI;
        [FormerlySerializedAs("infoUI")] public GameObject XButton; //the gameobject variable holding the constant UI
        //public GameObject appManager; //the gameobject variable holding the app manager

        GameObject myGO; //the gameObject that creates the temporary canvas
        public GameObject testUI;
    
        //public int numOfThings;

        public void TakeSlides()
        {
        
            if (!XButton.activeSelf) //if the infoUI is currently inactive
            {
            
                XButton.SetActive(true); //set the infoUI to active

                myGO = Instantiate(testUI);
            
                myGO.name = "TestCanvas";
                cardStack = myGO.transform.GetChild(0).gameObject;
                cardNum = gameObject.GetComponent<CardStack>().cards.Length;
                cardOffset = gameObject.GetComponent<CardStack>().GetOffset();
                
                

                cardStack.GetComponent<ScreenspaceCardStack>().cards = new Transform[cardNum];
            
            
                for (int i = 0; i < cardNum; i++)
                {
                    cardStack.GetComponent<ScreenspaceCardStack>().cards[i] = Instantiate(gameObject.transform.GetChild(cardNum - 1 - i));
                    // cardStack.GetComponent<ScreenspaceCardStack>().cards[i].transform.parent = cardStack.transform;
                    cardStack.GetComponent<ScreenspaceCardStack>().cards[i].transform.SetParent(cardStack.transform,false);
                    cardStack.GetComponent<ScreenspaceCardStack>().cards[i].GetComponent<RectTransform>().localScale = new Vector3(800f, 800f, 1f);
                    cardStack.GetComponent<ScreenspaceCardStack>().cards[i].GetComponent<CanvasGroup>().alpha = 1f;
                }
            
                myGO.SetActive(true);
                cardStack.GetComponent<ScreenspaceCardStack>().cardArrayOffset = cardOffset;


            }

        }
    
    }
}
