using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform m_Player;
    [SerializeField] private Vector3 m_Offset;

    private Vector3 m_Position;
    private float m_PosY;
    
    void Start()
    {
        m_Position = m_Player.position;
        m_PosY = 0;
    }

    // Update is called once per frame
    void Update()
    {
        m_Position = m_Player.position - m_Offset;
        m_Position.y = m_PosY - m_Offset.y;
        transform.position = m_Position;
        m_Position += m_Offset;
        transform.LookAt(m_Position);
    }
}