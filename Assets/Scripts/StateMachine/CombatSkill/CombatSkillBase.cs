using System.Collections;
using System.Collections.Generic;
using UGG.Move;
using UnityEngine;

public abstract class CombatSkillBase : ScriptableObject
{
    [SerializeField] protected string skillName;
    [SerializeField] protected int skillID;
    [SerializeField] protected float skillCDTime;
    [SerializeField] protected float skillUseDistance;
    [SerializeField] protected bool skillIsDone;

    protected Animator animator;
    protected AICombatSystem combat;
    protected CharacterMovementBase movement;

    //AnimationID
    protected int horizontalID = Animator.StringToHash("Horizontal");
    protected int verticalID = Animator.StringToHash("Vertical");
    protected int runID = Animator.StringToHash("Run");
    
    public abstract void InvokeSkill();

    protected void UseSkill()
    {
        animator.Play(skillName, 0, 0f);
        skillIsDone = false;
        ResetSkill();
    }

    public void ResetSkill()
    {

        GameObjectPoolSystem.Instance.TakeGameObject("Timer").GetComponent<Timer>().CreateTime(skillCDTime, () => skillIsDone = true, false);
    }

    #region 

    public void InitSkill(Animator _animator, AICombatSystem _combat, CharacterMovementBase _movement)
    {
        this.animator = _animator;
        this.combat = _combat;
        this.movement = _movement;
    }

    public string GetSkillName() => skillName; 

    public int GetSkillID() => skillID;

    public bool GetSkillIsDone() => skillIsDone;

    public void SetSkillIsDone(bool done) => skillIsDone = done;

    #endregion
}
