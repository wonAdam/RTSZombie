using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using RTSZombie;

namespace RTSZombie.UI
{
    public class DragReceiver : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        [SerializeField] private Image dragIndicatorPrefab;

        private Image indicatorInstance;

        private Vector2 beginPosition;

        private Vector2 draggingPosition;

        public void OnBeginDrag(PointerEventData eventData)
        {
            RZDebug.Log(this, $"OnBeginDrag point: {eventData.position}");
            indicatorInstance = Instantiate(dragIndicatorPrefab, RZUIManager.MainCanvas.transform);

            beginPosition = eventData.position;
            indicatorInstance.GetComponent<RectTransform>().sizeDelta = new Vector2(0f, 0f);
            indicatorInstance.GetComponent<RectTransform>().anchoredPosition = new Vector2(beginPosition.x, beginPosition.y);
        }

        public void OnDrag(PointerEventData eventData)
        {
            RZDebug.Log(this, $"OnDrag point: {eventData.position}");

            draggingPosition = eventData.position;
            float width = Mathf.Abs(beginPosition.x - draggingPosition.x);
            float height = Mathf.Abs(beginPosition.y - draggingPosition.y);

            indicatorInstance.GetComponent<RectTransform>().sizeDelta = new Vector2(width, height);
            indicatorInstance.GetComponent<RectTransform>().anchoredPosition = new Vector2(
                Mathf.Min(beginPosition.x, draggingPosition.x), Mathf.Min(beginPosition.y, draggingPosition.y)
            );
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            RZDebug.Log(this, $"OnEndDrag point: {eventData.position}");

            Destroy(indicatorInstance.gameObject);
        }
    }

}
