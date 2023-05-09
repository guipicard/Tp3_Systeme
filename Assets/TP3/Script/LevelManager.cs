using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private int starsCollected;
    private int lightningsCollected;
    private int m_CurrentLevel = 1;

    private static LevelManager levelManager;

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


    void Start()
    {
        
    }



   
}