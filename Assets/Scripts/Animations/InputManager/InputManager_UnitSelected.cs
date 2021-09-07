using RTSZombie;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager_UnitSelected : StateMachineBehaviour
{
    private List<RZUnit> selectedUnits = new List<RZUnit>();

    [SerializeField] private LayerMask tarrainLayerMask;

    private RZInputManager owner;

    private Animator anim;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        anim = animator;

        owner = animator.GetComponent<RZInputManager>();
        RZUIManager.Instance.OpenHUD(HUDEnum.SelectedUnitDisplayHUD);
        RZUIManager.Instance.OpenHUD(HUDEnum.UnitCommandHUD);

        RZInputManager.Instance.selectedUnits.ForEach(unit => {
            if (unit != null) unit.SetSelected(true);
        });

        owner.onLeftClick = OnLeftClick;
        owner.onRightClick = OnRightClick;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        RZInputManager.Instance.selectedUnits.ForEach(unit => {
            if (unit != null) unit.SetSelected(false);
        });

        RZUIManager.Instance.CloseHUD(HUDEnum.SelectedUnitDisplayHUD);
        RZUIManager.Instance.CloseHUD(HUDEnum.UnitCommandHUD);
        owner.onLeftClick = null;
        owner.onRightClick = null;
    }

    private void OnRightClick(Vector2 mousePosition)
    {
        Ray clickRay = Camera.main.ScreenPointToRay(mousePosition);
        if (Physics.Raycast(clickRay.origin, clickRay.direction, out RaycastHit hit, Mathf.Infinity, tarrainLayerMask))
        {
            RZInputManager.Instance.selectedUnits.ForEach(unit => {
                if (unit != null) unit.CommandMove(hit.point);
            });
        }
    }

    private void OnLeftClick(Vector2 mousePosition)
    {
        Ray clickRay = Camera.main.ScreenPointToRay(mousePosition);
        LayerMask friendlyMask = RZUnitDataContainer.Instance.friendlyLayer;
        LayerMask enemyMask = RZUnitDataContainer.Instance.enemyLayer;
        if (Physics.Raycast(clickRay.origin, clickRay.direction, out RaycastHit friendlyHit, Mathf.Infinity, friendlyMask))
        {
            if (friendlyHit.collider.transform.GetComponent<RZUnit>() != null)
            {
                owner.SelectUnit(friendlyHit.collider.transform.GetComponent<RZUnit>());
                anim.SetTrigger(InputManagerState.UnitSelected.ToString());
                return;
            }
        }
        else if (Physics.Raycast(clickRay.origin, clickRay.direction, out RaycastHit enemyHit, Mathf.Infinity, enemyMask))
        {
            if (enemyHit.collider.transform.GetComponent<RZUnit>() != null)
            {
                RZInputManager.Instance.selectedUnits.ForEach(unit => {
                    if (unit != null) unit.CommandAttack((enemyHit.collider.transform.GetComponent<RZUnit>()));
                });
                return;
            }
        }

        owner.ClearSelectedUnits();
        anim.SetTrigger(InputManagerState.Normal.ToString());
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
}
