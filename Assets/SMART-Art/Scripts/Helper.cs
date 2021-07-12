using UnityEngine;
using UnityEngine.Serialization;

namespace Scripts
{
    public class Helper : MonoBehaviour
    {
        [SerializeField] private GameObject XButton;

        public void Activate()
        {
            gameObject.SetActive(true);
        }
    }
}
