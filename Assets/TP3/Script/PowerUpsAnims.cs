using System;
using System.Collections;
using System.Collections.Generic;
using TP3.Script;
using UnityEngine;

public class PowerUpsAnims : MonoBehaviour
{
    [SerializeField] private float m_RotationTime;
    [SerializeField] private float m_VerticalTranslationTime;
    [SerializeField] private float m_VerticalTranslationTravel;
    private float m_Elapsed;
    private bool m_GoesUp;
    private Vector3 m_InitialPosition;
    private Vector3 m_HigherPosition;

    private int m_Collected;
    void Start()
    {
        m_Elapsed = 0.0f;
        m_GoesUp = true;
        m_InitialPosition = transform.position;
        m_HigherPosition = m_InitialPosition + new Vector3(0, m_VerticalTranslationTravel, 0);
    }

    void Update()
    {
        m_Elapsed = m_GoesUp ? m_Elapsed + Time.deltaTime : m_Elapsed - Time.deltaTime;
        if (m_Elapsed > m_VerticalTranslationTime || m_Elapsed < 0)
        {
            m_GoesUp = !m_GoesUp;
            m_Elapsed = m_GoesUp ? 0 : m_VerticalTranslationTime;
        }
        transform.position = Vector3.Lerp(m_InitialPosition + new Vector3(0, m_VerticalTranslationTravel, 0), m_InitialPosition, Mathf.Pow(m_Elapsed, 2) / m_VerticalTranslationTime);

        transform.eulerAngles += new Vector3(0, Time.deltaTime * (360 / m_RotationTime), 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            AudioManager.instance.PlaySound(SoundClip.Pickup, 1.0f);
            gameObject.SetActive(false);
        }
    }
}
