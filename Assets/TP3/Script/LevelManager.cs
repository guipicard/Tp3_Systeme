using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Vector3 = System.Numerics.Vector3;

namespace TP3.Script
{
    public class LevelManager
    {
        private static LevelManager levelManager;

        public static Action InitialiseGameAction;

        public static Action SubscribeGameAction;
        public static Action DisableGameAction;

        public static Action<LevelScripts> descriptionAction;
        public static Action<LevelScripts> BeginLevelAction;
        public static Action<bool> EndLevelAction;
        public static Action<Inventory.ItemType> CollectItemAction;
        public static Action PauseAction;
        public static Action UnPauseAction;


        public static GameObject m_DescriptionBox;
        public static GameObject m_InventoryBox;
        public static GameObject m_PauseScreen;
        public static GameObject m_EndButton;

        private static UnityEngine.Vector3 playerLastPosition;
        private static UnityEngine.Quaternion playerLastRotation;
        private static LevelScripts m_CurrentLevel;

        public static List<Item> m_Collection;

        public static SaveGame Save;

        private static bool m_GameWon;

        public static LevelManager Instance
        {
            get
            {
                if (levelManager == null)
                {
                    levelManager = new LevelManager();
                }

                return levelManager;
            }
        }

        private LevelManager()
        {
            Save = SaveGame.GetInstance().LoadGame();
        }

        public static void Init()
        {
            m_Collection = new List<Item>();

            if (Save != null) ChargeGame();

            m_DescriptionBox.SetActive(false);
            UnPauseAction?.Invoke();
        }

        public static void SubscribeAll()
        {
            BeginLevelAction += SetCurrentLevel;
            EndLevelAction += EndLevel;
            PauseAction += SetPause;
            UnPauseAction += UnPause;

            SubscribeGameAction.Invoke();
        }

        public static void UnsubsribeAll()
        {
            PauseAction.Invoke();
            BeginLevelAction -= SetCurrentLevel;
            EndLevelAction -= EndLevel;
            PauseAction -= SetPause;
            UnPauseAction -= UnPause;
            m_Collection = new List<Item>();
            SetGameWon(false);

            DisableGameAction.Invoke();
        }

        public static void SetDescriptionBoxActive(bool _state)
        {
            m_DescriptionBox.SetActive(_state);
        }

        public static void BeginLevel(LevelScripts _level)
        {
            SetDescriptionBoxActive(false);
            SetInventoryInActive();
        }

        public static void EndLevel(bool _state)
        {
            if (_state)
            {
                AudioManager.instance.PlaySound(SoundClip.GameWin, 1.0f);
            }
            else
            {
                AudioManager.instance.PlaySound(SoundClip.GameLose, 1.0f);
            }

            m_CurrentLevel.completed = _state;
            SetInventoryActive();
            SetDescriptionBoxActive(true);
        }

        private static void SetInventoryActive()
        {
            m_InventoryBox.SetActive(true);
        }

        private static void SetInventoryInActive()
        {
            m_InventoryBox.SetActive(false);
        }

        public static UnityEngine.Vector3 GetPlayerLastPosition()
        {
            return playerLastPosition;
        }

        public static Quaternion GetPlayerLastRotation()
        {
            return playerLastRotation;
        }


        public static void SetPlayerLastPosition(UnityEngine.Vector3 _position, UnityEngine.Quaternion _rotation)
        {
            playerLastPosition = _position;
            playerLastRotation = _rotation;
        }

        public static void SetCurrentLevel(LevelScripts _level)
        {
            m_CurrentLevel = _level;
        }

        private static void SetPause()
        {
            m_PauseScreen.SetActive(true);
        }

        private static void UnPause()
        {
            m_PauseScreen.SetActive(false);
        }

        public static void LoadGame()
        {
            Save = SaveGame.GetInstance().LoadGame();
        }

        public static void ChargeGame()
        {
            if (Save == null)
            {
                Debug.LogError("No save");
            }
            else
            {
                m_Collection = new List<Item>();
                for (int i = 0; i < Save.collection.Count; i++)
                {
                    CollectItemAction(Save.collection[i]);
                }

                GameObject.Find("Player").transform.position = Save.playerPosition;
            }
        }

        public static bool GetGameWon()
        {
            return m_GameWon;
        }

        public static void SetGameWon(bool _state)
        {
            m_GameWon = _state;
        }
    }
}