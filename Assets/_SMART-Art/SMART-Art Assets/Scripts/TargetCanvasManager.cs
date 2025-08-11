using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Vuforia;
using static System.Net.Mime.MediaTypeNames;

public class TargetCanvasManager : MonoBehaviour
{
    [SerializeField] private Canvas canvas;
    //image that shows the scanned image
    [SerializeField] private UnityEngine.UI.Image image;
    //the behavior for the image target to find the image on the target
    [SerializeField] private ImageTargetBehaviour imageTargetBehaviour;
    //the text shown for the point of interests when not in ar mode
    [SerializeField] private TextMeshProUGUI text;
    //the ar mode toggle
    [SerializeField] private Toggle toggle;

    private void Start()
    {
        canvas.enabled = false;
    }
    //enables the canvas for non ar mode to show the information on the screen
    public void EnableCanvas()
    {
        //checks if the canvas is already enabled and ar mode is off
        if (canvas.enabled == false && toggle.isOn == false)
        {
            canvas.sortingOrder = 1;
            canvas.enabled = true;
            Texture2D tex = imageTargetBehaviour.GetRuntimeTargetTexture(); //texture taken from the target
            //sets the image on the non ar mode canvas to the scanned target here
            Sprite newSprite = Sprite.Create(tex, new Rect(0,0, tex.width, tex.height), new Vector2(1f,1f));
            image.sprite = newSprite;
            RectTransform canvasRectTransform = canvas.GetComponent<RectTransform>();

            //scales the image to fit into the canvas
            float imageSizeDiff = 0;
            if (tex.height > tex.width)
            {
                imageSizeDiff = canvasRectTransform.rect.height / tex.height;
            }
            else
            {
                imageSizeDiff = canvasRectTransform.rect.width / tex.width;
            }

            image.rectTransform.sizeDelta = new Vector2(tex.width * imageSizeDiff, tex.height * imageSizeDiff);

            //deactivates all of the ar mode cards when scanning outside of ar mode
            var POIButtons = GameObject.FindGameObjectsWithTag("Card");
            for(int i = 0; i < POIButtons.Length; i++)
            {
                POIButtons[i].SetActive(false);
            }
        }
    }
    //not currently doing anything but meant to make POI text fit on the canvas
    public void resizeText()
    {
        RectTransform canvasRectTransform = canvas.GetComponent<RectTransform>();
        //text.rectTransform.sizeDelta = new Vector2(canvasRectTransform.rect.width - 60, canvasRectTransform.rect.height - 60);
    }
    //disables the canvas and pushes it back so it doesn't block raycasts
    public void DisableCanvas()
    {
        canvas.enabled = false;
        canvas.sortingOrder = -1;
    }
}
