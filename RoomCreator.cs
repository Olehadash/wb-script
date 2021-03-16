using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomCreator : MonoBehaviour
{
    #region Singlton
    private static RoomCreator instance;

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

    public static RoomCreator GetInstance
    {
        get
        {
            return instance;
        }
    }

    #endregion

    public void CreateRoom()
    {
        ServerController.onSuccessHandler += RoomCreated;
        WWWForm form = new WWWForm();
        form.AddField("Name", GLobalParametrs.Player.Name);
        form.AddField("CreatorName", GLobalParametrs.Player.Name);
        ServerController.PostREquest("createroom", form, false);

        GLobalParametrs.playRoom = new RoomsModel.Room();
        GLobalParametrs.playRoom.Name = GLobalParametrs.Player.Name;
        GLobalParametrs.playRoom.CreatorName = GLobalParametrs.Player.Name;
    }

    public void DeleteRoom()
    {
        ServerController.onSuccessHandler += RoomCreated;
        WWWForm form = new WWWForm();
        form.AddField("Name", GLobalParametrs.Player.Name);
        form.AddField("CreatorName", GLobalParametrs.Player.Name);
        ServerController.PostREquest("deleteroom", form, false);
        GLobalParametrs.playRoom = new RoomsModel.Room();
    }

    void RoomCreated(WWW www)
    {
        ServerController.onSuccessHandler -= RoomCreated;
        WEbSocketController.GetInstance.SendRoom(www.text);
    }

}
