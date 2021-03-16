using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoinRoomViewController : MonoBehaviour
{
    [SerializeField]
    private GameObject roomsView;
    [SerializeField]
    private GameObject content;
    [SerializeField]
    private GameObject element;
    [SerializeField]
    private GameObject label;

    private List<RoomViewer> roomViewers = new List<RoomViewer>();

    private void Update()
    {
        if(GLobalParametrs.rooms.rooms.Count > roomViewers.Count) addElement();
        if (GLobalParametrs.rooms.rooms.Count < roomViewers.Count) deleteElement();
        if (GLobalParametrs.rooms.rooms.Count == roomViewers.Count) REsetRoms();
        if (HaveNOAnyFreeRoom())
        {
            roomsView.SetActive(false);
            label.SetActive(true);
        }
    }
    void REsetRoms()
    {
        int len1 = roomViewers.Count;
        for (int i = 0; i < len1; i++)
        {
            roomViewers[i].Room = GLobalParametrs.rooms.rooms[i];
        }
    }

    bool HaveNOAnyFreeRoom()
    {
        int len1 = roomViewers.Count;
        if (len1 == 0) return true;
        for (int i = 0; i < len1; i++)
        {
            if (roomViewers[i].Room.isShown == 0) return false;
        }
        return true;
    }

    void deleteElement()
    {
        int len1 = roomViewers.Count;
        for (int i = 0; i < len1; i++)
        {
            if(compare(roomViewers[i]))
            {
                roomViewers[i].Remove();
                roomViewers.RemoveAt(i);
                break;
            }
        }
    }

    bool compare(RoomViewer view)
    {
        int len1 = GLobalParametrs.rooms.rooms.Count;
        for (int i = 0; i < len1; i++)
        {
            if(GLobalParametrs.rooms.rooms[i].Name.Equals(view.Room.Name))
            {
                return false;
            }
        }
        return true;
    }

    void addElement()
    {
        roomsView.SetActive(true);
        label.SetActive(false);
        Transform el = Instantiate(element.transform, Vector3.zero, Quaternion.identity);
        el.SetParent(content.transform);
        el.localScale = new Vector3(1, 1, 1);
        RoomViewer view = el.gameObject.GetComponent<RoomViewer>();
        view.Room = GLobalParametrs.rooms.rooms[GLobalParametrs.rooms.rooms.Count - 1];
        roomViewers.Add(view);
    }
}
