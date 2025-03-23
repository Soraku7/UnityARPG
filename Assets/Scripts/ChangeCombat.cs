using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UGG.Combat;

public class ChangeCombat : StateMachineBehaviour
{
    private AICombatSystem _aiCombatSystem;

    [SerializeField] private float detectionTime;

    [SerializeField] private bool canChangeCombat;
    [SerializeField] private bool allowReleaseChangeCombat;

    [SerializeField] private string changeCombatName;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_aiCombatSystem == null)
        {
            _aiCombatSystem = animator.GetComponent<AICombatSystem>();
        }

        canChangeCombat = true;
        allowReleaseChangeCombat = false;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        canChangeCombat = false;
        allowReleaseChangeCombat = false;
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        CheckChangeCombatTime(animator);
        ChangeCombatAction(animator);
    }

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}

    private void CheckChangeCombatTime(Animator animator)
    {
        if (_aiCombatSystem == null) return;
        if (_aiCombatSystem.GetCurrentTarget() == null) return;


        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < detectionTime)
        {
            canChangeCombat = true;
        }
        else if(animator.GetCurrentAnimatorStateInfo(0).normalizedTime > detectionTime)
        {
            canChangeCombat = false;
        }
    }

    private void ChangeCombatAction(Animator animator)
    {
        if (_aiCombatSystem == null) return;
        if (_aiCombatSystem.GetCurrentTarget() == null) return;


        if (canChangeCombat)
        {
            if (_aiCombatSystem.GetCurrentTargetDistance() < 2.5f)
            {
                //allowReleaseChangeCombat = true;
                animator.CrossFade(changeCombatName, 0f, 0, 0f);
            }
        }


        if (!canChangeCombat && allowReleaseChangeCombat)
        {
            //animator.CrossFade(changeCombatName, 0f, 0, 0f);
        }
    }
}
