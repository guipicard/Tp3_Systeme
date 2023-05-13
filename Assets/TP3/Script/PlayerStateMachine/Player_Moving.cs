using System.Collections;
using System.Collections.Generic;
using TP3.Script.PlayerStateMachine;
using UnityEngine;
using UnityEngine.EventSystems;

public class Player_Moving : PlayerState
{
    // private float CurrentVelocity;
    public Player_Moving(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void UpdateExecute()
    {
        m_Direction = GetDirection();
        if (m_Direction == UnityEngine.Vector3.zero)
        {
            _StateMachine.SetState(new Player_Idle(_StateMachine));
        }
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _StateMachine.SetState(new Player_Jumping(_StateMachine));
        }
    }

    public override void FixedUpdateExecute()
    {
        if (!m_Animator.GetBool("Running"))
        {
            m_Animator.SetBool("Running", true);
        }
        m_CurrentVelocity = m_RigidBody.velocity;
        // IsGrounded();

        Move();
        

        // JumpMultiplier();
        m_RigidBody.velocity = m_CurrentVelocity;
        m_Transform.rotation = Rotate();
        // Animate();
        
        if (!IsGrounded())
        {
            _StateMachine.SetState(new Player_Falling(_StateMachine));
        }
    }
}