using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum PopupType
{
    None,
    Menu,
    Switch,
    Win,
    Lose,
    Turn,
    Surrender,
    SurrenderMessage,
    Draw,
    DrawMessage,
    pause,
    wayt
}

public class PopupController : MonoBehaviour
{
    #region Singltone
    private static PopupController instance;
    private void Awake()
    {
        if (instance == null)
            instance = this;
    }
    public static PopupController GetInstance
    {
        get
        {
            return instance;
        }
    }

    private void OnDestroy()
    {
        instance = null;
    }
    #endregion
    #region Serializable Field
    [SerializeField]
    private GameObject menu;
    [SerializeField]
    private GameObject switcher;
    [SerializeField]
    private GameObject youwin;
    [SerializeField]
    private GameObject youlose;
    [SerializeField]
    private GameObject popupui;
    [SerializeField]
    private GameObject turnPopup;
    [SerializeField]
    private GameObject surrender;
    [SerializeField]
    private GameObject surrendermessage;
    [SerializeField]
    private GameObject draw;
    [SerializeField]
    private GameObject drawmessage;
    [SerializeField]
    private GameObject pause;
    [SerializeField]
    private GameObject wayt;
    #endregion
    #region Private field
    public PopupType type = PopupType.None;
    #endregion
    public void OpenPopup(PopupType t, bool hideafter)
    {
        
        
        popupui.SetActive(true);

        StartCoroutine(ShowPopup( hideafter, t));
    }

    IEnumerator ShowPopup( bool hideafter, PopupType t)
    {
        
        while (type != PopupType.None) yield return null;
        this.type = t;
        GameObject popup = GetUi();
        if (popup != null)
        {
            popup.SetActive(true);
            CanvasGroup group = popup.GetComponent<CanvasGroup>();
            group.alpha = 0;
            while(group.alpha <1)
            {
                group.alpha += 0.1f;
                yield return null;
            }
            if (hideafter)
            {
                yield return new WaitForSeconds(3);
                StartCoroutine(Hide(true));
            }
        }
    }

    public void HidePopup(PopupType t, bool noParent)
    {
        this.type = t;
        StartCoroutine(Hide(noParent));
    }

    IEnumerator Hide(bool noParent)
    {
        yield return null;
        GameObject popup = GetUi();
        if (popup != null)
        {
            CanvasGroup group = popup.GetComponent<CanvasGroup>();
            group.alpha = 1;
            while(group.alpha >0)
            {
                group.alpha -= 0.1f;
                yield return null;
            }
            popup.SetActive(false);
            type = PopupType.None;
            popupui.SetActive(noParent);
            GLobalParametrs.Presed = noParent;
            if (!noParent)
            {
                CallMessage msg = new CallMessage();
                msg.from = GLobalParametrs.Player.Name;
                msg.to = GLobalParametrs.Anemy.Name;
                msg.comand = "nopause";
                msg.message = "";
                WEbSocketController.GetInstance.SendMessage(JsonUtility.ToJson(msg));
            }
        }
    }
    GameObject GetUi()
    {
        switch(type)
        {
            case PopupType.Menu:
                return menu;
                break;
            case PopupType.Switch:
                return switcher;
                break;
            case PopupType.Win:
                return youwin;
                break;
            case PopupType.Lose:
                return youlose;
                break;
            case PopupType.Turn:
                return turnPopup;
                break;
            case PopupType.Surrender:
                return surrender;
                break;
            case PopupType.SurrenderMessage:
                return surrendermessage;
                break;
            case PopupType.Draw:
                return draw;
                break;
            case PopupType.DrawMessage:
                return drawmessage;
                break;
            case PopupType.pause:
                return pause;
                break;
            case PopupType.wayt:
                return wayt;
                break;

        }
        return null;
    }
}
