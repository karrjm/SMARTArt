using System;
using Scripts;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

namespace TallahasseePrototype.Scripts
{
    public class DragHandler : MonoBehaviour, IDragHandler, IEndDragHandler
    {
        private CardStack cardStack;

        private void Awake()
        {
            cardStack = gameObject.GetComponent<CardStack>();
        }

        // must implement or IEndDragHandler will not work
        public void OnDrag(PointerEventData eventData)
        {
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            // current, most recent swipe
            Vector3 dragVectorDirection = (eventData.position - eventData.pressPosition).normalized;

            // get direction of that swipe
            var direction = GetDragDirection(dragVectorDirection);

            if (direction == DraggedDirection.Left)
                cardStack.DecreaseOffset();
            else if (direction == DraggedDirection.Right)
                cardStack.IncreaseOffset();
            else if (direction == DraggedDirection.Down) cardStack.gameObject.SetActive(false);
        }

        // determine the direction of a drag
        private DraggedDirection GetDragDirection(Vector3 dragVector)
        {
            var positiveX = Mathf.Abs(dragVector.x); // min swipe dist?
            var positiveY = Mathf.Abs(dragVector.y); // min swipe dist?
            var draggedDir = DraggedDirection.None;
            if (positiveX > positiveY)
            {
                draggedDir = dragVector.x > 0 ? DraggedDirection.Right : DraggedDirection.Left;
            }
            else
            {
                draggedDir = dragVector.y > 0 ? DraggedDirection.Up : DraggedDirection.Down;
            }
            return draggedDir;
        }

        private enum DraggedDirection
        {
            Up,
            Down,
            Right,
            Left,
            None
        }
    }
}