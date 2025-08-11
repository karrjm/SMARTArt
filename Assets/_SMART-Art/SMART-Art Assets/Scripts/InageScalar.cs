using UnityEngine;
using UnityEngine.UI;

public class InageScalar : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private GameObject topicStack;
    public void scaleUI()
    {
        var rect = topicStack.GetComponent<RectTransform>();
        //rect.sizeDelta = new Vector2(image.rectTransform.rect.width, image.rectTransform.rect.height);
    }
}
