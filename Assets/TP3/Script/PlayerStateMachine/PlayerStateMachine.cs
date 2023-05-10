using System;
using System.Collections;
using System.Collections.Generic;
using TP3.Script.PlayerStateMachine;
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

    public Camera m_Camera;

    void Start()
    {
        _currentState = new Player_Idle(this);

        m_RigidBody = GetComponent<Rigidbody>();
        m_Animator = GetComponent<Animator>();
        m_Transform = GetComponent<Transform>();
        
        m_Camera = Camera.main;

        LevelManager.instance.BeginLevelAction += SetLevelPosition;
        LevelManager.instance.BeginLevelAction += SetStateFromName;

        LevelManager.instance.EndLevelAction += SetTransform;
    }

    public void SetState(PlayerState state)
    {
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

    private void SetLevelPosition(LevelScripts _level)
    {
        LevelManager.instance.SetPlayerLastPosition(transform);
        transform.position = _level.PlayerBeginPosition;
    }

    private void SetTransform(bool _state)
    {
        transform.position = LevelManager.instance.GetPlayerLastPosition().position;
        transform.rotation = LevelManager.instance.GetPlayerLastPosition().rotation;
    }

    public void SetStateFromName(LevelScripts _level)
    {
        if (_level.levelName == "Collect The Banana")
        {
            _currentState = new Player_Tree(this);
        }
    }
}
