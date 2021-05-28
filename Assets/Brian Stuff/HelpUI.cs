using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpUI : MonoBehaviour
{
    bool helpActive = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnHelpUI()
    {
        if (!helpActive)
        {
            gameObject.GetComponent<RectTransform>().GetChild(0).gameObject.SetActive(true);
            gameObject.GetComponent<RectTransform>().GetChild(1).gameObject.SetActive(true);
            gameObject.GetComponent<RectTransform>().GetChild(2).gameObject.SetActive(true);
            gameObject.GetComponent<RectTransform>().GetChild(3).gameObject.SetActive(true);
            gameObject.GetComponent<RectTransform>().GetChild(4).gameObject.SetActive(true);
            helpActive = true;
        }
        else
        {
            gameObject.GetComponent<RectTransform>().GetChild(0).gameObject.SetActive(false);
            gameObject.GetComponent<RectTransform>().GetChild(1).gameObject.SetActive(false);
            gameObject.GetComponent<RectTransform>().GetChild(2).gameObject.SetActive(false);
            gameObject.GetComponent<RectTransform>().GetChild(3).gameObject.SetActive(false);
            gameObject.GetComponent<RectTransform>().GetChild(4).gameObject.SetActive(false);
            helpActive = false;
        }

    }
}
