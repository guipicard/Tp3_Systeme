using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine : MonoBehaviour
{
    PlayerState _currentState;

    [SerializeField] public float m_Speed;
    [SerializeField] public float m_RotationSpeed;
    [SerializeField] public float m_JumpForce;
    [SerializeField] public float m_FallMultiplier;
    [SerializeField] public float m_lowJumpMultiplier;
    [SerializeField] public float m_MinJumpHeight;
    [SerializeField] public float m_MaxJumpHeight;
    
    public Rigidbody m_RigidBody;
    public Animator m_Animator;
    public Transform m_Transform;

    void Start()
    {
        _currentState = new Player_Idle(this);

        m_RigidBody = GetComponent<Rigidbody>();
        m_Animator = GetComponent<Animator>();
        m_Transform = GetComponent<Transform>();
    }

    public void SetState(PlayerState state)
    {
        Debug.Log($"{state}");
        _currentState = state;
    }
    
    void Update()
    {
        _currentState.UpdateExecute();
    }

    private void FixedUpdate()
    {
        _currentState.FixedUpdateExecute();
    }

    
}
