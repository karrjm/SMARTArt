using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Helper : MonoBehaviour
{
    [SerializeField] private GameObject infoUI;
    
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
        if (infoUI.activeSelf)
        {
            
        }
        else
        {
            gameObject.SetActive(true);
        }
    }
}
