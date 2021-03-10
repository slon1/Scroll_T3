using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Scroll_T3
{
    public class ScrollerControl : MonoBehaviour
    {
        [SerializeField]
        private ScrollRect scroll;
        [SerializeField]
        private RectTransform content;
        [SerializeField]
        private int bufferSize = 5;
        [SerializeField]
        private int preRenderSize = 1;
        [SerializeField]
        private GameObject prefab;

        private int count;
        private float dragLast;
        private float itemHeight;
        private int indexOnTop;
        private RectTransform drager;
        private List<Item> items;
        private bool needRefresh = false;
        private Dictionary<Transform, ItemData> scriptsDict;

        public void Init(List<Item> userdata)
        {
            count = userdata.Count;
            items = userdata;
            drager = scroll.content;
            itemHeight = Mathf.FloorToInt(scroll.viewport.rect.height / bufferSize);
            scroll.onValueChanged.AddListener((x) => OnValueChanged(x));
            scroll.content.sizeDelta = new Vector2(scroll.content.sizeDelta.x, count * itemHeight);
            dragLast = drager.anchoredPosition.y;
            indexOnTop = 0;
            scriptsDict = new Dictionary<Transform, ItemData>(bufferSize + preRenderSize);

            for (int i = 0; i < (bufferSize + preRenderSize); i++)
            {
                GameObject go = Instantiate(prefab, content);
                go.name = i.ToString();
                ItemData data = go.GetComponent<ItemData>();
                data.Item = items[i].Data;
                data.rect.sizeDelta = new Vector2(content.rect.width, itemHeight);
                scriptsDict.Add(go.transform, data);
            }
        }

        private void ReorderChilds(bool dir, int index)
        {
            if ((index + bufferSize) >= items.Count)
            { return; }

            if (!dir)
            {
                scriptsDict[content.GetChild(0)].Item = items[index + bufferSize].Data;
                content.GetChild(0).SetAsLastSibling();
            }
            else
            {
                scriptsDict[content.GetChild(content.childCount - 1)].Item = items[index].Data;
                content.GetChild(content.childCount - 1).SetAsFirstSibling();
            }

        }
        private void RefreshChilds(int n)
        {
            for (int i = 0; i < content.childCount; i++)
            {
                scriptsDict[content.GetChild(i)].Item = items[n + i].Data;
            }
            needRefresh = false;
        }
        public void GoTo(int n)
        {
            if (n == indexOnTop)
            { return; }

            n = Mathf.Clamp(n, 0, items.Count - bufferSize);
            drager.anchoredPosition = new Vector2(drager.anchoredPosition.x, (itemHeight * n));
            scroll.velocity = Vector2.zero;
            needRefresh = true;
        }

        private void OnValueChanged(Vector2 vec)
        {
            float dragDelta = drager.anchoredPosition.y - dragLast;
            SetContentPosition(content.anchoredPosition.y + dragDelta);
            int indexLast = indexOnTop;
            indexOnTop = Mathf.Clamp(Mathf.FloorToInt(drager.anchoredPosition.y / itemHeight), 0, count - bufferSize - preRenderSize);
            if (needRefresh)
            {
                SetContentPosition(vec.y == 0 ? itemHeight : 0);
                RefreshChilds(indexOnTop);
            }
            else if (indexLast != indexOnTop)
            {
                SetContentPosition(indexOnTop < indexLast ? itemHeight : 0);
                ReorderChilds(indexOnTop < indexLast, indexOnTop);
            }
            dragLast = drager.anchoredPosition.y;
        }
        private void SetContentPosition(float newPositionY)
        {
            content.anchoredPosition = new Vector2(content.anchoredPosition.x, newPositionY);
        }

        private void OnDestroy()
        {
            items.Clear();
            scriptsDict.Clear();
            items = null;
            scriptsDict = null;
        }
    }
}