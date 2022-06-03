using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour, IPunInstantiateMagicCallback, IOnEventCallback
{
    public Clip normShoot, pirceShoot, expShoot;
    public float speed = 10;
    public Vector3 dir;
    public Slots.BulletType type;
    public GameObject explosionPref;
    [SerializeField] GameObject normal, pirce, explosive;
    [SerializeField] GameObject pirceCollision, normalKill, pierceKill;
    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        type = (Slots.BulletType)info.photonView.InstantiationData[0];
        dir = (Vector3)info.photonView.InstantiationData[1];
        if (type == Slots.BulletType.normal) normal.SetActive(true);
        if (type == Slots.BulletType.exlosive) explosive.SetActive(true);
        if (type == Slots.BulletType.pirce) pirce.SetActive(true);

        AddMethods.TurnToPoint(new Vector3(dir.x, dir.y, 0) + transform.position, transform);

        if (type == Slots.BulletType.normal) SoundCreator.Create(normShoot);
        if (type == Slots.BulletType.pirce) SoundCreator.Create(pirceShoot);
        if (type == Slots.BulletType.exlosive) SoundCreator.Create(expShoot);
    }
    void Start()
    {
   
    }
    void Update()
    {
        transform.Translate(dir * Time.deltaTime * speed, Space.World);
    }
   

    public void TurnToPoint(Vector3 point)
    {
        Vector2 direction = point - transform.position;
        float angle = Mathf.Atan2(-direction.x, direction.y) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!GetComponent<PhotonView>().IsMine) return;
       

        if (type == Slots.BulletType.normal)
        {
            if (collision.gameObject.CompareTag("Player")&&!isMyPlayer(collision.gameObject))
            {
                if (collision.gameObject.GetComponent<PlayerController>().IsDead)
                {
                    PhotonNetwork.Destroy(gameObject);
                    return;
                }
                GameController.instance.OnEnemyKilled(collision.gameObject);
                PhotonNetwork.Instantiate(normalKill.name, transform.position, transform.rotation);
                PhotonNetwork.Destroy(gameObject);
            }
            if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Building"))
            {
                PhotonNetwork.Instantiate(pirceCollision.name, transform.position, transform.rotation);
                PhotonNetwork.Destroy(gameObject);
            }
        }



        if (type == Slots.BulletType.pirce)
        {
            if (collision.gameObject.CompareTag("Player") && !isMyPlayer(collision.gameObject))
            {
                if (collision.gameObject.GetComponent<PlayerController>().IsDead)
                {
                    PhotonNetwork.Destroy(gameObject);
                    return;
                }
                GameController.instance.OnEnemyKilled(collision.gameObject);
                PhotonNetwork.Instantiate(pierceKill.name, transform.position, transform.rotation);
                PhotonNetwork.Destroy(gameObject);
            }
            else PhotonNetwork.Instantiate(pirceCollision.name, transform.position, transform.rotation);
        }



        if (type == Slots.BulletType.exlosive)
        {
            if (collision.gameObject.CompareTag("Player") && isMyPlayer(collision.gameObject)) return;
                Explosion();
            PhotonNetwork.Instantiate(explosionPref.name, transform.position, Quaternion.identity);
            PhotonNetwork.Destroy(gameObject);
        }

    }
    bool isMyPlayer(GameObject go)
    {
        if (go.GetComponent<PhotonView>().Owner.ActorNumber == GameManager.instance.myActor)
        {
          
            return true;
        }
        return false;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {///TTEST

       
        if (!GetComponent<PhotonView>().IsMine) return;
        if (type == Slots.BulletType.normal)
        {
            if (collision.gameObject.CompareTag("Player")) {
                if (collision.gameObject.GetComponent<PlayerController>().IsDead)
                {
                    PhotonNetwork.Destroy(gameObject);
                    return;
                }
                GameController.instance.OnEnemyKilled(collision.gameObject);
                PhotonNetwork.Instantiate(normalKill.name, transform.position, transform.rotation);
                PhotonNetwork.Destroy(gameObject);
            }
            if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Building"))
            {
                PhotonNetwork.Instantiate(pirceCollision.name, transform.position, transform.rotation);
                PhotonNetwork.Destroy(gameObject);
            }
        }



        if (type == Slots.BulletType.pirce)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                if (collision.gameObject.GetComponent<PlayerController>().IsDead)
                {
                    PhotonNetwork.Destroy(gameObject);
                    return;
                }
                GameController.instance.OnEnemyKilled(collision.gameObject);
                PhotonNetwork.Instantiate(pierceKill.name, transform.position, transform.rotation);
                PhotonNetwork.Destroy(gameObject);
            }
            else PhotonNetwork.Instantiate(pirceCollision.name, transform.position, transform.rotation);
        }



        if (type == Slots.BulletType.exlosive)
        {
            Explosion();
            PhotonNetwork.Instantiate(explosionPref.name, transform.position, Quaternion.identity);
            PhotonNetwork.Destroy(gameObject);
        }
        




        Debug.Log(collision.gameObject.name);

    }
    
    void Explosion()
    {
        
        foreach (var item in Physics2D.OverlapCircleAll(transform.position, 2))
        {
            if (item.gameObject.CompareTag("Player")) {
                if (item.gameObject.GetComponent<PlayerController>().IsDead|| isMyPlayer(item.gameObject))
                {

                    continue;
                }
                item.gameObject.GetComponent<PlayerController>().SetFire();
                GameController.instance.OnEnemyKilled(item.gameObject); 
            }
        }
    }











    public void OnEvent(EventData photonEvent)
    {

        if (photonEvent.Code == GameManager.ON_RESTART) { Destroy(gameObject); Logger.Log("bullet destroy"); }
    }
    private void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }

    private void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }
}
