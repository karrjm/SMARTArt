using UnityEngine;
using UnityEngine.Serialization;

namespace Scripts
{
    public class Helper : MonoBehaviour
    {
        public GameObject appManager;
        private int activeTrackers = 0;
        private bool firstFound = false;

        public void Activate()
        {
            if (appManager.GetComponent<GameManagerScript>().screenSpaceActive || activeTrackers > 0)
            {
                
            }
            else
            {
                gameObject.SetActive(true);
            }
        }

        public void IncreaseTrackers()
        {
            activeTrackers++;
            if (!firstFound)
            {
                firstFound = true;
            }
        }

        public void DecreaseTrackers()
        {
            if (firstFound)
            {
                activeTrackers--;
            }
        }
    }
}
