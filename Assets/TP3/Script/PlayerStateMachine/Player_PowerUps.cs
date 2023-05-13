using System.Collections;
using System.Collections.Generic;
using TP3.Script;
using TP3.Script.PlayerStateMachine;
using UnityEngine;

public class Player_PowerUps : PlayerState
{
    private float m_Elapsed;
    private float m_Timer;

    public Player_PowerUps(global::TP3.Script.PlayerStateMachine.PlayerStateMachine stateMachine) : base(stateMachine)
    {
        foreach (var item in m_PowerUps)
        {
            item.SetActive(true);
        }

        m_Elapsed = 10.0f;
        m_Timer = 0;
    }

    public override void UpdateExecute()
    {
        m_Direction = GetDirection();

        if (m_Direction != Vector3.zero) m_Transform.rotation = Rotate();
        m_Elapsed -= Time.deltaTime;
        if (m_Elapsed <= m_Timer)
        {
            LevelManager.EndLevelAction?.Invoke(false);
            _StateMachine.SetState(new Player_Idle(_StateMachine));
        }
    }

    public override void FixedUpdateExecute()
    {
        m_CurrentVelocity = m_RigidBody.velocity;
        m_CurrentRotation = m_Transform.rotation;


        Move();

        m_RigidBody.velocity = m_CurrentVelocity;


        m_Animator.SetBool("Running", m_RigidBody.velocity != UnityEngine.Vector3.zero);


        int collected = 0;
        foreach (var item in m_PowerUps)
        {
            if (!item.activeSelf) collected++;
        }

        if (collected == m_PowerUps.Count)
        {
            LevelManager.CollectItemAction?.Invoke(Inventory.ItemType.Lightning);
            LevelManager.EndLevelAction?.Invoke(true);
            SaveGame.GetInstance().SaveData();
            _StateMachine.SetState(new Player_Idle(_StateMachine));
        }
    }
}