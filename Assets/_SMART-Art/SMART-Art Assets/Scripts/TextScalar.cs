using TMPro;
using UnityEngine;

public class TextScalar : MonoBehaviour
{
    [SerializeField] private Canvas canvas;
    [SerializeField] private TextMeshProUGUI text;
    private void OnEnable()
    {
        RectTransform canvasRectTransform = canvas.GetComponent<RectTransform>();
        text.rectTransform.sizeDelta = new Vector2(canvasRectTransform.rect.width - 60, canvasRectTransform.rect.height - 60);
    }
}
