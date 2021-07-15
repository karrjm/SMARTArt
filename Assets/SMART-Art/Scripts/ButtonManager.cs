using UnityEngine;
using UnityEngine.UI;

namespace Scripts
{
    public class ButtonManager : MonoBehaviour
    {
        private Button[] buttons;

        private void Awake()
        {
            buttons = FindObjectsOfType<Button>();
            foreach (var button in buttons)
            {
                print(button);
            }
        }

        private void Update()
        {
        
        }
    }
}
