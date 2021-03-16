using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public delegate void OnSuccessHandler(WWW www);
public delegate void OnErrorHandeler(WWW www);
public delegate void OnPreloadHAndler(int bytes);


public class ServerController : MonoBehaviour
{

    #region Singlton
    private static ServerController instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private static bool isNullreference
    {
        get
        {
            return instance == null;
        }
    }

    public static ServerController GetInstance
    {
        get
        {
            return instance;
        }
    }

    #endregion
    #region Public Delegates
    public static OnSuccessHandler onSuccessHandler;
    public static OnSuccessHandler onSuccessHandler2;
    public static OnErrorHandeler onErrorHandeler;
    public static OnPreloadHAndler onPreloadHandler;
    #endregion

    #region Private fields
    private string host = "http://45.80.69.197:9999/";//"http://45.80.69.197:9999/";//"http://185.184.247.167:9999/";//
    Dictionary<string, string> headers;
    FileStream file = new FileStream();
    #endregion

    #region Post Request
    void setHeaders(bool isHeader)
    {
        headers = new Dictionary<string, string>();
        if(isHeader)
        //headers.Add("Authorization", UserModel.user.data.token_type + " " + UserModel.user.data.access_token);
        headers.Add("accept", "application/json");
    }

    public static void PostREquest(string comand, WWWForm form, bool isheaders)
    {
        if (isNullreference) return;
        instance.StartCoroutine(instance.postRequest(comand, form, isheaders));
    }

    IEnumerator postRequest(string comand, WWWForm form, bool isheaders)
    {
        headers = null;
        setHeaders(isheaders);
        WWW www;
        if (form == null)
            www = new WWW(host + comand, null, headers);
        else
            www = new WWW(host + comand, form.data, headers);

        yield return www;
        if (string.IsNullOrEmpty(www.error))
        {
            if (onSuccessHandler != null)
                onSuccessHandler(www);
            if (onSuccessHandler2 != null)
                onSuccessHandler2(www);
        }
        else
        {
            if(onErrorHandeler != null)
                onErrorHandeler(www);
            else
            {
                onSuccessHandler = null;
            }
            Debug.Log(www.text);
        }
    }
    #endregion

    #region Get File from web
    /*
        Ctreate File from byte arra and sawe it
        string comand - Comand for URL
        sring path - Path whre the file will be saved
    */
    public static void GetFileFromLink(string comand, string path)
    {
        instance.StartCoroutine(instance.GetFile(comand, path));
    }
    IEnumerator GetFile(string comand, string path)
    {
        WWW www = new WWW(host + comand);
        while (!www.isDone)
        {
            //Debug.Log( "bytes : " + www.bytesDownloaded);
            if(onPreloadHandler!= null)
            {
                onPreloadHandler(www.bytesDownloaded);
            }
            yield return null;
        }
        
        

        if (!string.IsNullOrEmpty(www.error))
        {
            Debug.Log(www.error);
        }
        else
        {
            byte[] results = www.bytes;
                file.CreateFileFromBytes(path, results);

                onSuccessHandler(null);
        }
    }
    #endregion
}
