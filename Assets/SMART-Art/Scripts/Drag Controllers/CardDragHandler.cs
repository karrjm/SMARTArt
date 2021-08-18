using System.Collections;
using Scripts.Stacks;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Scripts.Drag_Controllers
{
    public class CardDragHandler : MonoBehaviour, IDragHandler, IEndDragHandler, IPointerUpHandler, IPointerDownHandler
    {
        private GameObject appManager;
        private CardStack cardStack;

        private float dragXDistance;
        private float dragYDistance;
        private bool interactable = true;
        private bool zeroed;

        private void Awake()
        {
            cardStack = gameObject.GetComponent<CardStack>();
            appManager = GameObject.Find("ARCamera");
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

                if (Input.touchCount > 1) interactable = false;
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
                switch (direction)
                {
                    case DraggedDirection.Left:
                        cardStack.DecreaseOffset();
                        break;
                    case DraggedDirection.Right:
                        cardStack.IncreaseOffset();
                        break;
                    case DraggedDirection.Down:
                        break;
                    case DraggedDirection.Up:
                        break;
                }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            dragXDistance = Mathf.Abs(eventData.position.x - eventData.pressPosition.x);
            dragYDistance = Mathf.Abs(eventData.position.y - eventData.pressPosition.y);

            if ((dragXDistance >= 0 || dragYDistance >= 0) && interactable)
            {
                var minDragDist = 100;
                if (interactable && dragXDistance < minDragDist && dragYDistance < minDragDist)
                {
                    // return to first
                    cardStack.Reset();
                    // unlock topic swipe ability
                    gameObject.transform.GetComponentInParent<TopicDragHandler>().Unlock();
                    // tell app manager there is no active stack
                    appManager.GetComponent<AppManagerScript>().NullActiveStack();
                    // set any pinch zoom back to default
                    gameObject.transform.localScale = Vector3.one;
                    // set the card stack inactive
                    cardStack.gameObject.SetActive(false);
                }
            }

            if (gameObject.activeSelf)
                StartCoroutine(ReEnableSwipe());
            else
                interactable = true;

            zeroed = false;
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

        private IEnumerator ReEnableSwipe()
        {
            yield return new WaitForSeconds(0.5f);
            interactable = true;
        }

        private enum DraggedDirection
        {
            Down,
            Right,
            Left,
            Up
        }
    }
}