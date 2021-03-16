using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BestHTTP.SocketIO;
using UnityEngine.SceneManagement;

public delegate void AftergetTrueSocketMessage();

public struct CallMessage
{
    public string from;
    public string to;
    public string comand;
    public string message;
}

public class WEbSocketController : MonoBehaviour
{
    #region Singlton
    private static WEbSocketController instance;

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

    public static WEbSocketController GetInstance
    {
        get
        {
            return instance;
        }
    }

    #endregion

    #region Private fields
    SocketManager Manager;
    string address = "http://45.80.69.197:9999/socket.io/";
    public bool isConnect = false;
    //public WaytForACallController wayt;

    Dictionary<string, object> data;
    #endregion

    #region Init
    // Start is called before the first frame update
    void Start()
    {
        var options = new SocketOptions();
        //options.ConnectWith = BestHTTP.SocketIO.Transports.TransportTypes.Polling;
        Manager = new SocketManager(new System.Uri(address), options);
        if(!isConnect)
        {
            Debug.Log("io_wb socketio Start Conntecetion");
            Connect();
        }
    }
    #endregion

    #region Socket Connect Disconnect
    public void Connect()
    {
        //webSocket.Open();
        Manager.Socket.On(SocketIOEventTypes.Connect, (s, p, a) =>
        {
            Debug.Log("io_wb socketio Connteceted");
            isConnect = true;
        });

        Manager.Socket.On(SocketIOEventTypes.Error, (socket, packet, args) =>
        {
            //Debug.Log(string.Format("socketio Error: {0}", args[0].ToString()));
        });

        Manager.Socket.On("message", OnNewMessage);
        Manager.Socket.On("rooms", OnRoomMessage);

        Manager.Socket.On(SocketIOEventTypes.Disconnect, (s, p, a) =>
        {
            Debug.Log("socketio DisConnteceted");
        });


    }
    
    public void Disconnect()
    {
        Manager.Socket.Disconnect();
    }
    #endregion
    void OnRoomMessage(Socket socket, Packet packet, params object[] args)
    {
        //addChatMessage();
       /* data = args[0] as Dictionary<string, object>;
        Debug.Log(data["data"] + " "+ data.Count);*/
        string msg = args[0] as string;
        Debug.Log("io_wb Room Geted : " + msg);
        if (string.IsNullOrEmpty(msg)) {
            Debug.Log("returned "+ msg);
            return; 
        }
        GLobalParametrs.rooms = JsonUtility.FromJson<RoomsModel>(msg);

    }

    void OnNewMessage(Socket socket, Packet packet, params object[] args)
    {
        //addChatMessage();
        //data = args[0] as Dictionary<string, object>;
        string msg = args[0] as string;
        Debug.Log("io_wb socketio Geted : " + msg);
        if (string.IsNullOrEmpty(msg) || msg.Equals("Connected")) return;

        CallMessage cmsg = JsonUtility.FromJson<CallMessage>(msg);

        if(cmsg.to.Equals(GLobalParametrs.Player.Name))
        {
            if(cmsg.comand.Equals("join"))
            {
                GLobalParametrs.Anemy.Name = cmsg.from;
                GLobalParametrs.playRoom.ConnectorName = cmsg.from;
                SceneManager.LoadScene(2);
            }

            if(cmsg.comand.Equals("gamemessage"))
            {
                GLobalParametrs.gmessage = JsonUtility.FromJson<GameMessage>(cmsg.message);
                FiedController.GetInstance.AnemyVisulizeFromIO();
                GLobalParametrs.Presed = true;
            }

            if(cmsg.comand.Equals("strike"))
            {
                GLobalParametrs.Presed = false;
                FiedController.GetInstance.RestAllButtons();
            }

            if(cmsg.comand.Equals("turn"))
            {
                if (cmsg.message.Equals("player")) GLobalParametrs.turn = Turn.Anemy;
                else if(cmsg.message.Equals("anemy")) GLobalParametrs.turn = Turn.Player;
            }
            if (cmsg.comand.Equals("pause"))
            {
                PopupController.GetInstance.OpenPopup(PopupType.pause, false);
                GLobalParametrs.Pause = true;
            }

            if (cmsg.comand.Equals("nopause"))
            {
                PopupController.GetInstance.HidePopup(PopupType.pause, false);
                GLobalParametrs.Pause = false;
            }

            if (cmsg.comand.Equals("surrender"))
            {
                PopupController.GetInstance.OpenPopup(PopupType.SurrenderMessage, false);
                GLobalParametrs.Pause = true;
            }

            if (cmsg.comand.Equals("surrendermess"))
            {
                PopupController.GetInstance.HidePopup(PopupType.SurrenderMessage, cmsg.message.Equals("yes"));
                GLobalParametrs.Pause = false;
                if(cmsg.message.Equals("yes"))
                    PopupController.GetInstance.OpenPopup(PopupType.Lose, true);

            }

            if (cmsg.comand.Equals("draw"))
            {
                PopupController.GetInstance.OpenPopup(PopupType.DrawMessage, false);
                GLobalParametrs.Pause = true;
            }

            if (cmsg.comand.Equals("drawmess"))
            {
                PopupController.GetInstance.HidePopup(PopupType.DrawMessage, cmsg.message.Equals("yes"));
                GLobalParametrs.Pause = false;
                if (cmsg.message.Equals("yes"))
                    PopupController.GetInstance.OpenPopup(PopupType.Win, true);

            }
            if (cmsg.comand.Equals("alphabit"))
            {
                AlfabitBuilder.GetInstance.GetSyncAlphabit(cmsg.message);
            }

        }

        if (cmsg.from.Equals(GLobalParametrs.Player.Name))
        {
            if (cmsg.comand.Equals("turn"))
            {
                if (cmsg.message.Equals("player")) GLobalParametrs.turn = Turn.Player;
                else if (cmsg.message.Equals("anemy")) GLobalParametrs.turn = Turn.Anemy;
            }
        }
    }

    public void SendRoom(string message)
    {
        Debug.Log("io_wb rooms : " + message);
        if (Manager == null) return;
        Manager.Socket.Emit("rooms", message);
    }

    public void SendMessage(string message)
    {
        if (Manager == null) return;
        Debug.Log("io_wb id = " + message);
        Manager.Socket.Emit("message", message);
    }

    private void OnApplicationQuit()
    {
        Disconnect();
    }

    private void OnApplicationPause(bool pause)
    {
        if (Manager == null) return;
        if(!pause)
            Connect();
    }

    private void OnApplicationFocus(bool focus)
    {
        if (Manager == null) return;
        if (focus)
            Connect();
    }
}
