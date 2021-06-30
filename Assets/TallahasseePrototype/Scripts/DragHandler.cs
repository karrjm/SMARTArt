using UnityEngine;
using UnityEngine.EventSystems;

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

            switch (direction)
            {
                case DraggedDirection.Left:
                    cardStack.DecreaseOffset();
                    break;
                case DraggedDirection.Right:
                    cardStack.IncreaseOffset();
                    break;
                case DraggedDirection.Down:
                    cardStack.gameObject.SetActive(false);
                    break;
            }
        }

        // determine the direction of a drag
        private static DraggedDirection GetDragDirection(Vector3 dragVector)
        {
            var positiveX = Mathf.Abs(dragVector.x);
            var positiveY = Mathf.Abs(dragVector.y);
            DraggedDirection draggedDir;
            if (positiveX > positiveY)
                draggedDir = dragVector.x > 0 ? DraggedDirection.Right : DraggedDirection.Left;
            else
                draggedDir = dragVector.y > 0 ? DraggedDirection.Up : DraggedDirection.Down;

            return draggedDir;
        }

        private enum DraggedDirection
        {
            Up,
            Down,
            Right,
            Left
        }
    }
}