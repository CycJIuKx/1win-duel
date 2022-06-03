using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UICreator : MonoBehaviour
{
    public static UICreator instance;
    [SerializeField] GameObject textPref;
    [SerializeField] Transform content;
    private void Awake()
    {
        instance = this;
    }
 
    public GameObject CreateFollowText(string t, Transform target)
    {
        GameObject go = Instantiate(textPref, content);
        go.GetComponent<Text>().text = t;
       return go.GetComponent<UIFollower>().Init(target);
    }
}
