using System.Collections;
using System.Collections.Generic;
using TP3.Script.PlayerStateMachine;
using UnityEngine;
using Vector3 = System.Numerics.Vector3;

public class Player_Idle : PlayerState
{
    public Player_Idle(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void UpdateExecute()
    {
        m_Direction = GetDirection();
        if (m_Direction != UnityEngine.Vector3.zero)
        {
            _StateMachine.SetState(new Player_Moving(_StateMachine));
        }
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _StateMachine.SetState(new Player_Jumping(_StateMachine));
        }
    }

    public override void FixedUpdateExecute()
    {
        if (!m_Animator) return;
        if (m_Animator.GetBool("Running"))
        {
            m_Animator.SetBool("Running", false);
        }

        if (!IsGrounded())
        {
            _StateMachine.SetState(new Player_Falling(_StateMachine));
        }
    }
}