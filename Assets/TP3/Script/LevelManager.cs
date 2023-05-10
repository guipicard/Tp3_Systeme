using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{

    private static LevelManager levelManager;

    public Action<LevelScripts> descriptionAction;

    [SerializeField] private LevelPool m_LevelList;

    [SerializeField] private GameObject m_DescriptionBox;

    public static LevelManager instance
    {
        get
        {
            if (!levelManager)
            {
                levelManager = FindObjectOfType(typeof(LevelManager)) as LevelManager;

                if (!levelManager)
                {
                    Debug.LogError("There needs to be one active EventManager script on a GameObject in your scene.");
                }
                else
                {
                    DontDestroyOnLoad(levelManager);
                }
            }

            return levelManager;
        }
    }

    private void Start()
    {
        SetDescriptionBoxActive(false);
    }

    public void SetDescriptionBoxActive(bool _state)
    {
        m_DescriptionBox.SetActive(_state);
    }
}