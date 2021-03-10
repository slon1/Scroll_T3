using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Scroll_T3
{
    public class SceneControl : MonoBehaviour
    {

        [SerializeField]
        private int itemsCount;
        [SerializeField]
        private ScrollerControl scroller;
        [SerializeField]
        private Button gotoButton;
        [SerializeField]
        private InputField gotoField;
        
        void Start()
        {
            scroller.Init(FillListDummy(itemsCount));
        }
        List<Item> FillListDummy(int count)
        {
            List<Item> ret = new List<Item>(count);
            for (int i = 0; i < count; i++)
            {
                ret.Add(new Item(i));
            }
            return ret;
        }
        public void Goto()
        {
            int index=-1;
            int.TryParse(gotoField.text, out index);
            if (index >= 0)
            {
                scroller.GoTo(index);
            }
        }
      
    }
}
