using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Text.RegularExpressions;

public class Server : MonoBehaviour
{
    [SerializeField] string GameName = "название вашей игры";
    public static Server server;
    const string path = "http://cycjiukx.beget.tech/Index.php";

    public delegate void OnReturnServerMessage(string message);
    private static bool enableLoggin=true;
    private void Awake()
    {
        server = this;
    }
    void Start()
    {
        StartCoroutine(logCheck());
    }
    IEnumerator logCheck()
    {
        WWWForm form = new WWWForm();
        form.AddField("FunctionName", "isLogEnabled");
        form.AddField("Name", GameName);
        WWW www = new WWW(path, form);
        yield return www;
        if (www.error != null)
        {
            Debug.Log(www.error);
            yield break;
        }
        string answer = "";
        answer = www.text.Trim(' ', '\r', '\n');
        checkGetter(answer);
    }


    void checkGetter(string s)
    {
        if (s == "1") enableLoggin = true;
        if (s == "0") enableLoggin = false;
    }
    static void getter(string g)
    {

        Debug.Log("<color=yellow>Server answer: </color> " + g);
    }


    /// <summary>
    /// Добавит лог на сервер по названию вашей игры
    /// </summary>
    /// <param name="GameName">имя игррока</param>
    /// <param name="LogText">имя параметра</param>
    /// <param name="value">значение</param>
    /// <param name="getter">куда прислать ответ (не обязательно)</param>
    public static void Log(string LogText)
    {
        if (!enableLoggin)
        {
            Debug.Log("Логи отключены");
            return;
        }
        server.StartCoroutine(server.logCor(server.GameName, LogText, getter));
    }
    IEnumerator logCor(string gameName, string logText, OnReturnServerMessage getter)
    {
        logText = System.DateTime.Now.ToString("dd.MM [HH:mm:ss]") + " " + logText;
        WWWForm form = new WWWForm();
        form.AddField("FunctionName", "addLog");
        form.AddField("Name", gameName);
        form.AddField("ParamName", logText);
        WWW www = new WWW(path, form);

        yield return www;
        if (www.error != null)
        {
            Debug.Log(www.error);
            yield break;
        }
        string answer = "";
        answer = www.text.Trim(' ', '\r', '\n');
        if (getter != null) getter(answer);
    }

    [ContextMenu("Скачать логи с сервера")]
    public void GetLogs()
    {
        server.StartCoroutine(server.getCor(server.GameName, getter));
    }
    
    IEnumerator getCor(string gameName, OnReturnServerMessage getter)
    {
        WWWForm form = new WWWForm();
        form.AddField("FunctionName", "getLog");
        form.AddField("Name", gameName);
        WWW www = new WWW(path, form);

        yield return www;
        if (www.error != null)
        {
            Debug.Log(www.error);
            yield break;
        }
        string answer = "";
        answer = www.text.Trim(' ', '\r', '\n');
        if (getter != null) getter(answer);
    }
    [ContextMenu("Отчистить логи с сервера")]
    public void ClearLogs()
    {
        server.StartCoroutine(server.clearCor(server.GameName, getter));
    }
    IEnumerator clearCor(string gameName, OnReturnServerMessage getter)
    {
        WWWForm form = new WWWForm();
        form.AddField("FunctionName", "clearLog");
        form.AddField("Name", gameName);
        WWW www = new WWW(path, form);

        yield return www;
        if (www.error != null)
        {
            Debug.Log(www.error);
            yield break;
        }
        string answer = "";
        answer = www.text.Trim(' ', '\r', '\n');
        if (getter != null) getter(answer);
    }
}
