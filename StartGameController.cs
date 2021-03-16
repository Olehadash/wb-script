using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartGameController : MonoBehaviour
{
    #region Serializable Field
    [SerializeField]
    private GameObject JoinUser;
    [SerializeField]
    private GameObject GamePopup;
    [SerializeField]
    private GameObject ControlButtons;
    [SerializeField]
    private GameObject JoinGame;
    [SerializeField]
    private InputField username;
    [SerializeField]
    private ErrorPopupController popups;
    #endregion


    void Start()
    {
        if(PlayerPrefs.HasKey("user"))
        {
            GLobalParametrs.Player.Name = PlayerPrefs.GetString("user");
            JoinUser.SetActive(false);
            GamePopup.SetActive(true);
            ControlButtons.SetActive(true);
        }
    }
    #region Login
    public void OnChangeUserName()
    {
        GLobalParametrs.Player.Name = username.text;
    }

    public void OnUserJoinHandler()
    {
        ServerController.onSuccessHandler += onSuccess;
        WWWForm form = new WWWForm();
        form.AddField("Name", GLobalParametrs.Player.Name);
        form.AddField("DeviceID", SystemInfo.deviceUniqueIdentifier);
        ServerController.PostREquest("login", form, false);
    }

    void onSuccess(WWW www)
    {
        ServerController.onSuccessHandler -= onSuccess;
        if(www.text.Equals("USER CREATED") || www.text.Equals("USER ENTERED"))
        {
            PlayerPrefs.SetString("user", GLobalParametrs.Player.Name);
            StartCoroutine("GotoGameSet");
        }
        else
        {
            popups.LoginErrorPopup();
        }
    }

    IEnumerator GotoGameSet()
    {
        CanvasGroup canva = JoinUser.GetComponent<CanvasGroup>();
        while (canva.alpha>0)
        {
            canva.alpha -= .05f;
            yield return null;
        }
        JoinUser.SetActive(false);
        GamePopup.SetActive(true);
        ControlButtons.SetActive(true);
        canva = GamePopup.GetComponent<CanvasGroup>();
        canva.alpha = 0;
        CanvasGroup canva1 = ControlButtons.GetComponent<CanvasGroup>();
        canva1.alpha = 0;
        while (canva.alpha <1)
        {
            canva.alpha += .05f;
            canva1.alpha += .05f;
            yield return null;
        }
    }
    #endregion
    #region GameOption
    public void onCreateRoomHandler()
    {
        RoomCreator.GetInstance.CreateRoom();
        popups.WaytForConnectPopup();
    }

    public void onJoinButtonHandler()
    {
        GamePopup.SetActive(false);
        JoinGame.SetActive(true);
    }

    public void onDenayJoinGameHandler()
    {
        GamePopup.SetActive(true);
        JoinGame.SetActive(false);
    }
    #endregion
}
