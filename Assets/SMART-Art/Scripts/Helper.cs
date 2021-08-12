using UnityEngine;

namespace Scripts
{
    public class Helper : MonoBehaviour
    {
        public GameObject appManager;
        private int activeTrackers;
        private bool firstFound;

        public void Activate()
        {
            if (appManager.GetComponent<AppManagerScript>().screenSpaceActive || activeTrackers > 0)
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
