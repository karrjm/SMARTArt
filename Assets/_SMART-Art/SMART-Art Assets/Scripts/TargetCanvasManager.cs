using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Vuforia;

public class TargetCanvasManager : MonoBehaviour
{
    [SerializeField] private Canvas canvas;
    //[SerializeField] private GameObject target;
    [SerializeField] private UnityEngine.UI.Image image;
    [SerializeField] private ImageTargetBehaviour imageTargetBehaviour;

    private void Start()
    {
        canvas.enabled = false;
    }
    public void ToggleCanvas()
    {
        if (canvas.enabled == false)
        {
            canvas.enabled = true;
            Texture2D tex = imageTargetBehaviour.GetRuntimeTargetTexture();
            Sprite newSprite = Sprite.Create(tex, new Rect(0,0, tex.width, tex.height), new Vector2(1f,1f));
            image.sprite = newSprite;
            image.transform.localScale = new Vector2(4f,4f);
        }
        else
        {
            canvas.enabled = false;
        }
    }
}
