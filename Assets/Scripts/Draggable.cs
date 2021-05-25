using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Draggable : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
   

    void Awake()
    {
      
    }

    //when the object begins being dragged
    public void OnBeginDrag(PointerEventData eventData)
    {
    }

    //on every frame the object is being dragged
    public void OnDrag(PointerEventData eventData)
    {
        gameObject.GetComponent<RectTransform>().anchoredPosition += eventData.delta; //set the anchored position of the object to the position of the finger
    }

    //once the object stops being dragged
    public void OnEndDrag(PointerEventData eventData)
    {
    }

    //if the pointer is on the object and pressed down
    public void OnPointerDown(PointerEventData eventData)
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
}
