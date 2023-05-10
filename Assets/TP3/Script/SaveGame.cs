using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using UnityEngine;


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
    public List<InventoryItem.Item> collection;
    public Vector3 playerPosition;

    public void SaveData()
    {
        SaveGame data = new SaveGame();
        data.collection = GameObject.Find("Inventory").GetComponent<Inventory>().m_Collection;
        data.playerPosition = GameObject.Find("Player").transform.position;
        
        string json = JsonUtility.ToJson(data);
        
        string path = Path.Combine(Application.persistentDataPath, "save.json");
        File.WriteAllText(path, json);
        Debug.Log("SAvede");
    }
    
    public void LoadGame()
    {
        string path = Path.Combine(Application.persistentDataPath, "save.json");
        Debug.Log(path);
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveGame data = JsonUtility.FromJson<SaveGame>(json);

            GameObject.Find("Inventory").GetComponent<Inventory>().m_Collection = data.collection;
            GameObject.Find("Player").transform.position = data.playerPosition;

        }
        else
        {
            Debug.LogError("Save file not found at " + path);
        }
    }
}
