using UnityEngine;

namespace Brian_Stuff
{
    public class CardStackTest : MonoBehaviour
    {
        [SerializeField] private float cardMoveSpeed = 8f;
        [SerializeField] private int cardZMultiplier = 32;
        [SerializeField] private bool useDefaultUsedXPos = true;
        [SerializeField] private int usedCardXPos = 1280;
        [SerializeField] public Transform[] cards;

        public int _cardArrayOffset;
        private Vector3[] _cardPositions;
        private int _lower;
        private int _upper;
        private int _xPowerDifference;

        private void Awake()
        {
            _lower = cards.GetLowerBound(0);
            _upper = cards.GetUpperBound(0);
            _xPowerDifference = 9 - cards.Length;
        }

        private void Start()
        {
            if (useDefaultUsedXPos)
            {
                var cardWidth = (int) cards[0].GetComponent<RectTransform>().rect.width;
                usedCardXPos = (int) (Screen.width * 0.5f + cardWidth);
            }

            CardInit();
        }

        private void Update()
        {
            MoveCards();
        }

        private void MoveCards()
        {
            // This loop moves the cards.
            for (var i = cards.Length - 1; i >= 0; i--)
            {
                if (i != cards.Length - 1 && _cardArrayOffset > i)
                {
                    cards[i].localPosition = Vector3.Lerp(cards[i].localPosition, _cardPositions[cards.Length - 1 + _cardArrayOffset],
                        Time.deltaTime * cardMoveSpeed);
                    if (!(Mathf.Abs(cards[i].localPosition.x - _cardPositions[i + _cardArrayOffset].x) < 0.01f)) continue;
                    cards[i].localPosition = _cardPositions[i + _cardArrayOffset];
                
                    // This disables interaction with cards that are not on top of the stack.
                    cards[i].GetComponent<CanvasGroup>().interactable = cards[i].localPosition.x == 0;
                }

                else
                {
                    cards[i].localPosition = Vector3.Lerp(cards[i].localPosition, _cardPositions[i - _cardArrayOffset],
                        Time.deltaTime * cardMoveSpeed);
                    if (!(Mathf.Abs(cards[i].localPosition.x - _cardPositions[i + _cardArrayOffset].x) < 0.01f))
                        continue;
                    cards[i].localPosition = _cardPositions[i + _cardArrayOffset];

                    // This disables interaction with cards that are not on top of the stack.
                    cards[i].GetComponent<CanvasGroup>().interactable = cards[i].localPosition.x == 0;
                }
            }
        }

        public void IncreaseOffset()
        {
            if (_cardArrayOffset < _upper) _cardArrayOffset++;
        }

        public void DecreaseOffset()
        {
            if (_cardArrayOffset > _lower) _cardArrayOffset--;
        }

        private void CardInit()
        {
            _cardPositions = new Vector3[cards.Length * 2 - 1];

            // This loop is for cards still in the stack.		
            for (var i = 0; i < cards.Length; i++)
                if (i != 0)
                    _cardPositions[i] = new Vector3(-Mathf.Pow(2, i + _xPowerDifference) + _cardPositions[i + 1].x, 0,
                        cardZMultiplier * Mathf.Abs(i + 1 - cards.Length));
                else
                    _cardPositions[i] = Vector3.zero;

            // This loop is for cards outside of the stack.
            for (var i = cards.Length; i < _cardPositions.Length; i++)
                _cardPositions[i] = new Vector3(usedCardXPos + 4 * (i - cards.Length), 0, -2 + -2 * (i - cards.Length));
        }
    }
}
