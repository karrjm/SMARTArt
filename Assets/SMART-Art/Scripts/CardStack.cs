using UnityEngine;

namespace Scripts
{
    [RequireComponent(typeof(CardDragHandler))]
    public class CardStack : MonoBehaviour
    {
        [SerializeField] private int cardDistance = 1;
        [SerializeField] private float cardMoveSpeed = 8f;
        // [SerializeField] private int cardZMultiplier = 32;
        [SerializeField] public Transform[] cards;
        private int _cardArrayOffset;
        private Vector3[] _cardPositions;
        private int _lower;
        private UIFader _uiFader;
        private int _upper;

        private void Awake()
        {
            _lower = cards.GetLowerBound(0);
            _upper = cards.GetUpperBound(0);
            _uiFader = gameObject.GetComponent<UIFader>();
        }

        public void Reset()
        {
            _cardArrayOffset = 0;
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
                cards[i].localPosition = Vector3.Lerp(cards[i].localPosition, _cardPositions[i + _cardArrayOffset],
                    Time.deltaTime * cardMoveSpeed);
                if (!(Mathf.Abs(cards[i].localPosition.x - _cardPositions[i + _cardArrayOffset].x) < 0.01f)) continue;
                cards[i].localPosition = _cardPositions[i + _cardArrayOffset];

                var cg = cards[i].gameObject.GetComponent<CanvasGroup>();

                // Disables interaction with cards that are not on top of the stack and calls the UIFader.
                if (cards[i].localPosition.x == 0)
                {
                    cg.interactable = true;
                    _uiFader.FadeIn(cg);
                }
                else
                {
                    cg.interactable = false;
                    _uiFader.FadeToHalf(cg);
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

            if (_cardPositions.Length < 2)
            {
                _cardPositions[0] = Vector3.zero;
            }
            else
            {
                // This loop is for cards still in the stack.		
                for (var i = cards.Length; i > -1; i--)
                    if (i < cards.Length - 1)
                        // z: cardZMultiplier * Mathf.Abs(i + 1 - cards.Length)
                        _cardPositions[i] = new Vector3(-cardDistance + _cardPositions[i + 1].x, 0,0);
                    else
                        _cardPositions[i] = Vector3.zero;

                // This loop is for cards outside of the stack.
                for (var i = cards.Length; i < _cardPositions.Length; i++)
                    _cardPositions[i] = new Vector3(cardDistance + _cardPositions[i - 1].x, 0,0);

                // old way
                // cardPositions[i] = new Vector3(usedCardXPos + 4 * (i - cards.Length), 0,-2 + -2 * (i - cards.Length));
            }
        }

        public int GetOffset()
        {
            return _cardArrayOffset;
        }
    }
}