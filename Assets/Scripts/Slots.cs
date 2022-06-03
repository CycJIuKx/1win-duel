using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Slots : MonoBehaviour
{
    public static Slots instance;
    public enum DirType { normal, up, down };
    public enum DelayType { one, tow, three };
    public enum BulletType { normal, pirce, exlosive };
    public enum BulleCount { one, tow, three };

    [System.Serializable]
    public class ShootStruct
    {

        public DirType dir;
        public DelayType delay;
        public BulletType bullet;
        public BulleCount count;
    }

    public System.Action<ShootStruct> onRolled;

    public List<Transform> towers;
    public List<Transform> towersStartPos;
    public Transform midLine;
    public List<bool> isRolled;
    public bool canRoll = true;
    public List<SlotUi> usedSlots;
    [System.Serializable]
    public struct Sprites
    {
        public Sprite bulletNorm, bulletExp, bulletPirce;
        public Sprite dirNorm, dirUp, dirDown;
        public Sprite delay1, delay2, delay3;
        public Sprite count1, count2, count3;

    }
    public Sprites sprits;
    public Clip slotClip;


    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
      //  resetTowerPositions();
        GameController.instance.OnRestart += OnRoundRestart;
    }
    public void SlotStaySound()
    {
        SoundCreator.Create(slotClip);
    }
    private void OnRoundRestart()
    {
        StopAllCoroutines();
        resetTowerPositions();
       // foreach (var item in usedSlots)
       // {
       //     item.StopAllCoroutines();
       // }
    }

    private void Update()
    {

    }
    void addSlotsToTower()
    {
        foreach (Transform item in towers[1])
        {
            item.GetComponent<SlotUi>().Init((Slots.DirType)Random.Range(0, 3), midLine);
        }
        foreach (Transform item in towers[3])
        {
            item.GetComponent<SlotUi>().Init((Slots.BulletType)Random.Range(0, 3), midLine);
        }
        foreach (Transform item in towers[0])
        {
            item.GetComponent<SlotUi>().Init((Slots.DelayType)Random.Range(0, 3), midLine);
        }
        foreach (Transform item in towers[2])
        {
            item.GetComponent<SlotUi>().Init((Slots.BulleCount)Random.Range(0, 3), midLine);
        }
    }
    void resetTowerPositions()
    {
        foreach (var item in usedSlots)
        {
            item.Refresh();
        }
        usedSlots.Clear();
        towers[0].position = towersStartPos[0].position;
        towers[1].position = towersStartPos[1].position;
        towers[2].position = towersStartPos[2].position;
        towers[3].position = towersStartPos[3].position;

        // foreach (Transform item in towers)
        // {
        //     foreach (Transform cell in item) cell.GetComponent<SlotUi>().entered = false;
        // }

    }
    public void OnShoot()
    {
        GameController.instance.OnShoot();
        foreach (var item in usedSlots)
        {
            item.OnShoot();
        }
    }
    public void StartRoll()
    {
        Server.Log("StartRoll actor- " + GameManager.instance.myActor);
        GameController.instance.blockShoot = true;
        //  Debug.Log("StartRoll");
        isRolled[0] = false;
        isRolled[1] = false;
        isRolled[2] = false;
        isRolled[3] = false;
        addSlotsToTower();
        resetTowerPositions();
        StartCoroutine(process());
    }
    IEnumerator process()
    {
       
       moveDown(0);
       moveDown(1);
       moveDown(2);
       moveDown(3);
        yield return new WaitForSeconds(2);
      //  yield return new WaitUntil(() => isRolled[0] && isRolled[1] && isRolled[2] && isRolled[3]);
        ShootStruct shoot = new ShootStruct
        {
            bullet = GetBullet(),
            dir = GetDir(),
            delay = GetDelay(),
            count = GetCount(),
        };
        onRolled.Invoke(shoot);

    }
    BulleCount GetCount()
    {
        foreach (Transform item in towers[2])
        {
            if (item.GetComponent<SlotUi>().entered) return item.GetComponent<SlotUi>().count;
        }
        return 0;
    }
    DirType GetDir()
    {
        foreach (Transform item in towers[1])
        {
            if (item.GetComponent<SlotUi>().entered) return item.GetComponent<SlotUi>().dir;
        }
        return 0;
    }
    BulletType GetBullet()
    {
      //  return BulletType.exlosive;
        foreach (Transform item in towers[3])
        {
            if (item.GetComponent<SlotUi>().entered) return item.GetComponent<SlotUi>().bullet;
        }
        return 0;
    }
    DelayType GetDelay()
    {
        foreach (Transform item in towers[0])
        {
            if (item.GetComponent<SlotUi>().entered) return item.GetComponent<SlotUi>().delay;
        }
        return 0;
    }
    void moveDown(int index)
    {
        int r = Random.Range(3, towers[index].childCount-2);
        usedSlots.Add(towers[index].GetChild(r).GetComponent<SlotUi>());
        towers[index].GetChild(r).GetComponent<SlotUi>().SetPos();
        isRolled[index] = true;
        

    }
}
