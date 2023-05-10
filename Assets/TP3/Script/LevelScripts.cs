using System;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelScript", menuName = "ScriptableObjects/LevelScript", order = 1)]
public class LevelScripts : ScriptableObject
{
    public string levelName;
    public string description;
    public bool completed;
    public Vector3 cameraFocus;
    public Vector3 cameraOffset;
    public Vector3 PlayerBeginPosition;
}