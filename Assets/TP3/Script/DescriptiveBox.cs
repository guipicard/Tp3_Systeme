using System.Collections;
using System.Collections.Generic;
using TMPro;
using TP3.Script;
using UnityEngine;

public class DescriptiveBox : MonoBehaviour
{
    [SerializeField] private TextMeshPro m_LevelName;
    [SerializeField] private TextMeshPro m_LevelDescription;
    
    void Start()
    {
        LevelManager.descriptionAction += SetDescription;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetDescription(LevelScripts _levelData)
    {
        m_LevelName.text = _levelData.levelName;
        m_LevelDescription.text = _levelData.description;
    }
}
