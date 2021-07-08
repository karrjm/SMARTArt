using UnityEngine;
using UnityEngine.Serialization;

namespace Scripts
{
    public class Helper : MonoBehaviour
    {
        [FormerlySerializedAs("infoUI")] [SerializeField] private GameObject XButton;
    
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public void Activate()
        {
            if (XButton.activeSelf)
            {
            
            }
            else
            {
                gameObject.SetActive(true);
            }
        }
    }
}
