using System;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

namespace TP3.Script.PlayerStateMachine
{
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
        public List<GameObject> m_PowerUps;

        public Rigidbody m_RigidBody;
        public Animator m_Animator;
        public Transform m_Transform;

        private bool gamePaused;

        public Camera m_Camera;

        private void Awake()
        {
            LevelManager.SubscribeGameAction += SubscribeAll;
            LevelManager.DisableGameAction += UnsubsribeAll;
        }

        void Start()
        {
            Init();
        }

        private void Init()
        {
            foreach (var item in GameObject.FindGameObjectsWithTag("Lightning"))
            {
                m_PowerUps.Add(item);
            }
            foreach (var item in m_PowerUps)
            {
                item.SetActive(false);
            }

            gamePaused = false;
            m_RigidBody = GetComponent<Rigidbody>();
            m_Animator = GetComponent<Animator>();
            m_Transform = GetComponent<Transform>();

            m_Camera = Camera.main;
            SetState(new Player_Idle(this));
        }

        private void SubscribeAll()
        {
            LevelManager.BeginLevelAction += SetLevelPosition;
            LevelManager.BeginLevelAction += SetStateFromName;
            LevelManager.EndLevelAction += SetTransform;
            LevelManager.PauseAction += StopTime;
            LevelManager.UnPauseAction += StartTime;
        }

        private void UnsubsribeAll()
        {
            foreach (var item in m_PowerUps)
            {
                item.SetActive(true);
            }

            LevelManager.BeginLevelAction -= SetLevelPosition;
            LevelManager.BeginLevelAction -= SetStateFromName;
            LevelManager.EndLevelAction -= SetTransform;
            LevelManager.PauseAction += StopTime;
            LevelManager.UnPauseAction += StartTime;
        }

        public void SetState(PlayerState state)
        {
            _currentState = state;
        }

        void Update()
        {
            _currentState.UpdateExecute();
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                gamePaused = !gamePaused;
                if (gamePaused)
                {
                    LevelManager.PauseAction?.Invoke();
                }
                else
                {
                    LevelManager.UnPauseAction?.Invoke();
                }
            }
        }

        private void FixedUpdate()
        {
            _currentState.FixedUpdateExecute();
        }

        private void SetLevelPosition(LevelScripts _level)
        {
            LevelManager.SetPlayerLastPosition(transform.position, transform.rotation);
            transform.position = _level.PlayerBeginPosition;
        }

        private void StartTime()
        {
            Time.timeScale = 1;
        }

        private void StopTime()
        {
            Time.timeScale = 0;
        }

        private void SetTransform(bool _state)
        {
            transform.position = LevelManager.GetPlayerLastPosition();
            transform.rotation = LevelManager.GetPlayerLastRotation();
            if (LevelManager.GetGameWon()) m_Animator.SetTrigger("Win");
        }

        public void SetStateFromName(LevelScripts _level)
        {
            if (_level.levelName == "Collect The Banana")
            {
                _currentState = new Player_Tree(this);
            }
            else if (_level.levelName == "Lightning Struck")
            {
                _currentState = new Player_PowerUps(this);
            }
        }

        public void StepSound()
        {
            AudioManager.instance.PlaySound(SoundClip.Step, 1.0f);
        }
    }
}