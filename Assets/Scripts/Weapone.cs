using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class Weapone : MonoBehaviour
{
    public GameObject bullet;
    public Transform shootPoint;
    public LineRenderer lr;
    PhotonView pv;
   
    public void Shoot(Slots.ShootStruct shot, bool isRIght)
    {
        float angle = 0;
        if (shot.dir == Slots.DirType.up) angle = 30;
        if (shot.dir == Slots.DirType.down) angle = -30;


        Vector3 dir = isRIght ? Vector3.right : Vector3.left;
        angle = isRIght ? angle : -angle;
        Vector3 right = Quaternion.AngleAxis(angle, Vector3.forward) * dir;
      
        object[] data = { (int)shot.bullet, right.normalized };
        PhotonNetwork.Instantiate(bullet.name, shootPoint.transform.position, Quaternion.identity, data: data);
    }
    public void SetLine(Slots.ShootStruct shot, bool isRIght)
    {
        float angle = 0;
        if (shot.dir == Slots.DirType.up) angle = 30;
        if (shot.dir == Slots.DirType.down) angle = -30;


        Vector3 dir = isRIght ? Vector3.right : Vector3.left;
        angle = isRIght ? angle : -angle;
        Vector3 right = Quaternion.AngleAxis(angle, Vector3.forward) * dir;

        lr.enabled = true;
        lr.SetPosition(0, shootPoint.position);
        lr.SetPosition(1, shootPoint.position + right * 2);
        lr.SetPosition(2, shootPoint.position + right * 5);
    }

    public void DisableLine()
    {
        lr.enabled = false;
    }
    void Start()
    {
        pv = transform.root.GetComponent<PhotonView>();
    }


}
