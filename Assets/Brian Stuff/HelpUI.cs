using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


//SCRIPT MAY NOT BE USED IN FINAL VERSION OF THE APP
public class HelpUI : MonoBehaviour
{

    [SerializeField] private bool UIactive = false;
    public GameObject helpButton;
    

    public Sprite helpButtonSprite;
    public Sprite xButtonSprite;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HelpUISpawner()
    {
        if (!UIactive)
        {
            helpButton.GetComponent<Image>().sprite = xButtonSprite;
            gameObject.transform.GetChild(1).gameObject.SetActive(true);
            UIactive = true;
        }
        else
        {
            helpButton.GetComponent<Image>().sprite = helpButtonSprite;
            gameObject.transform.GetChild(1).gameObject.SetActive(false);
            UIactive = false;
        }
    }
}
