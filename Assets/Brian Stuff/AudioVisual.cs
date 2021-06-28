using UnityEngine;

//DONT KNOW IF SCRIPT WILL BE IN FINAL VERSION OF APP
namespace Brian_Stuff
{
    public class AudioVisual : MonoBehaviour
    {
        public GameObject pos1;
        public GameObject pos2;

        private RectTransform position;

        public float speed;
        // Start is called before the first frame update
        void Start()
        {
            position = gameObject.GetComponent<RectTransform>();

            while (position.position.x < pos2.GetComponent<RectTransform>().position.x)

            {
                position.anchoredPosition += new Vector2(speed * Time.deltaTime, 0f);
            }

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
