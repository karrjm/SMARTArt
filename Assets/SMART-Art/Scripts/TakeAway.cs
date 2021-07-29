using Scripts.Stacks;
using UnityEngine;

namespace Scripts
{
    public class TakeAway : MonoBehaviour
    {
        private GameObject cardStack;
        private int cardNum;
        public GameObject testUI;
        private int cardOffset;
        private GameObject myGO;

        public void TakeSlides()
        {
            myGO = Instantiate(testUI);
            myGO.name = "TestCanvas";
            cardStack = myGO.transform.GetChild(0).gameObject;
            cardNum = gameObject.GetComponent<CardStack>().cards.Length;
            cardOffset = gameObject.GetComponent<CardStack>().GetOffset();
            cardStack.GetComponent<ScreenspaceCardStack>().cards = new Transform[cardNum];

            for (var i = 0; i < cardNum; i++)
            {
                cardStack.GetComponent<ScreenspaceCardStack>().cards[i] = Instantiate(gameObject.transform.GetChild(i));
                // cardStack.GetComponent<ScreenspaceCardStack>().cards[i].transform.parent = cardStack.transform;
                cardStack.GetComponent<ScreenspaceCardStack>().cards[i].transform.SetParent(cardStack.transform, false);
                cardStack.GetComponent<ScreenspaceCardStack>().cards[i].GetComponent<RectTransform>().localScale = new Vector3(400f, 400f, 1f);
                cardStack.GetComponent<ScreenspaceCardStack>().cards[i].GetComponent<CanvasGroup>().alpha = 1f;
            }

            myGO.SetActive(true);
            cardStack.GetComponent<ScreenspaceCardStack>().cardArrayOffset = cardOffset;
        }
    }
}