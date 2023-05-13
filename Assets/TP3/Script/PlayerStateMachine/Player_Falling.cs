using System.Collections;
using System.Collections.Generic;
using TP3.Script.PlayerStateMachine;
using UnityEngine;

public class Player_Falling : PlayerState
{

    public Player_Falling(PlayerStateMachine stateMachine) : base(stateMachine)
    {
        m_Animator.SetBool("Falling", true);
        m_Animator.SetBool("Jumping", false);
    }

    public override void UpdateExecute()
    {
        m_Direction = GetDirection();
    }

    public override void FixedUpdateExecute()
    {
        m_CurrentVelocity = m_RigidBody.velocity;
        m_CurrentRotation = m_Transform.rotation;


        Move();
        if (m_Direction != Vector3.zero) m_Transform.rotation = Rotate();

        Vector3 fallMultiplier = Vector3.up * Physics.gravity.y * (m_FallMultiplier - 1) * Time.deltaTime;
        m_CurrentVelocity += fallMultiplier;
        
        m_RigidBody.velocity = m_CurrentVelocity;

        
        
        if (IsGrounded())
        {
            m_Animator.SetBool("Falling", false);
            m_Animator.SetBool("Jumping", false);
            
            _StateMachine.SetState(new Player_Idle(_StateMachine));
        }
    }
}
