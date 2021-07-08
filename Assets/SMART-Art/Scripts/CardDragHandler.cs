using UnityEngine;
using UnityEngine.EventSystems;

namespace TallahasseePrototype.Scripts
{
    public class CardDragHandler : MonoBehaviour, IDragHandler, IEndDragHandler
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