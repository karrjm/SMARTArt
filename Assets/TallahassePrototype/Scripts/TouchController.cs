using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchController : MonoBehaviour
{
    [SerializeField] private float minSwipeLength = 5f;
    [SerializeField] private float buttonCooldownTime = 0.125f;

    private Vector2 firstPressPos;
    private Vector2 secondPressPos;
    private Vector2 currentSwipe;
    private bool canUseHorizontalAxis = true;

    public Vector2 CurrentSwipe { get { return currentSwipe; } }
    public bool CanUseHorizontalAxis { get { return canUseHorizontalAxis; } }

    public void DetectSwipe()
    {
        if (Input.touches.Length > 0)
        {
            Touch t = Input.GetTouch(0);

            if (t.phase == TouchPhase.Began)
            {
                firstPressPos = new Vector2(t.position.x, t.position.y);
            }

            if (t.phase == TouchPhase.Ended)
            {
                secondPressPos = new Vector2(t.position.x, t.position.y);
                currentSwipe = new Vector3(secondPressPos.x - firstPressPos.x, secondPressPos.y - firstPressPos.y);

                // Make sure it was a legit swipe, not a tap
                if (currentSwipe.magnitude < minSwipeLength)
                {
                    // not a touch
                    return;
                }
                currentSwipe.Normalize();


                /// TouchController
                if (canUseHorizontalAxis)
                {
                    if (currentSwipe.y > 0 && currentSwipe.x > -0.5f && currentSwipe.x < 0.5f)
                    {
                        // Swipe up
                    }
                    else if (currentSwipe.y < 0 && currentSwipe.x > -0.5f && currentSwipe.x < 0.5f)
                    {
                        // Swipe down
                    }
                    else if (currentSwipe.x < 0 && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f)
                    {
                        // Swipe left
                        Debug.Log("LEFT SWIPE");
                        // _cardArrayOffset--;
                        StartCoroutine(Cooldown());
                    }
                    else if (currentSwipe.x > 0 && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f)
                    {
                        // Swipe right
                        Debug.Log("RIGHT SWIPE");
                        // _cardArrayOffset++;
                        StartCoroutine(Cooldown());
                    }
                }
            }
        }
    }
    IEnumerator Cooldown()
    {
        canUseHorizontalAxis = false;
        yield return new WaitForSeconds(buttonCooldownTime);
        canUseHorizontalAxis = true;
    }
}
