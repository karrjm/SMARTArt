using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Helper : MonoBehaviour
{
    [FormerlySerializedAs("infoUI")] [SerializeField] private GameObject XButton;
    
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
        if (XButton.activeSelf)
        {
            
        }
        else
        {
            gameObject.SetActive(true);
        }
    }
}
