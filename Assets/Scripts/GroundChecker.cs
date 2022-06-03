using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundChecker : MonoBehaviour
{
    public bool isGround;
    public List<GameObject> colls;
    public Clip clip;
    public float minVelositySound=-1;
    public float lastV;
    bool watch = false;
    public GameObject dustPref;
    void Start()
    {
        if (transform.parent.GetComponent<Photon.Pun.PhotonView>().IsMine) watch = true;
    }


    void Update()
    {
      if(watch)  lastV = transform.parent.GetComponent<Rigidbody2D>().velocity.y;
    }
    private void FixedUpdate()
    {
       
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
    
        if (collision.gameObject.CompareTag("Ground"))
        {
            if (collision.gameObject.name=="main")
            {
                Instantiate(dustPref, collision.contacts[0].point, Quaternion.identity);
            }
            // Debug.Log("Velosoty " + transform.parent.GetComponent<Rigidbody2D>().velocity.y);
            if (lastV < minVelositySound)  SoundCreator.Create(clip);
            colls.Add(collision.gameObject);
        }
        if (colls.Count > 0) isGround = true;
        else isGround = false;

    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            colls.Remove(collision.gameObject);
        if (colls.Count > 0) isGround = true;
        else isGround = false;
    }
}
