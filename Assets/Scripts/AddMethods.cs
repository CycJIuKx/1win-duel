using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public static class AddMethods
{
    private static System.Random rng = new System.Random();
    /// <summary>
    /// Убирает нулевые ссылки, возвращая список, НЕ МЕНЯЕТ САМ СЕБЯ
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="myList"></param>
    /// <returns></returns>
    public static List<T> RemoveNulls<T>(this List<T> myList)
    {

        myList = myList.Where(item => item != null).ToList();
        return myList;



    }
    /// <summary>
    /// поворачивает 2 д спрайт на направлению к дирекшену( осью Y)
    /// </summary>
    /// <param name="Dir"></param>
    /// <param name="myTrans"></param>
    public static void TurnToPoint(Vector3 Dir, Transform myTrans)
    {
        Vector2 direction = Dir - myTrans.position;
        float angle = Mathf.Atan2(-direction.x, direction.y) * Mathf.Rad2Deg;
        myTrans.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
    public static T GetRandomElement<T>(this List<T> myList)
    {

        int r = Random.Range(0, myList.Count);
        return myList[r];



    }
    public static Vector3 GetMousePosAtWorldByScreen(Camera cam)
    {
        return cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -cam.transform.position.z));
    }
    public static void Shuffle<T>(this IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
    public static T GetRandomObj<T>(this IList<T> list)
    {
        return list[Random.Range(0, list.Count)];
    }

    public static float GetRandom(this Vector2 r)
    {
        return Random.Range(r.x, r.y);
    }
    /// <summary>
    /// возвращает нормаль где 1 это цель перед вами, -1 позади, 0 сбоку
    /// /// </summary>
    /// <param name="owner"></param>
    /// <param name="target"></param>
    /// <returns></returns>
    public static float GetDotDir2D(this Transform owner, Transform target)//,Transform owner)
    {
        Vector3 forward = owner.TransformDirection(Vector3.up);
        Vector3 toOther = target.transform.position - owner.position;
        toOther.Normalize();
        return Vector3.Dot(forward, toOther);
    }
    public static void SetLeft(this RectTransform rt, float left)
    {
        rt.offsetMin = new Vector2(left, rt.offsetMin.y);
    }

    public static void SetRight(this RectTransform rt, float right)
    {
        rt.offsetMax = new Vector2(-right, rt.offsetMax.y);
    }

    public static void SetTop(this RectTransform rt, float top)
    {
        rt.offsetMax = new Vector2(rt.offsetMax.x, -top);
    }

    public static void SetBottom(this RectTransform rt, float bottom)
    {
        rt.offsetMin = new Vector2(rt.offsetMin.x, bottom);
    }
}
namespace Save
{
    public static class PlayerPrefs
    {
        public static void SetBool(string path, bool key)
        {
            UnityEngine.PlayerPrefs.SetString(path, key.ToString());
        }
        public static bool GetBool(string path)
        {
            string b = UnityEngine.PlayerPrefs.GetString(path);

            return b == "True" ? true : false;

        }
    }
}
[System.Serializable]
public class Int
{

    public int normal, value;
    public void AddProcent(int procent)
    {
        value += (int)((float)normal * (float)procent / 100);
    }
    public static Int operator *(Int a, int b)
    {
        // сравниваем цену книг
        a.value *= b;
        a.normal *= b;
        return a;
    }
    public static Int operator /(Int a, int b)
    {
        // сравниваем цену книг
        a.value /= b;
        a.normal /= b;
        return a;
    }
}
[System.Serializable]
public class Float
{
    public float normal, value;
    public void AddProcent(float procent)
    {
        value += normal * procent / 100;
    }
    public static Float operator /(Float a, int b)
    {
        // сравниваем цену книг
        a.value /= b;
        a.normal /= b;
        return a;
    }
    public static Float operator *(Float a, int b)
    {
        // сравниваем цену книг
        a.value *= b;
        a.normal *= b;
        return a;
    }
}
[System.Serializable]
public class Points
{
    public int current, max;
    public int regen;
    public void RegenIteration()
    {
        current += regen;
        if (current > max) current = max;
    }
}