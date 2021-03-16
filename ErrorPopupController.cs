using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ErrorPopupController : MonoBehaviour
{
    #region Serializable Field
    [SerializeField]
    private GameObject popups;
    [SerializeField]
    private GameObject loginpopup;
    [SerializeField]
    private GameObject wayitpopup;
    #endregion
    
    public void LoginErrorPopup()
    {
        popups.SetActive(true);
        loginpopup.SetActive(true);
    }

    public void WaytForConnectPopup()
    {
        popups.SetActive(true);
        wayitpopup.SetActive(true);
    }

    public void onRetryPress()
    {
        popups.SetActive(false);
        loginpopup.SetActive(false);
    }

    public void onDenayConnection()
    {
        RoomCreator.GetInstance.DeleteRoom();
        popups.SetActive(false);
        wayitpopup.SetActive(false);
    }

}
