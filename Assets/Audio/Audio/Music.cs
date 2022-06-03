using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music : MonoBehaviour
{
    public List<Clip> clips;
    public AudioSource source;
    void Start()
    {
     
        //Clip c = clips.GetRandomElement();
        //source.clip = c.clip;
        //source.volume = c.volume;
        //source.Play();
        Helpers.TowardMover.Add(gameObject, Helpers.TowardMover.MoveObject.volume, 0, 0.2f, 0.1f, false);
    }

    
    void Update()
    {
        if (!source.isPlaying)
        {
            Clip c = clips.GetRandomElement();
            source.clip = c.clip;
            source.volume = c.volume;
            source.Play();
        }
    }
}
