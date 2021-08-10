using UnityEngine;
using UnityEngine.EventSystems;

namespace Scripts
{
    public class PinchZoom : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        private bool _isDragging; //bool that determines of the attached object is being dragged
        public float minScale, maxScale; //stores the minimum and maximum scales the object can be


        float initialDistance; //stores the distance between both touch points at the start of pinching
        Vector3 initialScale; //stores the initial scale the object is at at the start of pinching
        

        public void OnPointerDown(PointerEventData eventData)
        { 
            //if touchCount is greater than 0 (meaning if at least one finger is touching the object)
            if (Input.touchCount > 0)
            {
                _isDragging = true; //set isDragging to true
            }
        }


        public void OnPointerUp(PointerEventData eventData) //when the finger or pointer is taken off the screen
        {
            _isDragging = false; //set isDragging to false
        }


        private void Update()
        {
            //if touchCount is exactly 2 and isDragging is true
            if (Input.touchCount == 2 && _isDragging)
            {
                var touchZero = Input.GetTouch(0); //create a variable called touchZero and set its value as the first finger to touch the screen
                var touchOne = Input.GetTouch(1); //create a variable called touchOne and set its value as the second finger to touch the screen

                //if touchZero's or touchOne's touch phase has been ended or cancelled
                if (touchZero.phase == TouchPhase.Ended || touchZero.phase == TouchPhase.Canceled || touchOne.phase == TouchPhase.Ended || touchOne.phase == TouchPhase.Canceled)
                {
                    return; //return nothing
                }

                //if touchZero's or touchOne's touch phase begins
                if (touchZero.phase == TouchPhase.Began || touchOne.phase == TouchPhase.Began)
                {
                    initialDistance = Vector2.Distance(touchZero.position, touchOne.position); //set the initialDistance value to the distance between the 2 fingers
                    initialScale = gameObject.transform.localScale; //set the initialScale to the localScale of the object
                }
                else //or else
                {
                    var currentDistance = Vector2.Distance(touchZero.position, touchOne.position); //declare variable currentDistance and set its value as the distance between the 2 fingers

                    //if the initial distance is approximately 0
                    if(Mathf.Approximately(initialDistance, 0))
                    {
                        return; //return nothing
                    }

                    var factor = currentDistance / initialDistance; //declare variable factor and set its value to the current distance divided by the initial distance
                    gameObject.transform.localScale = initialScale * factor; //set the object's scale to the initial scale times the factor
                }
            }
        }
    }
}
