using Scripts.Drag_Controllers;
using Scripts.Utilities;
using UnityEngine;

namespace Scripts.Stacks
{
    [RequireComponent(typeof(TopicDragHandler))]
    public class TopicStack : MonoBehaviour
    {
        [Tooltip("The distance between the topic cards.")] [SerializeField]
        private int topicCardDistance = 1;

        [Tooltip("The speed at which the topic cards move.")] [SerializeField]
        private float cardMoveSpeed = 8f;

        [Tooltip(
            "A collection of topics that make up the topic stack. " +
            "Drag the topic cards belonging to the topic stack into this collection. " +
            "Order matters, topics at element zero will be the last topic in the stack.")]
        [SerializeField]
        private Transform[] cards;

        public int cardArrayOffset;
        private Vector3[] _cardPositions;
        private UIFader _fader;
        private int _offsetLowerBound;
        private int _offsetUpperBound;

        private int stackScale;

        private void Awake()
        {
            _fader = gameObject.GetComponent<UIFader>();
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
                cards[i].localPosition = Vector3.Lerp(cards[i].localPosition,
                    _cardPositions[i + stackScale + cardArrayOffset],
                    Time.deltaTime * cardMoveSpeed);
                if (Mathf.Abs(cards[i].localPosition.y - _cardPositions[i + stackScale + cardArrayOffset].y) < 0.01f)
                {
                    cards[i].localPosition = _cardPositions[i + stackScale + cardArrayOffset];

                    var cg = cards[i].gameObject.GetComponent<CanvasGroup>();

                    // This disables interaction with cards that are not on top of the stack.
                    if (cards[i].localPosition.y == 0)
                    {
                        cg.interactable = true;
                        _fader.FadeIn(cg);
                    }
                    else
                    {
                        cg.interactable = false;
                        _fader.FadeToQuarter(cg);
                    }
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

            stackScale = cards.Length - 1;

            if (_cardPositions.Length < 2)
            {
                _cardPositions[0] = Vector3.zero;
            }
            else
            {
                // This loop is for cards still in the stack.		
                for (var i = cards.Length; i > -1; i--)
                    if (i < cards.Length - 1)
                        _cardPositions[i] =
                            new Vector3(0, -topicCardDistance + _cardPositions[i + 1].y, 0);
                    else
                        _cardPositions[i] = Vector3.zero;

                // This loop is for cards outside of the stack.
                for (var i = cards.Length; i < _cardPositions.Length; i++)
                    _cardPositions[i] = new Vector3(0, topicCardDistance + _cardPositions[i - 1].y, 0);
            }
        }
    }
}