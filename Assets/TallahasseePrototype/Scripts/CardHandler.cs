using UnityEngine;
using UnityEngine.EventSystems;

namespace TallahasseePrototype.Scripts
{
    public class CardHandler : MonoBehaviour, IDragHandler, IEndDragHandler
    {
        private CardStack CardStack { get; set; }

        private void Awake()
        {
            CardStack = gameObject.GetComponent<CardStack>();
        }

        // must implement or IEndDragHandler will not work
        public void OnDrag(PointerEventData eventData)
        {
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            Vector3 dragVectorDirection = (eventData.position - eventData.pressPosition).normalized;
            GetDragDirection(dragVectorDirection);
            if (GetDragDirection(dragVectorDirection) == DraggedDirection.Left)
                // _cardArrayOffset--
                CardStack.DecreaseOffset();
            else if (GetDragDirection(dragVectorDirection) == DraggedDirection.Right)
                //_cardArrayOffset++;
                CardStack.IncreaseOffset();
        }

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