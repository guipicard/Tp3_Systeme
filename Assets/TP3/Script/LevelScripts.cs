using UnityEngine;

[CreateAssetMenu(fileName = "LevelScript", menuName = "ScriptableObjects/LevelScript", order = 1)]
public class LevelScripts : ScriptableObject
{
    public string levelName;
    public string description;
    public bool completed;
    public GameObject item;
}