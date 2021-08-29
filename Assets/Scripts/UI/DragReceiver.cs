using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using RTSZombie;
using UnityEditor;

namespace RTSZombie.UI
{
    public class DragReceiver : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerClickHandler
    {
        [SerializeField] private Image dragIndicatorPrefab;

        [SerializeField] private LayerMask targetLayer;

        private Image indicatorInstance;

        private Vector2 beginPosition;

        private Vector2 draggingPosition;

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

            GameObject[] friendlyUnits = GameObject.FindGameObjectsWithTag(SharedValue.FriendlyTag);


            List<RZUnit> selectedUnit = new List<RZUnit>();
            foreach(var friendlyUnit in friendlyUnits)
            {
                Vector3 cameraToUnit = friendlyUnit.transform.position - Camera.main.transform.position;

                Vector3 bottomLeftRayPoint = bottomLeftRay.GetPoint(Vector3.Dot(bottomLeftRay.direction.normalized, cameraToUnit));
                Vector3 bottomRightRayPoint = bottomRightRay.GetPoint(Vector3.Dot(bottomRightRay.direction.normalized, cameraToUnit));
                Vector3 topLeftRayPoint = topLeftRay.GetPoint(Vector3.Dot(topLeftRay.direction.normalized, cameraToUnit));
                Vector3 topRightRayPoint = topRightRay.GetPoint(Vector3.Dot(topRightRay.direction.normalized, cameraToUnit));

                // Debug
                //RZDebug.DrawRectangle(bottomLeftRayPoint, bottomRightRayPoint, topRightRayPoint, topLeftRayPoint, Color.red, 1f);

                Vector3 bottomEdge = bottomRightRayPoint - bottomLeftRayPoint;
                Vector3 rightEdge = topRightRayPoint - bottomRightRayPoint;
                Vector3 topEdge = topLeftRayPoint - topRightRayPoint;
                Vector3 leftEdge = bottomLeftRayPoint - topLeftRayPoint;


                // 직사각형의 면과 면 시작점에서 유닛까지의 벡터의 Cross의 결과 벡터.
                // 결과 벡터가 카메라 backward와 비슷한 방향(-90, 90)이면 ok
                // 네개의 면에 전부 ok이면 직사각형 안에 있음.

                bool isInside = true;
                // bottom edge
                {
                    Vector3 startToUnit = friendlyUnit.transform.position - bottomLeftRayPoint;
                    Vector3 crossVector = Vector3.Cross(startToUnit, bottomEdge);

                    float dot = Vector3.Dot(crossVector.normalized, -Camera.main.transform.forward);
                    if (dot < 0)
                        isInside = false;
                }

                // right edge
                {
                    Vector3 startToUnit = friendlyUnit.transform.position - bottomRightRayPoint;
                    Vector3 crossVector = Vector3.Cross(startToUnit, rightEdge);

                    float dot = Vector3.Dot(crossVector.normalized, -Camera.main.transform.forward);
                    if (dot < 0)
                        isInside = false;
                }

                // top edge
                {
                    Vector3 startToUnit = friendlyUnit.transform.position - topRightRayPoint;
                    Vector3 crossVector = Vector3.Cross(startToUnit, topEdge);

                    float dot = Vector3.Dot(crossVector.normalized, -Camera.main.transform.forward);
                    if (dot < 0)
                        isInside = false;
                }

                // left edge
                {
                    Vector3 startToUnit = friendlyUnit.transform.position - topLeftRayPoint;
                    Vector3 crossVector = Vector3.Cross(startToUnit, leftEdge);

                    float dot = Vector3.Dot(crossVector.normalized, -Camera.main.transform.forward);
                    if (dot < 0)
                        isInside = false;
                }

                if (isInside)
                {
                    selectedUnit.Add(friendlyUnit.GetComponent<RZUnit>());
                }
            }
            RZInputManager.Instance.SelectUnits(selectedUnit);

            Destroy(indicatorInstance.gameObject);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            Ray clickRay = Camera.main.ScreenPointToRay(eventData.position);

            if(Physics.Raycast(clickRay, out RaycastHit hit, Mathf.Infinity, targetLayer))
            {
                if(hit.collider.GetComponent<RZUnit>().tag == SharedValue.FriendlyTag)
                    RZInputManager.Instance.SelectUnit(hit.collider.GetComponent<RZUnit>());
            }

        }
    }

}
