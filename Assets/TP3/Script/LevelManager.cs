using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private static LevelManager levelManager;

    public Action<LevelScripts> descriptionAction;
    public Action<LevelScripts> BeginLevelAction;
    public Action<bool> EndLevelAction;
    public Action<Inventory.ItemType> CollectItemAction;
    public Action PauseAction;
    public Action UnPauseAction;

    [SerializeField] private LevelPool m_LevelList;

    [SerializeField] private GameObject m_DescriptionBox;
    [SerializeField] private GameObject m_InventoryBox;
    [SerializeField] private GameObject m_PauseScreen;

    private Transform playerLastPosition;
    private LevelScripts m_CurrentLevel;

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
        BeginLevelAction += SetCurrentLevel;
        EndLevelAction += EndLevel;
        PauseAction += SetPause;
        UnPauseAction += UnPause;
        PauseAction += SetInventoryActive;
        UnPauseAction += SetInventoryInActive;
        
        m_PauseScreen.SetActive(false);
    }

    public void SetDescriptionBoxActive(bool _state)
    {
        m_DescriptionBox.SetActive(_state);
    }

    public void BeginLevel()
    {
        SetDescriptionBoxActive(false);
        SetInventoryInActive();
    }

    public void EndLevel(bool _state)
    {
        m_CurrentLevel.completed = _state;
        SetInventoryActive();
        SetDescriptionBoxActive(true);
    }

    private void SetInventoryActive()
    {
        m_InventoryBox.SetActive(true);
    }

    private void SetInventoryInActive()
    {
        m_InventoryBox.SetActive(false);
    }

    public Transform GetPlayerLastPosition()
    {
        return playerLastPosition;
    }

    public void SetPlayerLastPosition(Transform _transform)
    {
        playerLastPosition = _transform;
    }

    public void SetCurrentLevel(LevelScripts _level)
    {
        m_CurrentLevel = _level;
    }

    private void SetPause()
    {
        m_PauseScreen.SetActive(true);
    }

    private void UnPause()
    {
        m_PauseScreen.SetActive(false);
    }
}