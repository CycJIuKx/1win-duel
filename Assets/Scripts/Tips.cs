using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tips : MonoBehaviour
{
    public Clip clip;
    [TextArea(5 ,5)]
    public List<string> tips;
    int index=1;
    public Text text;
    void Start()
    {

    }


    public void Next()
    {
        SoundCreator.Create(clip);
        if (index == tips.Count) index = 0;

        text.text = tips[index];
        index++;
    }
    
}
