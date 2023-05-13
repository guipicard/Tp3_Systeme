using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using TP3.Script;
using UnityEngine;
using UnityEngine.SceneManagement;


[System.Serializable]
public class SaveGame
{
    private static SaveGame m_Instance;

    public static SaveGame GetInstance()
    {
        if (m_Instance == null)
        {
            m_Instance = new SaveGame();
        }

        return m_Instance;
    }
    public List<Inventory.ItemType> collection = new List<Inventory.ItemType>();
    public Vector3 playerPosition;

    public void SaveData()
    {
        SaveGame data = new SaveGame();
        foreach (var item in LevelManager.m_Collection)
        {
            data.collection.Add(item.type); 
        }
        data.playerPosition = GameObject.Find("Player").transform.position;
        
        string json = JsonUtility.ToJson(data);
        
        string path = Path.Combine(Application.persistentDataPath, "save.json");
        File.WriteAllText(path, json);
        LevelManager.LoadGame();
    }
    
    public SaveGame LoadGame()
    {
        string path = Path.Combine(Application.persistentDataPath, "save.json");
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveGame data = JsonUtility.FromJson<SaveGame>(json);
            return data;
        }
        else
        {
            Debug.LogError("Save file not found at " + path);
        }

        return null;
    }
}
