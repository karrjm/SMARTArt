using UnityEngine;
using UnityEngine.UI;

namespace I0plus.XduiUnity
{
    /// <summary>
    ///     トグルのON/OFFで画像の入れ替えをする
    /// </summary>
    [ExecuteAlways]
    public class ToggleGraphicSwap : MonoBehaviour
    {
        private Toggle toggle;

        private Toggle Toggle => toggle ? toggle : toggle = GetComponent<Toggle>();

        private void Awake()
        {
            Toggle.onValueChanged.AddListener(OnValueChanged);
        }

        private void OnEnable()
        {
            var target = Toggle.targetGraphic;
            if (target != null) target.enabled = !Toggle.isOn;
        }

        private void OnValueChanged(bool on)
        {
            var target = Toggle.targetGraphic;
            if (target != null) target.enabled = !on;
        }
    }
}