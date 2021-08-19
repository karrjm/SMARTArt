using Scripts.Stacks;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Scripts.Drag_Controllers
{
    public class TopicDragHandler : MonoBehaviour, IDragHandler, IEndDragHandler
    {
        private bool interactable = true;

        private void Awake()
        {
            gameObject.GetComponent<TopicStack>();
        }

        private void Update()
        {
            if (Input.touchCount == 0) interactable = true;
        }


        // must implement or IEndDragHandler will not work
        public void OnDrag(PointerEventData eventData)
        {
            if (Input.touchCount > 1) interactable = false;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            // current, most recent swipe
            Vector3 dragVectorDirection = (eventData.position - eventData.pressPosition).normalized;

            // get direction of that swipe
            var direction = GetDragDirection(dragVectorDirection);

            if (interactable)
                switch (direction)
                {
                    case DraggedDirection.Left:

                        break;
                    case DraggedDirection.Right:

                        break;
                    case DraggedDirection.Down:
                        var parent = transform.parent;
                        parent.GetChild(0).gameObject.GetComponent<TopicStack>().DecreaseOffset();
                        parent.GetChild(1).gameObject.GetComponent<TopicStack>().DecreaseOffset();
                        break;
                    case DraggedDirection.Up:
                        var parent1 = transform.parent;
                        parent1.GetChild(0).gameObject.GetComponent<TopicStack>().IncreaseOffset();
                        parent1.GetChild(1).gameObject.GetComponent<TopicStack>().IncreaseOffset();
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
                draggedDir = dragVector.y > 0 ? DraggedDirection.Up : DraggedDirection.Down;

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
            Left,
            Down,
            Up
        }
    }
}