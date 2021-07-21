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
        //public GameObject startUI;
        [FormerlySerializedAs("infoUI")] public GameObject XButton; //the variable holding the infoUI, or the constant UI that stays the same
        public Canvas tempUI; //the variable holding the tempUI, or the variable UI that changes 
        public GameObject selectedPanel; //the currently selected panel, only has a value during runtime
        public bool childrenActive = false;
        public bool screenSpaceActive = false;
        public GameObject activeStack = null;
        

        public void Dismiss()//declaration of the Dismiss function
        {
            Destroy(GameObject.Find("TestCanvas")); //destroy the game object in scene called "TestCanvas"
            //startUI.SetActive(true);
        }


        public void NullActiveStack()
        {
            activeStack = null;
        }
    }
}
