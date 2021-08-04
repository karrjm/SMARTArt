using System;
using Scripts.Drag_Controllers;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Scripts
{
    public class Draggable : MonoBehaviour, IDragHandler, IEndDragHandler, IPointerUpHandler, IPointerDownHandler
    {
        [SerializeField] private Canvas canvas;
        private RectTransform rectTransform;

        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
        }

        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (!gameObject.GetComponent<ScreenspaceDragHandler>().interactable)
            {
                rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
           
        }

        public void OnPointerUp(PointerEventData eventData)
        {
           
        }

        public void OnPointerDown(PointerEventData eventData)
        {
          
        }
    }
}
