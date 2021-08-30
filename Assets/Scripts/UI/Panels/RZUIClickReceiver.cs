using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using RTSZombie;
using UnityEditor;
using System;

namespace RTSZombie.UI
{
    public class RZUIClickReceiver : RZUIPanel, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerClickHandler
    {
        [SerializeField] private Image dragIndicatorPrefab;

        private Image indicatorInstance;

        private Vector2 beginPosition;

        private Vector2 draggingPosition;

        public Action<Ray, Ray, Ray, Ray> onDragEnd;

        public Action<Ray> onClick;

        public void OnBeginDrag(PointerEventData eventData)
        {
            indicatorInstance = Instantiate(dragIndicatorPrefab, RZUIManager.MainCanvas.transform);

            beginPosition = eventData.position;
            indicatorInstance.GetComponent<RectTransform>().sizeDelta = new Vector2(0f, 0f);
            indicatorInstance.GetComponent<RectTransform>().anchoredPosition = new Vector2(beginPosition.x, beginPosition.y);
        }

        public void OnDrag(PointerEventData eventData)
        {
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
            float minX = Mathf.Min(beginPosition.x, draggingPosition.x);
            float maxX = Mathf.Max(beginPosition.x, draggingPosition.x);
            float minY = Mathf.Min(beginPosition.y, draggingPosition.y);
            float maxY = Mathf.Max(beginPosition.y, draggingPosition.y);
            Ray bottomLeftRay = Camera.main.ScreenPointToRay(new Vector2(minX, minY));
            Ray bottomRightRay = Camera.main.ScreenPointToRay(new Vector2(maxX, minY));
            Ray topLeftRay = Camera.main.ScreenPointToRay(new Vector2(minX, maxY));
            Ray topRightRay = Camera.main.ScreenPointToRay(new Vector2(maxX, maxY));

            if(onDragEnd != null)
                onDragEnd.Invoke(bottomLeftRay, bottomRightRay, topRightRay, topLeftRay);

            Destroy(indicatorInstance.gameObject);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            Ray clickRay = Camera.main.ScreenPointToRay(eventData.position);

            if(onClick != null)
                onClick.Invoke(clickRay);
        }
    }

}
