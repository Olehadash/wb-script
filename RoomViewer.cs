using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RoomViewer : MonoBehaviour
{
    [SerializeField]
    private Text label;
    private RoomsModel.Room room;
    private bool active = false;
    
    public RoomsModel.Room Room
    {
        set
        {
            room = value;
            label.text = room.Name;
            gameObject.SetActive(true);
            active = true;
        }

        get
        {
            return room;
        }
    }
    public void Remove()
    {
        Destroy(gameObject);
    }

    void Update()
    {
        if (!active) return;
        if(room.isShown ==1) gameObject.SetActive(false);
    }

    public void onPressHandler()
    {
        ServerController.onSuccessHandler += RoomConnect;
        WWWForm form = new WWWForm();
        form.AddField("Name", room.Name);
        form.AddField("CreatorName", room.CreatorName);
        form.AddField("ConnectorName", GLobalParametrs.Player.Name);
        GLobalParametrs.playRoom = room;
        ServerController.PostREquest("connectroom", form, false);
    }

    void RoomConnect(WWW www)
    {
        CallMessage msg = new CallMessage();
        msg.from = GLobalParametrs.Player.Name;
        msg.to = room.CreatorName;
        msg.comand = "join";
        msg.message = "";

        string json = JsonUtility.ToJson(msg);
        WEbSocketController.GetInstance.SendMessage(json);
        WEbSocketController.GetInstance.SendRoom(www.text);
        SceneManager.LoadScene(2);
    }
}
