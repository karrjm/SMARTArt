using UnityEngine;

namespace Scripts
{
    public class Helper : MonoBehaviour
    {
        private int activeTrackers;
        private bool firstFound;
        private bool screenActive = true;

        public void Awake()
        {
            GameObject.Find("ARCamera");
        }

        public void Activate()
        {
            if (activeTrackers > 0 || screenActive)
            {
            }
            else
            {
                gameObject.SetActive(true);
            }
        }

        public void SetScreenActive()
        {
            screenActive = !screenActive;
        }

        public void IncreaseTrackers()
        {
            activeTrackers++;
            if (!firstFound) firstFound = true;
        }

        public void DecreaseTrackers()
        {
            if (firstFound) activeTrackers--;
        }
    }
}