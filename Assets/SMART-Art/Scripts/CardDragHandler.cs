using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Scripts
{
    
    public class CardDragHandler : MonoBehaviour, IDragHandler, IEndDragHandler
    {
        bool interactable = true;
        private CardStack cardStack;

        private void Awake()
        {
            cardStack = gameObject.GetComponent<CardStack>();
        }

        private void Update()
        {
            if (Input.touchCount == 0)
            {
                interactable = true;
            }
        }

        // must implement or IEndDragHandler will not work
        public void OnDrag(PointerEventData eventData)
        {
            if (Input.touchCount > 1)
            {
                interactable = false;
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            // current, most recent swipe
            Vector3 dragVectorDirection = (eventData.position - eventData.pressPosition).normalized;
            float dragXDistance = Mathf.Abs(eventData.position.x - eventData.pressPosition.x);
            Debug.Log(dragXDistance);

            // get direction of that swipe
            var direction = GetDragDirection(dragVectorDirection);

            if (interactable && dragXDistance >= 75)
            {
                switch (direction)
                {
                    case DraggedDirection.Left:
                        cardStack.DecreaseOffset();
                        break;
                    case DraggedDirection.Right:
                        cardStack.IncreaseOffset();
                        break;
                    case DraggedDirection.Down:
                        cardStack.Reset();
                        gameObject.transform.GetComponentInParent<PoiDragHandler>().Unlock();
                        cardStack.gameObject.SetActive(false);
                        break;
                    case DraggedDirection.Up:
                        break;


                }
            }


        }

        // determine the direction of a drag
        private DraggedDirection GetDragDirection(Vector3 dragVector)
        {
            var positiveX = Mathf.Abs(dragVector.x); // min swipe dist?
            var positiveY = Mathf.Abs(dragVector.y); // min swipe dist?
            DraggedDirection draggedDir;
            
            if (positiveX > positiveY)
                draggedDir = dragVector.x > 0 ? DraggedDirection.Right : DraggedDirection.Left;
            else
            {
                draggedDir = dragVector.y > 0 ? DraggedDirection.Up : DraggedDirection.Down;
            }

            return draggedDir;
        }

        private enum DraggedDirection
        {
            Down,
            Right,
            Left,
            Up
        }
    }
}