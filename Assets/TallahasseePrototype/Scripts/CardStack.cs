using UnityEngine;
using UnityEngine.EventSystems;

namespace TallahasseePrototype.Scripts
{
    public class CardStack : MonoBehaviour, IDragHandler, IEndDragHandler
    {
        [SerializeField] private float cardMoveSpeed = 8f;
        [SerializeField] private int cardZMultiplier = 32;
        [SerializeField] private bool useDefaultUsedXPos = true;
        [SerializeField] private int usedCardXPos = 1280;
        [SerializeField] private Transform[] cards;

        private int _cardArrayOffset;
        private Vector3[] _cardPositions;
        private int _xPowerDifference;

        private void Start()
        {
            _xPowerDifference = 9 - cards.Length;

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

        // must implement or IEndDragHandler will not work
        public void OnDrag(PointerEventData eventData)
        {
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            Vector3 dragVectorDirection = (eventData.position - eventData.pressPosition).normalized;
            GetDragDirection(dragVectorDirection);
            if (GetDragDirection(dragVectorDirection) == DraggedDirection.Left)
                _cardArrayOffset--;
            else if (GetDragDirection(dragVectorDirection) == DraggedDirection.Right) _cardArrayOffset++;
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

                // This disables interaction with cards that are not on top of the stack.
                cards[i].GetComponent<CanvasGroup>().interactable = Mathf.Approximately(0f, cards[i].localPosition.x);
            }
        }

        private void CardInit()
        {
            _cardPositions = new Vector3[cards.Length * 2 - 1];

            // This loop is for cards still in the stack.		
            for (var i = cards.Length; i > -1; i--)
                if (i < cards.Length - 1)
                    _cardPositions[i] = new Vector3(-Mathf.Pow(2, i + _xPowerDifference) + _cardPositions[i + 1].x, 0,
                        cardZMultiplier * Mathf.Abs(i + 1 - cards.Length));
                else
                    _cardPositions[i] = Vector3.zero;

            // This loop is for cards outside of the stack.
            for (var i = cards.Length; i < _cardPositions.Length; i++)
                _cardPositions[i] = new Vector3(usedCardXPos + 4 * (i - cards.Length), 0, -2 + -2 * (i - cards.Length));
        }

        private static DraggedDirection GetDragDirection(Vector3 dragVector)
        {
            var positiveX = Mathf.Abs(dragVector.x);
            var positiveY = Mathf.Abs(dragVector.y);
            DraggedDirection draggedDir;
            if (positiveX > positiveY)
                draggedDir = dragVector.x > 0 ? DraggedDirection.Right : DraggedDirection.Left;
            else
                draggedDir = dragVector.y > 0 ? DraggedDirection.Up : DraggedDirection.Down;

            return draggedDir;
        }

        private enum DraggedDirection
        {
            Up,
            Down,
            Right,
            Left
        }
    }
}