using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardStack2D : MonoBehaviour
{
    [SerializeField] private float cardMoveSpeed = 8f;
    [SerializeField] private int cardZMultiplier = 32;
    [SerializeField] private bool useDefaultUsedXPos = true;
    [SerializeField] private int usedCardXPos = 1280;
    [SerializeField] private Transform[] cards = null;

    private TouchController touchController;
    private int cardArrayOffset;
    private Vector3[] cardPositions;
    private int xPowerDifference;
    private Vector2 firstClickPos;
    private Vector2 secondClickPos;

    void Start()
    {
        touchController = GetComponent<TouchController>();

        xPowerDifference = 9 - cards.Length;

        ///This is optional, but makes it easy to figure out the off screen position for cards.
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

        touchController.DetectSwipe();

        /// This loop moves the cards.
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
}