using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

namespace Scripts
{
    public class GameManagerScript : MonoBehaviour
    { 
        public bool screenSpaceActive = false;
        public GameObject activeStack = null;
        

        public void Dismiss()//declaration of the Dismiss function
        {
            Destroy(GameObject.Find("TestCanvas")); //destroy the game object in scene called "TestCanvas"
            screenSpaceActive = false;
            //startUI.SetActive(true);
        }


        public void NullActiveStack()
        {
            activeStack = null;
        }
    }
}
