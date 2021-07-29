using Scripts.Drag_Controllers;
using Scripts.Utilities;
using UnityEngine;

namespace Scripts.Stacks
{
    [RequireComponent(typeof(ScreenspaceDragHandler))]
    public class ScreenspaceCardStack : MonoBehaviour
    {
        [Tooltip("The distance between the cards in screenspace.")] [SerializeField]
        private int cardDistance = 100000;

        [Tooltip("The speed at which the cards move.")] [SerializeField]
        private float cardMoveSpeed = 8f;

        public Transform[] cards;
        public int cardArrayOffset;
        private Vector3[] _cardPositions;
        private UIFader _fader;
        private int _offsetLowerBound;
        private int _offsetUpperBound;

        public void Awake()
        {
            _fader = gameObject.GetComponent<UIFader>();
        }

        public void Reset()
        {
            cardArrayOffset = 0;
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
                cards[i].localPosition = Vector3.Lerp(cards[i].localPosition, _cardPositions[i + 1 + cardArrayOffset],
                    Time.deltaTime * cardMoveSpeed);
                if (Mathf.Abs(cards[i].localPosition.x - _cardPositions[i + 1 + cardArrayOffset].x) < 0.01f)
                {
                    cards[i].localPosition = _cardPositions[i + 1 + cardArrayOffset];

                    var cg = cards[i].gameObject.GetComponent<CanvasGroup>();

                    // This disables interaction with cards that are not on top of the stack.
                    if (cards[i].localPosition.x == 0)
                        cg.interactable = true;
                    // uiFader.FadeIn(cg);
                    else
                        cg.interactable = false;
                    // uiFader.FadeToHalf(cg);
                }
            }
        }

        public void IncreaseOffset()
        {
            if (cardArrayOffset < _offsetUpperBound) cardArrayOffset++;
        }

        public void DecreaseOffset()
        {
            if (cardArrayOffset > _offsetLowerBound) cardArrayOffset--;
        }

        private void CardInit()
        {
            _cardPositions = new Vector3[cards.Length * 2 - 1];
            var lowerBound = cards.GetLowerBound(0);
            var upperBound = cards.GetUpperBound(0);
            _offsetLowerBound = lowerBound - upperBound;
            _offsetUpperBound = 0;

            if (_cardPositions.Length < 2)
            {
                _cardPositions[0] = Vector3.zero;
            }
            else
            {
                // This loop is for cards still in the stack.		
                for (var i = cards.Length; i > -1; i--)
                    if (i < cards.Length - 1)
                        _cardPositions[i] = new Vector3(-cardDistance + _cardPositions[i + 1].x, 0, 0);
                    else
                        _cardPositions[i] = Vector3.zero;

                // This loop is for cards outside of the stack.
                for (var i = cards.Length; i < _cardPositions.Length; i++)
                    _cardPositions[i] = new Vector3(cardDistance + _cardPositions[i - 1].x, 0, 0);
            }
        }
    }
}