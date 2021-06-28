using UnityEngine;

namespace Brian_Stuff
{
    public class CardStackTest : MonoBehaviour
    {
        [SerializeField] private float cardMoveSpeed = 8f; //the speed at which the cards move at when swiped
        [SerializeField] private int cardZMultiplier = 32; //the variable used in the init function to set the z position of the cards
        [SerializeField] private bool useDefaultUsedXPos = true; //the default x pos to be used in setting the card locations
        [SerializeField] private int usedCardXPos = 1280; //the x position of any cards that have already been used or are out of the array
        [SerializeField] public Transform[] cards; //an array holding all of the cards as transforms

        public int _cardArrayOffset; //the offset number to determine which slide you are currently on
        private Vector3[] _cardPositions; //an array containing all of the card positions as 3d vectors
        private int _lower; //the lower bound of the cards array
        private int _upper; //the upper bound of the cards array
        private int _xPowerDifference; //another variable used in determining the initial position of the cards

        private void Awake()
        {
            _lower = cards.GetLowerBound(0); //set the lower bound
            _upper = cards.GetUpperBound(0); //set the upper bound
            _xPowerDifference = 9 - cards.Length; //set the x power difference to nine minus whatever the amount of cards is
        }

        private void Start()
        {
            if (useDefaultUsedXPos) //if useDefaultUsedXPos is true
            {
                var cardWidth = (int) cards[0].GetComponent<RectTransform>().rect.width; //declare the cardWidth variable and set it to the width of the first card
                usedCardXPos = (int) (Screen.width * 0.5f + cardWidth); //set usedCardXPos to half the screen width plus the cardWidth
            }

            CardInit(); //run the CardInit function
        }

        private void Update()
        {
            MoveCards(); //run the MoveCards function
        }

        private void MoveCards()
        {
            // This loop moves the cards.
            for (var i = cards.Length - 1; i >= 0; i--) //for every downwards iteration i from the amount of cards (length of cards array) to 0
            {
                if (i != cards.Length - 1 && _cardArrayOffset > i) //if i is not equal to the length of the cards array minus 1 AND the cardArrayOffset is greater than i 
                {
                    cards[i].localPosition = Vector3.Lerp(cards[i].localPosition, _cardPositions[cards.Length - 1 + _cardArrayOffset],
                        Time.deltaTime * cardMoveSpeed); //set the position of the card at index i to a new lerp Vector between points a and b at t speed
                    if (!(Mathf.Abs(cards[i].localPosition.x - _cardPositions[i + _cardArrayOffset].x) < 0.01f)) continue; //if the negative reciprocal of the position of card i minus the set card position is less than 0.01, continue the last line
                    cards[i].localPosition = _cardPositions[i + _cardArrayOffset]; //set the position of card i to the new local position
                
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
            if (_cardArrayOffset < _upper) _cardArrayOffset++; //if the offset is less than the upper bound, then increase the offset by 1
        }

        public void DecreaseOffset()
        {
            if (_cardArrayOffset > _lower) _cardArrayOffset--; //if the offset is greater than the lower bound, then decrease the offset by 1
        }

        private void CardInit()
        {
            _cardPositions = new Vector3[cards.Length * 2 - 1]; //set cardPositions equal to a new 3d vector that is double the length of the cards array minus 1

            // This loop is for cards still in the stack.		
            for (var i = 0; i < cards.Length; i++) //for every increasing iteration i from 0 to the length of the cards array
                if (i != 0) //if i does not equal zero
                    _cardPositions[i] = new Vector3(-Mathf.Pow(2, i + _xPowerDifference) + _cardPositions[i + 1].x, 0,
                        cardZMultiplier * Mathf.Abs(i + 1 - cards.Length)); //set the position of card i to the vector utilizing this calculation
                else
                    _cardPositions[i] = Vector3.zero; // set the position of card i to vector 0,0,0

            // This loop is for cards outside of the stack.
            for (var i = cards.Length; i < _cardPositions.Length; i++) //for every increasing iteration i from the length of the cards array to the length of the positions array
                _cardPositions[i] = new Vector3(usedCardXPos + 4 * (i - cards.Length), 0, -2 + -2 * (i - cards.Length)); //set position i  to a new vector using the formula
        }
    }
}
