using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DescriptiveBox : MonoBehaviour
{
    [SerializeField] private TextMeshPro m_LevelName;
    [SerializeField] private TextMeshPro m_LevelDescription;
    
    void Start()
    {
        LevelManager.instance.descriptionAction += SetDescription;
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
