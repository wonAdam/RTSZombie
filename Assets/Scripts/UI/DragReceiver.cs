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

        [SerializeField] private LayerMask targetLayer;

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

            //Ray beginPositionRay = Camera.main.ScreenPointToRay(beginPosition);
            //Ray draggingPositionRay = Camera.main.ScreenPointToRay(draggingPosition);

            //float minX = Mathf.Min(beginPosition.x, draggingPosition.x);
            //float maxX = Mathf.Max(beginPosition.x, draggingPosition.x);
            //float minY = Mathf.Min(beginPosition.y, draggingPosition.y);
            //float maxY = Mathf.Max(beginPosition.y, draggingPosition.y);
            //Ray bottomLeftRay = Camera.main.ScreenPointToRay(new Vector2(minX, minY));
            //Ray bottomRightRay = Camera.main.ScreenPointToRay(new Vector2(maxX, minY));
            //Ray topLeftRay = Camera.main.ScreenPointToRay(new Vector2(minX, maxY));
            //Ray topRightRay = Camera.main.ScreenPointToRay(new Vector2(maxX, maxY));

            //Debug.DrawRay(bottomLeftRay.origin, bottomLeftRay.direction, Color.red, 1f);
            //Debug.DrawRay(bottomRightRay.origin, bottomRightRay.direction, Color.red, 1f);
            //Debug.DrawRay(topLeftRay.origin, topLeftRay.direction, Color.red, 1f);
            //Debug.DrawRay(topRightRay.origin, topRightRay.direction, Color.red, 1f);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            RZDebug.Log(this, $"OnEndDrag point: {eventData.position}");

            Vector3 beginPositionInWorld = Camera.main.ScreenToWorldPoint(beginPosition);
            Vector3 draggingPositionInWorld = Camera.main.ScreenToWorldPoint(draggingPosition);
            float minXInWorld = Mathf.Min(beginPositionInWorld.x, draggingPositionInWorld.x);
            float maxXInWorld = Mathf.Max(beginPositionInWorld.x, draggingPositionInWorld.x);
            float minYInWorld = Mathf.Min(beginPositionInWorld.y, draggingPositionInWorld.y);
            float maxYInWorld = Mathf.Max(beginPositionInWorld.y, draggingPositionInWorld.y);
            Vector3 boxExtents = new Vector3(
                Mathf.Abs(beginPositionInWorld.x - draggingPositionInWorld.x),
                Mathf.Abs(beginPositionInWorld.y - draggingPositionInWorld.y),
                1f
            );

            RaycastHit[] hits = Physics.BoxCastAll(
                Camera.main.transform.position,
                boxExtents,
                Camera.main.transform.forward,
                Quaternion.LookRotation(Camera.main.transform.forward, Camera.main.transform.up),
                Mathf.Infinity,
                targetLayer
            );

            if(hits.Length > 0)
            {
                foreach(var hit in hits)
                {
                    RZDebug.Log(this, $"BoxCastAll Hits: {hit.collider.gameObject.name}");
                }
            }

            Destroy(indicatorInstance.gameObject);
        }
    }

}
