using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NB_Transition", menuName = "StateMachine/Transition/New NB_Transition")]
public class NB_Transition : ScriptableObject
{
    [Serializable]
    private class StateAcitionConfig
    {
        public StateActionSO fromState;
        public StateActionSO toState;
        public List<ConditionSO> conditions;

        public void Init(StateMachineSystem stateMachineSystem)
        {
            fromState.InitState(stateMachineSystem);
            toState.InitState(stateMachineSystem);

            foreach (var item in conditions)
            {
                item.InitCondition(stateMachineSystem);
            }
        }
    }

    

    private Dictionary<StateActionSO, List<StateAcitionConfig>> states = new Dictionary<StateActionSO, List<StateAcitionConfig>>();


    [SerializeField] private List<StateAcitionConfig> configStateData = new List<StateAcitionConfig>();

    private StateMachineSystem stateMachineSystem;


    public void InitTransition(StateMachineSystem stateMachineSystem)
    {
        this.stateMachineSystem = stateMachineSystem;
        SaveAllStateTransitionInfo();
    }

    
    private void SaveAllStateTransitionInfo()
    {
        foreach (var item in configStateData)
        {
            item.Init(stateMachineSystem);
            
            if (!states.ContainsKey(item.fromState))
            {
                states.Add(item.fromState, new List<StateAcitionConfig>());
                states[item.fromState].Add(item);
            }
            else
            {
                states[item.fromState].Add(item);
            }
        }
    }
    
    public void TryGetApplyCondition()
    {
        int conditionPriority = 0;
        int statePriority = 0;
        List<StateActionSO> toStates = new List<StateActionSO>();
        StateActionSO toState = null;


        if (states.ContainsKey(stateMachineSystem.currentState))
        {
            foreach (var stateItem in states[stateMachineSystem.currentState])
            {
                foreach (var conditionItem in stateItem.conditions)
                {
                    if (conditionItem.ConditionSetUp())
                    {
                        if (conditionItem.GetConditionPriority() >= conditionPriority)
                        {
                            conditionPriority = conditionItem.GetConditionPriority();
                            toStates.Add(stateItem.toState);
                        }
                    }
                }
            }
        }
        else
        {
            return;
        }

        if (toStates.Count != 0 || toStates != null)
        {

            foreach (var item in toStates)
            {
                if (item.GetStatePriority() >= statePriority)
                {

                    statePriority = item.GetStatePriority();
                    toState = item;
                }
            }
        }

        if (toState != null)
        {
            stateMachineSystem.currentState.OnExit();
            stateMachineSystem.currentState = toState;
            stateMachineSystem.currentState.OnEnter();
            toStates.Clear();
            conditionPriority = 0;
            statePriority = 0;
            toState = null;
        }
    }



}
