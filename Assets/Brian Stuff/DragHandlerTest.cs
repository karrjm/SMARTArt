using UnityEngine;
using UnityEngine.EventSystems;

namespace Brian_Stuff
{
    public class DragHandlerTest : MonoBehaviour, IDragHandler, IEndDragHandler
    {
        private CardStackTest CardStack { get; set; }

        private void Awake()
        {
            CardStack = gameObject.GetComponent<CardStackTest>();
        }

        // must implement or IEndDragHandler will not work
        public void OnDrag(PointerEventData eventData)
        {
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            // current swipe
            Vector3 dragVectorDirection = (eventData.position - eventData.pressPosition).normalized;
            
            // get direction of current swipe
            GetDragDirection(dragVectorDirection);
            
            if (GetDragDirection(dragVectorDirection) == DraggedDirection.Left)
                // _cardArrayOffset--
                CardStack.DecreaseOffset();
            else if (GetDragDirection(dragVectorDirection) == DraggedDirection.Right)
                //_cardArrayOffset++;
                CardStack.IncreaseOffset();
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
