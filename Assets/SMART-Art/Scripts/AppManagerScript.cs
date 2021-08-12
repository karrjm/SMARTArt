using UnityEngine;

namespace Scripts
{
    public class AppManagerScript : MonoBehaviour
    { 
        public bool screenSpaceActive;
        public GameObject activeStack;
        

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
