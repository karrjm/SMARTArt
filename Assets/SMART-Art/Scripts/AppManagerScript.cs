using UnityEngine;

namespace Scripts
{
    public class AppManagerScript : MonoBehaviour
    { 
        public GameObject activeStack;

        public void NullActiveStack()
        {
            activeStack = null;
        }
    }
}
