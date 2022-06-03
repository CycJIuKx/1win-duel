using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Clip
{
    public AudioClip clip;
    public float volume=0.6f;
}
public class SoundCreator : MonoBehaviour
{
    [SerializeField] Transform content;
    static SoundCreator instance;
    [SerializeField] GameObject Pref;

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        
    }

    
   public static GameObject Create(Clip clip, float time=8)
    {
        GameObject go = Instantiate(instance.Pref, instance.content);
        go.GetComponent<AudioSource>().clip = clip.clip;
        go.GetComponent<AudioSource>().volume = clip.volume;
        go.GetComponent<AudioSource>().Play();
        Destroy(go, time);
        return go;
    }
}
