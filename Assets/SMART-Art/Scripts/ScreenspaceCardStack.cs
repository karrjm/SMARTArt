using TallahasseePrototype.Scripts;
using UnityEngine;

namespace TallahasseePrototype.Merge
{
    public class ScreenspaceCardStack : MonoBehaviour
    {
        [SerializeField] private float cardMoveSpeed = 8f;
        [SerializeField] private int cardZMultiplier = 32;
        [SerializeField] public Transform[] cards;

        public int cardArrayOffset;
        private Vector3[] cardPositions;
        private int lower;
        private int upper;
        private const int XPowerDifference = 10;
        private UIFader uiFader;

        public void Awake()
        {
            //cards = new Transform[gameObject.GetComponent<TakeAway>().cardNum];
            lower = cards.GetLowerBound(0);
            upper = cards.GetUpperBound(0);
            uiFader = gameObject.GetComponent<UIFader>();
        }

        private void Start()
        {
            CardInit();
        }

        private void Update()
        {
            MoveCards();
        }

        private void MoveCards()
        {
            // This loop moves the cards.
            for (var i = 0; i < cards.Length; i++)
            {
                cards[i].localPosition = Vector3.Lerp(cards[i].localPosition, cardPositions[i + cardArrayOffset],
                    Time.deltaTime * cardMoveSpeed);
                if (!(Mathf.Abs(cards[i].localPosition.x - cardPositions[i + cardArrayOffset].x) < 0.01f)) continue;
                cards[i].localPosition = cardPositions[i + cardArrayOffset];
                
                var cg = cards[i].gameObject.GetComponent<CanvasGroup>();
                
                // This disables interaction with cards that are not on top of the stack.
                if (cards[i].localPosition.x == 0)
                {
                    cg.interactable = true;
                    uiFader.FadeIn(cg);
                }
                else
                {
                    cg.interactable = false;
                    uiFader.FadeOut(cg);
                }
            }
        }

        public void IncreaseOffset()
        {
            if (cardArrayOffset < upper) cardArrayOffset++;
        }

        public void DecreaseOffset()
        {
            if (cardArrayOffset > lower) cardArrayOffset--;
        }

        private void CardInit()
        {
            // overflow
            cardPositions = new Vector3[cards.Length * 2 - 1];

            if (cardPositions.Length < 2)
            {
                cardPositions[0] = Vector3.zero;
            }
            else
            {
                // This loop is for cards still in the stack.		
                for (var i = cards.Length; i > -1; i--)
                    if (i < cards.Length - 1)
                        cardPositions[i] = new Vector3(-Mathf.Pow(2,XPowerDifference) + cardPositions[i + 1].x, 0,
                            cardZMultiplier * Mathf.Abs(i + 1 - cards.Length));
                    else
                        cardPositions[i] = Vector3.zero;

                // This loop is for cards outside of the stack.
                for (var i = cards.Length; i < cardPositions.Length; i++)
                    cardPositions[i] = new Vector3(Mathf.Pow(2,XPowerDifference) + cardPositions[i - 1].x, 0,
                        cardZMultiplier * Mathf.Abs(i + 1 - cards.Length));
                
                // old way
                // cardPositions[i] = new Vector3(usedCardXPos + 4 * (i - cards.Length), 0,-2 + -2 * (i - cards.Length));
            }
        }
    }
}
