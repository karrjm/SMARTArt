using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

namespace Scripts
{
    public class CardDragHandler : MonoBehaviour, IDragHandler, IEndDragHandler, IPointerUpHandler, IPointerDownHandler
    {
        private CardStack cardStack;
        private GameObject appManager;
        private bool interactable = true;

        private float dragXDistance;
        private float dragYDistance;
        private bool zeroed = false;

        private void Awake()
        {
            cardStack = gameObject.GetComponent<CardStack>();
            appManager = GameObject.Find("AppManager");
        }

        private void Update()
        {
            if (Input.touchCount > 0)
            {
                if (!zeroed)
                {
                    dragXDistance = 0;
                    dragYDistance = 0;
                    zeroed = true;
                }
                if (Input.touchCount > 1)
                {
                    interactable = false;
                }
            }
            
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (Input.touchCount > 1) interactable = false;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            // current, most recent swipe
            Vector3 dragVectorDirection = (eventData.position - eventData.pressPosition).normalized;
            dragXDistance = Mathf.Abs(eventData.position.x - eventData.pressPosition.x);
            dragYDistance = Mathf.Abs(eventData.position.y - eventData.pressPosition.y);

            // get direction of that swipe
            var direction = GetDragDirection(dragVectorDirection);

            var minDragDist = 100;
            if (interactable && (dragXDistance >= minDragDist || dragYDistance >= minDragDist))
            {
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
                        appManager.GetComponent<GameManagerScript>().NullActiveStack();
                        gameObject.transform.localScale = Vector3.one;
                        cardStack.gameObject.SetActive(false);
                        break;
                    case DraggedDirection.Up:
                        break;
                }
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

        private enum DraggedDirection
        {
            Down,
            Right,
            Left,
            Up
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            dragXDistance = Mathf.Abs(eventData.position.x - eventData.pressPosition.x);
            dragYDistance = Mathf.Abs(eventData.position.y - eventData.pressPosition.y);

            if ((dragXDistance >= 0 || dragYDistance >= 0) && interactable )
            {
                var minDragDist = 100;
                if (interactable && ((dragXDistance < minDragDist && dragYDistance < minDragDist)))
                {
                    gameObject.GetComponent<TakeAway>().TakeSlides();
                    appManager.GetComponent<GameManagerScript>().screenSpaceActive = true;
                    appManager.GetComponent<GameManagerScript>().activeStack = null;
                    gameObject.SetActive(false);
                }
                    
            }
            interactable = true;
            zeroed = false;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            
        }
    }
}