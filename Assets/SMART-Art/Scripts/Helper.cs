using UnityEngine;
using UnityEngine.Serialization;

namespace Scripts
{
    public class Helper : MonoBehaviour
    {
        [SerializeField] private GameObject XButton;
        public GameObject appManager;

        public void Activate()
        {
            if (appManager.GetComponent<GameManagerScript>().screenSpaceActive)
            {
                
            }
            else
            {
                gameObject.SetActive(true);
            }
        }
    }
}
