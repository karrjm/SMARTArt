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
        

        public void OnDrag(PointerEventData eventData)
        {
            if (!gameObject.GetComponent<TutorialDragHandler>().interactable)
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
