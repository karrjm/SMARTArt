using Scripts.Stacks;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Scripts.Drag_Controllers
{
    public class TopicDragHandler : MonoBehaviour, IDragHandler, IEndDragHandler
    {
        private bool interactable = true;
        private TopicStack topicStack;

        private void Awake()
        {
            topicStack = gameObject.GetComponent<TopicStack>();
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
                        topicStack.DecreaseOffset();
                        break;
                    case DraggedDirection.Right:
                        topicStack.IncreaseOffset();
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