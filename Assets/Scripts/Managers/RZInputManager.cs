using RTSZombie.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RTSZombie
{
    public class RZInputManager : SingletonBehaviour<RZInputManager>
    {
        [SerializeField] public bool logMousePosition = true;

        [SerializeField] private Animator stateMachine;

        public List<RZUnit> selectedUnits = new List<RZUnit>();

        public int cameraMoveThreshold;

        public MainCamera mainCamera;

        public Action<Vector2> onLeftDragBegin;

        public Action<Vector2> onLeftDrag;

        public Action<Vector2> onLeftDragEnd;

        public Action<Vector2> onRightClick;

        public Action<Vector2> onLeftClick;

        private KeyCode cameraUpKey;

        private KeyCode cameraDownKey;

        private KeyCode cameraRightKey;

        private KeyCode cameraLeftKey;

        public float dragThreshold = 0.5f;

        private RZUIClickReceiver clickReceiver;

        // DO NOT CHANGE
        public enum MouseButton
        { 
            Left = 0,
            Right = 1,
            Middle = 2
        }


        protected override void SingletonAwakened()
        {
        }

        protected override void SingletonStarted()
        {
            RZUIManager.Instance.OpenPanel(PanelEnum.ClickReceiver);
            clickReceiver = RZUIManager.Instance.GetPanel<RZUIClickReceiver>();
            clickReceiver.transform.SetAsFirstSibling();
            clickReceiver.onDragEnd = OnEndDrag_ClickReceiver;
            clickReceiver.onClick = OnClick_ClickReceiver;

            Cursor.lockState = CursorLockMode.Confined;
            mainCamera = Camera.main.gameObject.AddComponent<MainCamera>();
            cameraMoveThreshold = RZGlobalConfig.Instance.cameraMoveThreshold;

            cameraUpKey = RZGlobalConfig.Instance.cameraUpKey;
            cameraDownKey = RZGlobalConfig.Instance.cameraDownKey;
            cameraRightKey = RZGlobalConfig.Instance.cameraRightKey;
            cameraLeftKey = RZGlobalConfig.Instance.cameraLeftKey;
        }

        private void Update()
        {
            if(logMousePosition)
            {
                RZDebug.Log(this, Input.mousePosition.ToString());
            }

            HandleMouseClickNDrag();

            HandleMousePosition();

            HandleKeyBoardInput();
        }

        private void HandleKeyBoardInput()
        {
            if(Input.GetKey(cameraUpKey))
            {
                mainCamera.MoveCamera(Vector2.up);
            }
            if(Input.GetKey(cameraDownKey))
            {
                mainCamera.MoveCamera(Vector2.down);
            }
            if (Input.GetKey(cameraRightKey))
            {
                mainCamera.MoveCamera(Vector2.right);
            }
            if (Input.GetKey(cameraLeftKey))
            {
                mainCamera.MoveCamera(Vector2.left);
            }
        }

        private void HandleMousePosition()
        {
            var view = Camera.main.ScreenToViewportPoint(Input.mousePosition);
            bool isMouseInsideScreen = view.x >= 0 && view.x <= 1 && view.y >= 0 && view.y <= 1;
            if (!isMouseInsideScreen) 
                return;

            Vector2 mouseScreenDirection = Vector3.zero;
            if (Input.mousePosition.x >= Screen.width * ((cameraMoveThreshold - 1f) / cameraMoveThreshold)) // right
            {
                mouseScreenDirection.x = 1f;
            }

            else if (Input.mousePosition.x <= Screen.width * (1f / cameraMoveThreshold)) // left
            {
                mouseScreenDirection.x = -1f;
            }


            if (Input.mousePosition.y >= Screen.height * ((cameraMoveThreshold - 1f) / cameraMoveThreshold)) // up
            {
                mouseScreenDirection.y = 1f;
            }

            else if (Input.mousePosition.y <= Screen.height * (1f / cameraMoveThreshold)) // down
            {
                mouseScreenDirection.y = -1f;
            }

            mainCamera.MoveCamera(mouseScreenDirection);
        }

        private void HandleMouseClickNDrag()
        {
            if (Input.GetMouseButtonDown((int)MouseButton.Left))
            {
                StartCoroutine(OnLeftMouseButtonDown());
            }

            else if (Input.GetMouseButtonDown((int)MouseButton.Right))
            {
                StartCoroutine(OnRightMouseButtonDown());
            }
        }

        protected override void SingletonDestroyed()
        {
            var clickReceiver = RZUIManager.Instance.GetPanel<RZUIClickReceiver>();
            if(clickReceiver != null)
            {
                clickReceiver.onDragEnd = null;
                clickReceiver.onClick = null;
            }
        }

        private void OnEndDrag_ClickReceiver(Ray bottomLeftRay, Ray bottomRightRay, Ray topRightRay, Ray topLeftRay)
        {
            GameObject[] friendlyUnits = GameObject.FindGameObjectsWithTag(SharedValue.FriendlyTag);

            List<RZUnit> selectedUnit = new List<RZUnit>();
            foreach (var friendlyUnit in friendlyUnits)
            {
                Vector3 cameraToUnit = friendlyUnit.transform.position - Camera.main.transform.position;

                Vector3 bottomLeftRayPoint = bottomLeftRay.GetPoint(Vector3.Dot(bottomLeftRay.direction.normalized, cameraToUnit));
                Vector3 bottomRightRayPoint = bottomRightRay.GetPoint(Vector3.Dot(bottomRightRay.direction.normalized, cameraToUnit));
                Vector3 topLeftRayPoint = topLeftRay.GetPoint(Vector3.Dot(topLeftRay.direction.normalized, cameraToUnit));
                Vector3 topRightRayPoint = topRightRay.GetPoint(Vector3.Dot(topRightRay.direction.normalized, cameraToUnit));

                // Debug
                RZDebug.DrawRectangle(bottomLeftRayPoint, bottomRightRayPoint, topRightRayPoint, topLeftRayPoint, Color.red, 1f);

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
            SelectUnits(selectedUnit);
        }

        private void OnClick_ClickReceiver(Ray clickRay)
        {
            if (Physics.Raycast(clickRay, out RaycastHit hit, Mathf.Infinity, RZUnitDataContainer.Instance.friendlyLayer))
            {
                if (hit.collider.GetComponent<RZUnit>().tag == SharedValue.FriendlyTag)
                    SelectUnit(hit.collider.GetComponent<RZUnit>());
            }
        }

        public void ClearSelectedUnits()
        {
            foreach (var unit in selectedUnits)
                unit.SetSelected(false);

            selectedUnits.Clear();
        }

        public void SelectUnits(List<RZUnit> units)
        {
            ClearSelectedUnits();

            selectedUnits = new List<RZUnit>(units);
            selectedUnits.ForEach(unit => unit.SetSelected(true));

            if (selectedUnits.Count > 0)
            {
                stateMachine.SetTrigger(InputManagerState.UnitSelected.ToString());
            }
        }

        public void SelectUnit(RZUnit unit)
        {
            ClearSelectedUnits();
            
            selectedUnits.Add(unit);
            unit.SetSelected(true);
            if (selectedUnits.Count > 0)
            {
                stateMachine.SetTrigger(InputManagerState.UnitSelected.ToString());
            }
        }

        private IEnumerator OnLeftMouseButtonDown()
        {
            RZDebug.Log(this, "OnLeftMouseButtonDown Enter");

            float timer = 0f;
            bool dragStart = false;
            while (!dragStart && !Input.GetMouseButtonUp((int)MouseButton.Left))
            {
                yield return null;
                timer += Time.deltaTime;

                if (timer >= dragThreshold)
                {
                    dragStart = true;
                    StartCoroutine(OnLeftDragBegin());
                }

                if (clickReceiver.isDragging)
                    dragStart = true;
            }

            RZDebug.Log(this, "OnLeftMouseButtonDown Exit");

            if (!dragStart)
                onLeftClick?.Invoke(Input.mousePosition);
        }

        private IEnumerator OnRightMouseButtonDown()
        {
            RZDebug.Log(this, "OnRightMouseButtonDown Enter");

            float timer = 0f;
            bool dragStart = false;
            while (!dragStart && !Input.GetMouseButtonUp((int)MouseButton.Right))
            {
                yield return null;
                timer += Time.deltaTime;

                if (timer >= dragThreshold)
                {
                    dragStart = true;
                }

                if (clickReceiver.isDragging)
                    dragStart = true;
            }

            RZDebug.Log(this, "OnRightMouseButtonDown Exit");

            if (!dragStart)
                onRightClick?.Invoke(Input.mousePosition);
        }

        private IEnumerator OnLeftDragBegin()
        {
            RZDebug.Log(this, "OnLeftDragBegin Enter");

            if (onLeftDragBegin != null)
                onLeftDragBegin.Invoke(Input.mousePosition);

            while (!Input.GetMouseButtonUp((int)MouseButton.Left))
            {
                yield return null;

                if (onLeftDrag != null)
                    onLeftDrag.Invoke(Input.mousePosition);
            }

            yield return null;
            RZDebug.Log(this, "OnLeftDragBegin Exit");

            if (onLeftDragEnd != null)
                onLeftDragEnd.Invoke(Input.mousePosition);
        }
    }
}
