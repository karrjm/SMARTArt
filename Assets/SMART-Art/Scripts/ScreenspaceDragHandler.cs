using UnityEngine;
using UnityEngine.EventSystems;

namespace Scripts
{
    public class ScreenspaceDragHandler : MonoBehaviour, IDragHandler, IEndDragHandler
    {
        private GameManagerScript app;
        private bool interactable = true;
        private ScreenspaceCardStack screenspaceCardStack;

        private void Awake()
        {
            screenspaceCardStack =
                gameObject
                    .GetComponent<
                        ScreenspaceCardStack>(); //set the previously declared CardStack variable to the card stack script attached to the object
            app = FindObjectOfType<GameManagerScript>();
        }

        private void Update()
        {
            if (Input.touchCount == 0) interactable = true;
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (Input.touchCount > 1) interactable = false;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            // current swipe
            Vector3 dragVectorDirection = (eventData.position - eventData.pressPosition).normalized;
            var dragXDistance = Mathf.Abs(eventData.position.x - eventData.pressPosition.x);
            var dragYDistance = Mathf.Abs(eventData.position.y - eventData.pressPosition.y);

            // get direction of current swipe
            var direction = GetDragDirection(dragVectorDirection);

            var minDragDist = 100;
            if (interactable && (dragXDistance >= minDragDist || dragYDistance >= minDragDist))
                switch (direction)
                {
                    case DraggedDirection.Left:
                        screenspaceCardStack.DecreaseOffset();
                        break;
                    case DraggedDirection.Right:
                        screenspaceCardStack.IncreaseOffset();
                        break;
                    case DraggedDirection.Down:
                        screenspaceCardStack.Reset();
                        app.Dismiss();
                        break;
                    case DraggedDirection.Up:
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