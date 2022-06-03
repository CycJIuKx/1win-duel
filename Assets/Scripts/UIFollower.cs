using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
public class UIFollower : MonoBehaviour
{
    private Transform target;
    private Transform myTransform;
    private Camera cam;
    private Vector3 lastPoint;
    [SerializeField] Vector3 offset;
    void Start()
    {
        cam = Camera.main;
        myTransform = GetComponent<Transform>();
    }

    void Update()
    {
        if (target)
        {
            lastPoint = target.position;
            myTransform.position = cam.WorldToScreenPoint(target.position + offset);
        }
        else
        {
            myTransform.position = cam.WorldToScreenPoint(lastPoint);
        }
        // else Destroy(gameObject);
    }
    public GameObject Init(Transform target)
    {
        this.target = target;
        return gameObject;
    }
}