using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SlotUi : MonoBehaviour
{
    public Slots.BulletType bullet;
    public Slots.DelayType delay;
    public Slots.DirType dir;
    public Slots.BulleCount count;
    public bool entered;
    RectTransform tower;
    Transform midLine;
    void Start()
    {
        tower =transform.parent.GetComponent<RectTransform>();
    }


    public void Init(Slots.BulletType b, Transform mid)
    {
        //смена картинки
        entered = false;
        if (b == Slots.BulletType.exlosive) GetComponent<Image>().sprite = Slots.instance.sprits.bulletExp;
        if (b == Slots.BulletType.normal) GetComponent<Image>().sprite = Slots.instance.sprits.bulletNorm;
        if (b == Slots.BulletType.pirce) GetComponent<Image>().sprite = Slots.instance.sprits.bulletPirce;

        bullet = b;
        midLine = mid;
    }

    public void Init(Slots.DelayType b, Transform mid)
    {
        if (b == Slots.DelayType.one) GetComponent<Image>().sprite = Slots.instance.sprits.delay1;
        if (b == Slots.DelayType.tow) GetComponent<Image>().sprite = Slots.instance.sprits.delay2;
        if (b == Slots.DelayType.three) GetComponent<Image>().sprite = Slots.instance.sprits.delay3;

        entered = false;
        delay = b;
        midLine = mid;
    }
    public void Init(Slots.BulleCount b, Transform mid)
    {
        if (b == Slots.BulleCount.one) GetComponent<Image>().sprite = Slots.instance.sprits.count1;
        if (b == Slots.BulleCount.tow) GetComponent<Image>().sprite = Slots.instance.sprits.count2;
        if (b == Slots.BulleCount.three) GetComponent<Image>().sprite = Slots.instance.sprits.count3;

        entered = false;
        count = b;
        midLine = mid;
    }
    public void Init(Slots.DirType b, Transform mid)
    {
        if (b == Slots.DirType.normal) GetComponent<Image>().sprite = Slots.instance.sprits.dirNorm;
        if (b == Slots.DirType.down) GetComponent<Image>().sprite = Slots.instance.sprits.dirDown;
        if (b == Slots.DirType.up) GetComponent<Image>().sprite = Slots.instance.sprits.dirUp;
        entered = false;
        dir = b;
        midLine = mid;

    }
    public void OnShoot()
    {
        foreach (Transform item in transform.parent)
        {
            if (item!=transform)
            {
                item.GetChild(0).GetComponent<SpriteMask>().enabled = false;
                item.GetChild(1).GetComponent<SpriteMask>().enabled = false;
                item.GetChild(2).GetComponent<SpriteMask>().enabled = false;
            }
            else
            {
                item.GetChild(0).GetComponent<SpriteMask>().enabled = true;
                item.GetChild(1).GetComponent<SpriteMask>().enabled = true;
                item.GetChild(2).GetComponent<SpriteMask>().enabled = true;
            }
        }
        GetComponent<Image>().enabled = false;
        foreach (var item in GetComponentsInChildren<ParticleSystem>())
        {
            item.Play();
        }   
    } 
    public void Refresh()
    {
        GetComponent<Image>().enabled = true;
        StopAllCoroutines();
    }
    public void SetPos()
    {
        StartCoroutine(process());
    }
    IEnumerator process()
    {
        entered = true;
        float speed = Random.Range(100, 70);
        if (transform.position.y > midLine.position.y)
        {
            while (transform.position.y > midLine.position.y)
            {
                tower.Translate(Vector2.down * speed * Time.deltaTime);
                yield return null;
            }
        } else
        {
            while (transform.position.y < midLine.position.y)
            {
                tower.Translate(Vector2.up * speed * Time.deltaTime);
                yield return null;
            }
        }
        Slots.instance.SlotStaySound();
    }

}
