using System.Collections;
using Scripts.Drag_Controllers;
using Scripts.Stacks;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts
{
    public class AppManagerScript : MonoBehaviour
    { 
        public GameObject activeStack;
        public GameObject currentButton;
        
        public void NullActiveStack()
        {

            activeStack.GetComponent<CardStack>().Reset();
            // unlock topic swipe ability
            activeStack.transform.GetComponentInParent<TopicDragHandler>().Unlock();
            // set any pinch zoom back to default
            activeStack.transform.localScale = Vector3.one;
            // set the card stack inactive
            activeStack.gameObject.SetActive(false);
            activeStack = null;


            StartCoroutine(ReEnableButton());

        }

        private IEnumerator ReEnableButton()
        {
            yield return new WaitForSeconds(0.25f);
            currentButton.GetComponent<Button>().interactable = true;
            currentButton = null;
        }

    }
}
