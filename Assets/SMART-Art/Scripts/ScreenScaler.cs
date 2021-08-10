using UnityEngine;
using UnityEngine.UI;

namespace Scripts
{
    public class ScreenScaler : MonoBehaviour
    {
        public float resoX; //stores the x value of the screen resolution
        public float resoY; //stores the y value of the screen resolution

        private CanvasScaler can; //stores the canvas scaler
        // Start is called before the first frame update
        void Start()
        {
            can = gameObject.GetComponent<CanvasScaler>(); //sets the canvas scaler can to the canvas scaler component attached to the game object
            SetInfo(); //call the SetInfo function
            if (gameObject.CompareTag("InfoUI"))
            {
                gameObject.SetActive(false); //set the game object to inactive
            }
        }
        
        void SetInfo() //declaration of the SetInfo function
        {
            resoX = Screen.currentResolution.width; //set resX to the screen width
            resoY = Screen.currentResolution.height; //set resY to the screen height

            can.referenceResolution = new Vector2(resoX, resoY); //set the reference resolution of the canvas on the object to the same values as resX and ResY
        }
    }
}
