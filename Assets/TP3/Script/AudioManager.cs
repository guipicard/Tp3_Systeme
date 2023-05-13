using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public enum SoundClip
{
    WrongTree,
    Win,
    MenuClick,
    MenuSelect,
    MenuOpen,
    Step,
    GameWin,
    GameLose,
    Pickup
}


public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField]
    private Transform m_PoolParent;

    public AudioSource audioSourcePrefab;
    public int poolSize = 10;
    public List<AudioClip> SoundList;
    public Dictionary<SoundClip, AudioClip> SoundDictionary;
    private List<AudioSource> activeSources;



    private AudioPool audioPool;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            audioPool = new AudioPool(audioSourcePrefab, poolSize, m_PoolParent);
            SoundDictionary = new Dictionary<SoundClip, AudioClip>();
            activeSources = new List<AudioSource>();
            for (int i = 0; i < SoundList.Count; i++)
            {
                SoundDictionary.Add((SoundClip)i, SoundList[i]);
            }
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    public void PlaySound(SoundClip _clip, float _volume)
    {
        var source = audioPool.GetPooledObject();
        if (source != null)
        {
            source.clip = SoundDictionary[_clip];
            source.volume = _volume;
            activeSources.Add(source);
            source.Play();
        }
    }

    private void Update()
    {
        for (int i = activeSources.Count - 1; i >= 0; i--)
        {
            if (!activeSources[i].isPlaying)
            {
                audioPool.ReturnPooledObject(activeSources[i]);
                activeSources.RemoveAt(i);
            }
        }
    }
}
