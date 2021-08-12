using Scripts.Utilities;
using UnityEngine;

namespace Scripts.Stacks
{
    public class LabelStack : MonoBehaviour
    {

            [Tooltip("The distance between the topic cards.")] [SerializeField]
            private float topicCardDistance = 0.5f;

            [Tooltip("The speed at which the topic cards move.")] [SerializeField]
            private float cardMoveSpeed = 8f;

            [Tooltip(
                "A collection of topics that make up the topic stack. " +
                "Drag the topic cards belonging to the topic stack into this collection. " +
                "Order matters, topics at element zero will be the last topic in the stack.")]
            [SerializeField]
            private Transform[] cards;

            private int _cardArrayOffset;
            private Vector3[] _cardPositions;
            private UIFader _fader;
            private int _offsetLowerBound;
            private int _offsetUpperBound;
            public GameObject topicStack;

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
                _cardArrayOffset = topicStack.GetComponent<TopicStack>()._cardArrayOffset;
                MoveCards();
            }

            private void MoveCards()
            {
                // This loop moves the cards.
                for (var i = 0; i < cards.Length; i++)
                {
                    cards[i].localPosition = Vector3.Lerp(cards[i].localPosition, _cardPositions[i + 1 + _cardArrayOffset],
                        Time.deltaTime * cardMoveSpeed);
                    if (Mathf.Abs(cards[i].localPosition.y - _cardPositions[i + 1 + _cardArrayOffset].y) < 0.01f)
                    {
                        cards[i].localPosition = _cardPositions[i + 1 + _cardArrayOffset];

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
                if (_cardArrayOffset < _offsetUpperBound) _cardArrayOffset++;
            }

            public void DecreaseOffset()
            {
                if (_cardArrayOffset > _offsetLowerBound) _cardArrayOffset--;
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
