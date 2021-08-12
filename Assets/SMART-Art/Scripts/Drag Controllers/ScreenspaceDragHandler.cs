using Scripts.Stacks;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Scripts.Drag_Controllers
{
    public class ScreenspaceDragHandler : MonoBehaviour, IDragHandler, IEndDragHandler
    {
        private AppManagerScript app;
        private GameObject appManager;
        public bool interactable = true;
        private ScreenspaceCardStack screenspaceCardStack;

        private void Awake()
        {
            screenspaceCardStack =
                gameObject
                    .GetComponent<
                        ScreenspaceCardStack>(); //set the previously declared CardStack variable to the card stack script attached to the object
            app = FindObjectOfType<AppManagerScript>();
            appManager = GameObject.Find("ARCamera");
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

            // get direction of current swipe
            var direction = GetDragDirection(dragVectorDirection);

            if (interactable)
            {
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