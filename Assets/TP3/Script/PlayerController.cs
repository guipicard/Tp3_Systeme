using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Numerics;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float m_Speed;
    [SerializeField] private float m_RotationSpeed;
    [SerializeField] private float m_JumpForce;
    [SerializeField] private float m_FallMultiplier;
    [SerializeField] private float m_lowJumpMultiplier;
    [SerializeField] private float m_MinJumpHeight;
    [SerializeField] private float m_MaxJumpHeight;

    private float JumpedFromPositionY;

    private Vector3 m_Direction;
    private Vector3 m_CurrentVelocity;

    private Rigidbody m_RigidBody;
    private Animator m_Animator;

    private Ray m_GroundRay;
    private RaycastHit m_GroundHit;

    private Quaternion targetRotation;

    private bool m_Ground;

    private static readonly int Running = Animator.StringToHash("Running");
    private static readonly int Jumping = Animator.StringToHash("Jumping");
    private static readonly int JumpId = Animator.StringToHash("Jump");
    private static readonly int Falling = Animator.StringToHash("Falling");

    void Start()
    {
        m_RigidBody = GetComponent<Rigidbody>();
        m_Animator = GetComponent<Animator>();

        m_GroundRay = new Ray();
        m_Direction = Vector3.zero;

        targetRotation = Quaternion.identity;

        m_Ground = true;
    }

    void Update()
    {
        GetDirection();
        Jump();
    }

    private void FixedUpdate()
    {
        m_CurrentVelocity = m_RigidBody.velocity;
        IsGrounded();
        // if (m_Ground && m_RigidBody.velocity.y < 0) m_CurrentVelocity.y = 0;
        if (m_Direction != Vector3.zero)
        {
            Move();
            Rotate();
        }
        else
        {
            m_CurrentVelocity.x = 0;
            m_CurrentVelocity.z = 0;
        }
        JumpMultiplier();
        m_RigidBody.velocity = m_CurrentVelocity;
        Animate();
    }

    private void GetDirection()
    {
        // floats instead of vector3 to simplify the code, each inputs key adds its direction to a vector3.zero
        float horizontal = 0f;
        float vertical = 0f;

        if (Input.GetKey(KeyCode.W)) // Vector3(1, 0, 1)
        {
            horizontal += 1;
            vertical += 1;
        }

        if (Input.GetKey(KeyCode.A)) // Vector3(-1, 0, 1)
        {
            horizontal -= 1;
            vertical += 1;
        }

        if (Input.GetKey(KeyCode.S)) // Vector3(-1, 0, -1)
        {
            horizontal -= 1;
            vertical -= 1;
        }

        if (Input.GetKey(KeyCode.D)) // Vector3(1, 0, -1)
        {
            horizontal += 1;
            vertical -= 1;
        }

        m_Direction = new Vector3(horizontal, 0, vertical).normalized;
    }

    private void IsGrounded()
    {
        m_GroundRay = new Ray(transform.position + Vector3.up, Vector3.down);
        m_Ground = Physics.Raycast(m_GroundRay, out m_GroundHit, 1.2f) && m_GroundHit.collider.gameObject.layer == 7;
    }

    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && m_Ground)
        {
            m_RigidBody.AddForce(Vector3.up * m_JumpForce, ForceMode.Impulse);
            JumpedFromPositionY = transform.position.y;
            m_Animator.SetTrigger(JumpId);
        }
    }

    private void JumpMultiplier()
    {
        Vector3 fallMultiplier = Vector3.up * Physics.gravity.y * (m_FallMultiplier - 1) * Time.deltaTime;
        Vector3 jumpMultiplier = Vector3.up * Physics.gravity.y * (m_lowJumpMultiplier - 1) * Time.deltaTime;
        if (m_RigidBody.velocity.y < 0)
        {
            m_CurrentVelocity += fallMultiplier;
        }
        else if (m_RigidBody.velocity.y > 0)
        {
            if (!Input.GetKey(KeyCode.Space))
            {
                m_CurrentVelocity+= jumpMultiplier;
                if (transform.position.y > JumpedFromPositionY + m_MinJumpHeight)
                {
                    m_CurrentVelocity.y = 0;
                }
            }
            else if (transform.position.y > JumpedFromPositionY + m_MaxJumpHeight)
            {
                m_CurrentVelocity.y = 0;
            }
        }
    }

    private void Move()
    {
        m_CurrentVelocity.x = m_Direction.x * m_Speed;
        m_CurrentVelocity.z = m_Direction.z * m_Speed;
    }

    private void Rotate()
    {
        if (m_Direction != Vector3.zero) targetRotation = Quaternion.LookRotation(m_Direction, Vector3.up);

        if (transform.rotation != targetRotation)
        {
            // Slerp looks smoother than Lerp
            transform.rotation = Quaternion.Slerp(m_RigidBody.rotation, targetRotation, Time.fixedDeltaTime * m_RotationSpeed);
        }
    }

    private void Animate()
    {
        // run
        m_Animator.SetBool(Running, m_Direction != Vector3.zero);
        // jump
        m_Animator.SetBool(Jumping, !m_Ground);
        m_Animator.SetBool(Falling, m_RigidBody.velocity.y < 0);
    }


    


    // private void OnTriggerEnter(Collider other)
    // {
    //
    // }
    //
    // private void Attack()
    // {
    //     
    // }
}