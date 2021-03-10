using UnityEngine;
using UnityEngine.UI;

namespace Scroll_T3
{
    public class ItemData : MonoBehaviour
    {
        public Text text;
        public Image image;
        public RectTransform rect;
        private int data;

        public int Item
        {
            get => data;
            set
            {
                data = value;
                text.text = value.ToString();
            }
        }

        private void Awake()
        {
            image.color = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
        }

    }
   
    public class Item
    {
        private int data;

        public Item(int data)
        {
            this.data = data;
        }
        public int Data { get => data; set => data = value; }
    }
}
