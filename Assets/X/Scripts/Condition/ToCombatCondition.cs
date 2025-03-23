using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ToCombatCondition", menuName = "StateMachine/Condition/ToCombatCondition")]
public class ToCombatCondition : ConditionSO
{
    public override bool ConditionSetUp()
    {
        //如果当前目标不等于空返回true 否则返回false
        return (_combatSystem.GetCurrentTarget() != null) ? true : false;
    }
}
