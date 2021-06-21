using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnChildren : MonoBehaviour
{
    public bool childrenActive = true;
    public GameObject child;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnChildrenButton()
    {
        if (!childrenActive)
        {
            child.SetActive(true);
            childrenActive = true;
        }
        else
        {
            GameObject.Find("HelpUI").SetActive(false);
            childrenActive = false;
        }
    }
}
