using UnityEngine;

namespace Scripts
{
    [RequireComponent(typeof(CardDragHandler))]
    public class CardStack : MonoBehaviour
    {
        [Tooltip("The distance between the cards.")] [SerializeField]
        private int cardDistance = 1;

        [Tooltip("The speed at which the cards move.")] [SerializeField]
        private float cardMoveSpeed = 8f;

        [Tooltip(
            "A collection of cards that make up the card stack. " +
            "Drag the cards belonging to the card stack into this collection. " +
            "Order matters, cards at element zero will be the last card in the stack.")]
        [SerializeField]
        public Transform[] cards;

        private GameObject _appManager;

        private int _cardArrayOffset;
        private Vector3[] _cardPositions;
        private UIFader _fader;
        private int _offsetLowerBound;
        private int _offsetUpperBound;

        private void Awake()
        {
            _fader = gameObject.GetComponent<UIFader>();
            _appManager = GameObject.Find("AppManager");
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

        private void OnEnable()
        {
            if (_appManager.GetComponent<GameManagerScript>().activeStack != null)
                _appManager.GetComponent<GameManagerScript>().activeStack.SetActive(false);

            _appManager.GetComponent<GameManagerScript>().activeStack = gameObject;
        }

        private void MoveCards()
        {
            // This loop moves the cards.
            for (var i = 0; i < cards.Length; i++)
            {
                cards[i].localPosition = Vector3.Lerp(cards[i].localPosition, _cardPositions[i + 1 + _cardArrayOffset],
                    Time.deltaTime * cardMoveSpeed);
                if (Mathf.Abs(cards[i].localPosition.x - _cardPositions[i + 1 + _cardArrayOffset].x) < 0.01f)
                {
                    cards[i].localPosition = _cardPositions[i + 1 + _cardArrayOffset];

                    var cg = cards[i].gameObject.GetComponent<CanvasGroup>();

                    // Disables interaction with cards that are not on top of the stack and calls the UIFader.
                    if (cards[i].localPosition.x == 0)
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
                        _cardPositions[i] = new Vector3(-cardDistance + _cardPositions[i + 1].x, 0, 0);
                    else
                        _cardPositions[i] = Vector3.zero;

                // This loop is for cards outside of the stack.
                for (var i = cards.Length; i < _cardPositions.Length; i++)
                    _cardPositions[i] = new Vector3(cardDistance + _cardPositions[i - 1].x, 0, 0);
            }
        }

        public int GetOffset()
        {
            return _cardArrayOffset;
        }
    }
}