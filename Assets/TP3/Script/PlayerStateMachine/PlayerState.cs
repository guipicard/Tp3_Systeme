using System.Collections;
using System.Collections.Generic;
using TMPro;
using TP3.Script.PlayerStateMachine;
using UnityEngine;
using UnityEngine.UI;
using Vector3 = System.Numerics.Vector3;

public abstract class PlayerState
{
    protected PlayerStateMachine _StateMachine;

    private float m_Speed;
    private float m_RotationSpeed;
    protected float m_JumpForce;
    protected float m_FallMultiplier;
    protected float m_lowJumpMultiplier;
    protected float m_MinJumpHeight;
    protected float m_MaxJumpHeight;
    protected List<GameObject> m_PowerUps;

    protected float JumpedFromPositionY;

    protected UnityEngine.Vector3 m_Direction;
    protected UnityEngine.Vector3 m_CurrentVelocity;
    protected UnityEngine.Quaternion m_CurrentRotation;

    protected UnityEngine.Rigidbody m_RigidBody;
    protected UnityEngine.Animator m_Animator;
    protected UnityEngine.Transform m_Transform;

    protected UnityEngine.Ray m_GroundRay;

    protected UnityEngine.RaycastHit m_GroundHit;

    private UnityEngine.Quaternion targetRotation;



    public PlayerState(PlayerStateMachine stateMachine)
    {
        Init(stateMachine);
    }
    
    private void Init(PlayerStateMachine stateMachine)
    {
        _StateMachine = stateMachine;

        LoadSerializables(stateMachine);
        LoadComponents(stateMachine);

        m_GroundRay = new Ray();
        

        m_Direction = UnityEngine.Vector3.zero;


        targetRotation = UnityEngine.Quaternion.identity;
        m_CurrentRotation = UnityEngine.Quaternion.identity;
    }

    public abstract void UpdateExecute();
    public abstract void FixedUpdateExecute();

    private void LoadSerializables(PlayerStateMachine stateMachine)
    {
        m_Speed = stateMachine.m_Speed;
        m_RotationSpeed = stateMachine.m_RotationSpeed;
        m_JumpForce = stateMachine.m_JumpForce;
        m_FallMultiplier = stateMachine.m_FallMultiplier;
        m_lowJumpMultiplier = stateMachine.m_lowJumpMultiplier;
        m_MinJumpHeight = stateMachine.m_MinJumpHeight;
        m_MaxJumpHeight = stateMachine.m_MaxJumpHeight;
        m_PowerUps = stateMachine.m_PowerUps;
    }

    private void LoadComponents(PlayerStateMachine stateMachine)
    {
        m_RigidBody = stateMachine.m_RigidBody;
        m_Animator = stateMachine.m_Animator;
        m_Transform = stateMachine.m_Transform;
    }

    public UnityEngine.Vector3 GetDirection()
    {
        float horizontal = 0f;
        float vertical = 0f;

        if (Input.GetKey(KeyCode.W))
        {
            vertical += 1;
        }

        if (Input.GetKey(KeyCode.A))
        {
            horizontal -= 1;
        }

        if (Input.GetKey(KeyCode.S))
        {
            vertical -= 1;
        }

        if (Input.GetKey(KeyCode.D)) 
        {
            horizontal += 1;
        }

        return new UnityEngine.Vector3(horizontal, 0, vertical).normalized;
    }

    public void Move()
    {
        m_CurrentVelocity.x = m_Direction.x * m_Speed;
        m_CurrentVelocity.z = m_Direction.z * m_Speed;
    }

    protected UnityEngine.Quaternion Rotate()
    {
        targetRotation = UnityEngine.Quaternion.LookRotation(m_Direction, UnityEngine.Vector3.up);

        if (m_Transform.rotation != targetRotation)
        {
            return UnityEngine.Quaternion.Slerp(m_Transform.rotation, targetRotation,
                Time.fixedDeltaTime * m_RotationSpeed);
        }

        return m_Transform.rotation;
    }

    protected bool IsGrounded()
    {
        m_GroundRay = new Ray(m_Transform.position + UnityEngine.Vector3.up, UnityEngine.Vector3.down);
        return Physics.Raycast(m_GroundRay, out m_GroundHit, 1.2f) && m_GroundHit.collider.gameObject.layer == 7;
    }
}
