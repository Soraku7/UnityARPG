using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UGG.Combat;
using UnityEngine.Rendering.PostProcessing;

public class AICombatSystem : CharacterCombatSystemBase
{
    [SerializeField] private Transform detectionCenter;
    [SerializeField] private float detectionRang;

    [SerializeField] private LayerMask whatisEnemy;
    [SerializeField] private LayerMask whatisObs;

    private Collider[] colliderTargets = new Collider[1];
    private Collider[] detectionedTarget = new Collider[1];

    [SerializeField] private Transform currentTarget;

    //AnimationID
    private int lockOnID = Animator.StringToHash("LockOn");

    [SerializeField] private float animationMoveMult;

    [SerializeField] private List<CombatSkillBase> skills = new List<CombatSkillBase>();

    private void Start()
    {
        InitAllSkill();
    }

    private void Update()
    {
        AIView();
        LockOnTarget();
        UpdateAnimationMove();
        DetectionTarget();
    }

    private void LateUpdate()
    {
        OnAnimatorActionAutoLockON();
    }


    private void AIView()
    {

        int targetCount = Physics.OverlapSphereNonAlloc(detectionCenter.position, detectionRang, colliderTargets, whatisEnemy);
        
        if (targetCount > 0)
        {
            if (!Physics.Raycast((transform.root.position + transform.root.up * 0.5f), (colliderTargets[0].transform.position - transform.root.position).normalized, out var hit, detectionRang, whatisObs))
            {
                if (Vector3.Dot((colliderTargets[0].transform.position - transform.root.position).normalized, transform.root.forward) > 0.35f)
                {
                    currentTarget = colliderTargets[0].transform;
                }
            }
        }
    }

    private void LockOnTarget()
    {
        if (_animator.CheckAnimationTag("Motion") && currentTarget != null)
        {
            _animator.SetFloat(lockOnID, 1f);
            transform.root.rotation = transform.LockOnTarget(currentTarget, transform, 50f);
        }
        else
        {
            _animator.SetFloat(lockOnID, 0f);
        }
    }

    public Transform GetCurrentTarget()
    {
        if(currentTarget == null)
        {
            return null;
        }

        return currentTarget;
    }

    private void UpdateAnimationMove()
    {
        if (_animator.CheckAnimationTag("Roll"))
        {
            _characterMovementBase.CharacterMoveInterface(transform.root.forward, _animator.GetFloat(animationMoveID) * animationMoveMult, true);
        }

        if (_animator.CheckAnimationTag("Attack"))
        {
            _characterMovementBase.CharacterMoveInterface(transform.root.forward, _animator.GetFloat(animationMoveID) * animationMoveMult, true);
        }
    }

    private void OnAnimatorActionAutoLockON()
    {
        if (CanAttackLockOn())
        {
            if (_animator.CheckAnimationTag("Attack") || _animator.CheckAnimationTag("GSAttack"))
            {
                transform.root.rotation = transform.LockOnTarget(currentTarget, transform.root.transform, 50f);
            }
        }
    }

    #region 

    private bool CanAttackLockOn()
    {
        if (_animator.CheckAnimationTag("Attack") || _animator.CheckAnimationTag("GSAttack"))
        {
            if (_animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.75f)
            {
                return true;
            }
        }
        return false;
    }

    private void DetectionTarget()
    {

        int targetCount = Physics.OverlapSphereNonAlloc(detectionCenter.position, detectionRang, detectionedTarget, enemyLayer);


        if (targetCount > 0)
        {
            SetCurrentTarget(detectionedTarget[0].transform);
        }
    }

    private void SetCurrentTarget(Transform target)
    {

        if (currentTarget == null || currentTarget != target)
        {

            currentTarget = target;
        }
    }

    #endregion

    #region 

    private void InitAllSkill()
    {
        if (skills.Count == 0) return;

        for (int i = 0; i < skills.Count; i++)
        {
            skills[i].InitSkill(_animator, this, _characterMovementBase);
            
            if (!skills[i].GetSkillIsDone())
            {
                skills[i].ResetSkill();
            }
        }
    }

    public CombatSkillBase GetAnDoneSkill()
    {
        for (int i = 0; i < skills.Count; i++)
        {
            if (skills[i].GetSkillIsDone()) return skills[i];
            else continue;
        }

        return null;
    }

    public CombatSkillBase GetSkillUseName(string name)
    {
        for (int i = 0; i < skills.Count; i++)
        {
            if (skills[i].GetSkillName().Equals(name)) return skills[i];
            else continue;
        }

        return null;
    }

    public CombatSkillBase GetSkillUseID(int id)
    {
        for (int i = 0; i < skills.Count; i++)
        {
            if (skills[i].GetSkillID() == id) return skills[i];
            else continue;
        }

        return null;
    }

    #endregion
    
    public float GetCurrentTargetDistance() => Vector3.Distance(currentTarget.position, transform.root.position);
    
    public Vector3 GetDirectionForTarget() => (currentTarget.position - transform.root.position).normalized;
}
