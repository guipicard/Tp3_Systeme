using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelPool : MonoBehaviour
{

    private RaycastHit m_Hit;
    
    [System.Serializable]
    public struct DoorLevel
    {
        public GameObject door;
        public LevelScripts level;
    }

    [SerializeField]
    public List<DoorLevel> Levels;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        foreach (var door in Levels)
        {
            if (door.level.completed)
            {
                door.door.transform.Find("powerup_arrow_1").gameObject.SetActive(false);
                continue;
            }
            Debug.DrawRay(door.door.transform.position + Vector3.up, Vector3.back);
            if (Physics.Raycast(door.door.transform.position + Vector3.up, Vector3.back, out m_Hit))
            {
                if (m_Hit.collider.gameObject.layer == 6)
                {
                    LevelManager.instance.descriptionAction?.Invoke(door.level); 
                    LevelManager.instance.SetDescriptionBoxActive(true);
                    if (Input.GetKeyDown(KeyCode.Return))
                    {
                        LevelManager.instance.BeginLevel();
                        LevelManager.instance.BeginLevelAction?.Invoke(door.level);
                    }
                    break;
                }
            }
            else
            {
                LevelManager.instance.SetDescriptionBoxActive(false);
            }
        }
    }

    public List<DoorLevel> GetLevels()
    {
        return Levels;
    }
}
