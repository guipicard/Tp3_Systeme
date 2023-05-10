using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Jumping : PlayerState
{
    public Player_Jumping(PlayerStateMachine stateMachine) : base(stateMachine)
    {
        m_RigidBody.AddForce(Vector3.up * m_JumpForce, ForceMode.Impulse);
        JumpedFromPositionY = m_Transform.position.y;
        m_Animator.SetBool("Running", false);
        m_Animator.SetBool("Jumping", true);
        m_Animator.SetBool("Falling", false);
        m_Animator.SetTrigger("Jump");
    }

    public override void UpdateExecute()
    {
        m_Direction = GetDirection();
        if (m_RigidBody.velocity.y < 0)
        {
            _StateMachine.SetState(new Player_Falling(_StateMachine));
        }
    }

    public override void FixedUpdateExecute()
    {
        m_CurrentVelocity = m_RigidBody.velocity;
        m_CurrentRotation = m_RigidBody.rotation;


        Move();
        m_CurrentRotation = Rotate();

        JumpMultiplier();
        m_RigidBody.velocity = m_CurrentVelocity;
        m_Transform.rotation = m_CurrentRotation;
    }

    private void JumpMultiplier()
    {
        Vector3 jumpMultiplier = Vector3.up * Physics.gravity.y * (m_lowJumpMultiplier - 1) * Time.deltaTime;
        if (m_RigidBody.velocity.y > 0)
        {
            if (!Input.GetKey(KeyCode.Space))
            {
                m_CurrentVelocity += jumpMultiplier;
                if (m_Transform.position.y > JumpedFromPositionY + m_MinJumpHeight)
                {
                    m_CurrentVelocity.y = 0;
                }
            }
            else if (m_Transform.position.y > JumpedFromPositionY + m_MaxJumpHeight)
            {
                m_CurrentVelocity.y = 0;
            }
        }
    }
}