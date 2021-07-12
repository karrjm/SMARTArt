using UnityEngine;
using UnityEngine.Serialization;

namespace Scripts
{
    public class GameManagerScript : MonoBehaviour
    {
        //public GameObject startUI;
        [FormerlySerializedAs("infoUI")] public GameObject XButton; //the variable holding the infoUI, or the constant UI that stays the same
        public Canvas tempUI; //the variable holding the tempUI, or the variable UI that changes 
        public GameObject selectedPanel; //the currently selected panel, only has a value during runtime
        public bool childrenActive = false;

        public void Dismiss()//declaration of the Dismiss function
        {
            Destroy(GameObject.Find("TestCanvas")); //destroy the game object in scene called "TestCanvas"
            XButton.SetActive(false); //set the infoUI object to inactive
            //startUI.SetActive(true);
        }

    }
}
