using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RTSZombie
{


    public class InputManager_None : StateMachineBehaviour
    {
        [SerializeField] private LayerMask targetLayer;

        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            var panel = RZUIManager.Instance.OpenPanel(PanelEnum.ClickReceiver);
            panel.transform.SetAsFirstSibling();
            RZUIManager.mainCanvasInstance.RegisterDragEvent(OnEndDrag);
        }

        // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
        //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        //{
        //
        //}

        // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            RZUIManager.Instance.ClosePanel(PanelEnum.ClickReceiver);
            RZUIManager.mainCanvasInstance.UnregisterDragEvent();
        }

        // OnStateMove is called right after Animator.OnAnimatorMove()
        //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        //{
        //    // Implement code that processes and affects root motion
        //}

        // OnStateIK is called right after Animator.OnAnimatorIK()
        //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        //{
        //    // Implement code that sets up animation IK (inverse kinematics)
        //}

        private void OnClick(Ray clickRay)
        {
            if (Physics.Raycast(clickRay, out RaycastHit hit, Mathf.Infinity, targetLayer))
            {
                if (hit.collider.GetComponent<RZUnit>().tag == SharedValue.FriendlyTag)
                    RZInputManager.Instance.SelectUnit(hit.collider.GetComponent<RZUnit>());
            }
        }

        private void OnEndDrag(Ray bottomLeftRay, Ray bottomRightRay, Ray topRightRay, Ray topLeftRay)
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
            RZInputManager.Instance.SelectUnits(selectedUnit);
        }

    }



}
