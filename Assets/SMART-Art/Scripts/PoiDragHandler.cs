using UnityEngine;
using UnityEngine.EventSystems;

namespace Scripts
{
    public class PoiDragHandler : MonoBehaviour, IDragHandler, IEndDragHandler
    {
        private bool interactable = true;
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
            var dragXDistance = Mathf.Abs(eventData.position.x - eventData.pressPosition.x);
            var dragYDistance = Mathf.Abs(eventData.position.y - eventData.pressPosition.y);

            // get direction of that swipe
            var direction = GetDragDirection(dragVectorDirection);

            var minDragDist = 100;
            if (interactable && (dragXDistance >= minDragDist || dragYDistance >= minDragDist))
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

        public void Lock()
        {
            interactable = false;
        }

        public void Unlock()
        {
            interactable = true;
        }

        private enum DraggedDirection
        {
            Right,
            Left
        }
    }
}