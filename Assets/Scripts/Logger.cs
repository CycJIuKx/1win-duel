using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Logger : MonoBehaviour
{
    public Transform content;
    public GameObject textPref;
    public static Logger instance;
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        
    }

    
    public static void Log(string log)
    {
        GameObject go = Instantiate(instance.textPref, instance.content);
        go.GetComponent<Text>().text = log;
        Destroy(go, 10);
    }
}
