using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform m_Player;
    [SerializeField] private Vector3 m_Offset;
    
    private Vector3 FollowObjectPosition;
    private Vector3 FollowPosition;
    private Vector3 m_Position;
    private float m_PosY;
    
    void Start()
    {
        FollowPosition = m_Player.position;
        FollowObjectPosition = Vector3.zero;
        m_PosY = 0;
        LevelManager.instance.BeginLevelAction += SetCameraFollow;

        LevelManager.instance.EndLevelAction += ResetCameraFollow;
    }

    // Update is called once per frame
    void Update()
    {
        FollowPosition = FollowObjectPosition != Vector3.zero ? FollowObjectPosition : m_Player.position;
        m_Position = FollowPosition - m_Offset;
        m_Position.y = m_PosY - m_Offset.y;
        transform.position = m_Position;
        m_Position += m_Offset;
        transform.LookAt(m_Position);
    }

    public void SetCameraFollow(LevelScripts _level)
    {
        FollowObjectPosition = _level.cameraFocus;
        m_Offset = _level.cameraOffset;
    }

    public void ResetCameraFollow(bool _state)
    {
        FollowObjectPosition = Vector3.zero;
        m_Offset = new Vector3(0, -5, 5);
    }
}