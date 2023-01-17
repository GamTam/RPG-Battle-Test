using System;
using System.Collections;
using System.Collections.Generic;
using Battle.State_Machine;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    private State _state;
    private List<Battleable> _fighters;

    private void Awake()
    {
        SwitchStates(new Opening(this));
    }

    public void Click()
    {
        StartCoroutine(_state.OnClick());
    }

    public void SwitchStates(State state)
    {
        _state = state;
        StartCoroutine(state.EnterState());
    }
}

public enum DamageTypes
{
    HP,
    MP,
    Money
}