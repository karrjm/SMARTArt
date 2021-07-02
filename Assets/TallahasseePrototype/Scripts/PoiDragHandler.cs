using UnityEngine;
using UnityEngine.EventSystems;

namespace TallahasseePrototype.Scripts
{
    public class PoiDragHandler : MonoBehaviour, IDragHandler, IEndDragHandler
    {
        private PoiStack poiStack;

        private void Awake()
        {
            poiStack = gameObject.GetComponent<PoiStack>();
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
                    poiStack.DecreaseOffset();
                    break;
                case DraggedDirection.Right:
                    poiStack.IncreaseOffset();
                    break;
            }
        }

        // determine the direction of a drag
        private DraggedDirection GetDragDirection(Vector3 dragVector)
        {
            DraggedDirection draggedDir;
            draggedDir = dragVector.x > 0 ? DraggedDirection.Right : DraggedDirection.Left;
            return draggedDir;
        }

        private enum DraggedDirection
        {
            Right,
            Left
        }
    }
}