﻿using System.Collections;

namespace UnityEngine.UI.Extensions
{

    public class CardStack2D : MonoBehaviour
    {

        [SerializeField] private float minSwipeLength = 5f;
        [SerializeField] private float cardMoveSpeed = 8f;
        [SerializeField] private float buttonCooldownTime = 0.125f;
        [SerializeField] private int cardZMultiplier = 32;
        [SerializeField] private bool useDefaultUsedXPos = true;
        [SerializeField] private int usedCardXPos = 1280;
        [SerializeField] private Transform[] cards = null;


        private int cardArrayOffset;
        private Vector3[] cardPositions;
        private int xPowerDifference;
        private Vector2 firstPressPos;
        private Vector2 secondPressPos;
        private Vector2 currentSwipe;
        private Vector2 firstClickPos;
        private Vector2 secondClickPos;


        ///Static variables can be used across the scene if this script is in it.
        ///Thankfully it doesn't matter if another script attempts to use the variable and this script isn't in the scene. 
        public bool canUseHorizontalAxis = true;

        void Start()
        {
            ///I've found that 9 is a good number for this.
            ///I wouldn't really recommend changing it, but go ahead if you want to.		
            xPowerDifference = 9 - cards.Length;

            ///This is optional, but makes it super easy to figure out the off screen position for cards.
            ///Unfortunately, it's only really useful if the cards are the same width.
            if (useDefaultUsedXPos)
            {
                int cardWidth = (int)(cards[0].GetComponent<RectTransform>().rect.width);
                usedCardXPos = (int)(Screen.width * 0.5f + cardWidth);
            }

            cardPositions = new Vector3[cards.Length * 2 - 1];

            ///This loop is for cards still in the stack.		
            for (int i = cards.Length; i > -1; i--)
            {
                if (i < cards.Length - 1)
                {
                    cardPositions[i] = new Vector3(-Mathf.Pow(2, i + xPowerDifference) + cardPositions[i + 1].x, 0, cardZMultiplier * Mathf.Abs(i + 1 - cards.Length));
                }
                else
                {
                    cardPositions[i] = Vector3.zero;
                }
            }

            ///This loop is for cards outside of the stack.
            for (int i = cards.Length; i < cardPositions.Length; i++)
            {
                cardPositions[i] = new Vector3(usedCardXPos + 4 * (i - cards.Length), 0, -2 + -2 * (i - cards.Length));
            }
        }

        void Update()
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
                            cardArrayOffset--;
                            StartCoroutine(ButtonCooldown());
                        }
                        else if (currentSwipe.x > 0 && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f)
                        {
                            // Swipe right
                            cardArrayOffset++;
                            StartCoroutine(ButtonCooldown());
                        }
                    }
                }
            }

            ///This loop moves the cards.  I know that none of my lerps are the "right way," but it looks much nicer.
            for (int i = 0; i < cards.Length; i++)
            {
                cards[i].localPosition = Vector3.Lerp(cards[i].localPosition, cardPositions[i + cardArrayOffset], Time.deltaTime * cardMoveSpeed);
                if (Mathf.Abs(cards[i].localPosition.x - cardPositions[i + cardArrayOffset].x) < 0.01f)
                {
                    cards[i].localPosition = cardPositions[i + cardArrayOffset];

                    ///This disables interaction with cards that are not on top of the stack.
                    if (cards[i].localPosition.x == 0)
                    {
                        cards[i].gameObject.GetComponent<CanvasGroup>().interactable = true;
                    }
                    else
                    {
                        cards[i].gameObject.GetComponent<CanvasGroup>().interactable = false;
                    }
                }
            }
        }

        ///Stops the cards from scrolling super quickly if a button on the horizontal axis is held down.
        IEnumerator ButtonCooldown()
        {
            canUseHorizontalAxis = false;
            yield return new WaitForSeconds(buttonCooldownTime);
            canUseHorizontalAxis = true;
        }
    }
}