using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonPressController : MonoBehaviour
{
    public void OnCallMenuButtonPress()
    {
        GLobalParametrs.Pause = true;
        PopupController.GetInstance.OpenPopup(PopupType.Menu, false);
        CallMessage msg = new CallMessage();
        msg.from = GLobalParametrs.Player.Name;
        msg.to = GLobalParametrs.Anemy.Name;
        msg.comand = "pause";
        msg.message = "";
        WEbSocketController.GetInstance.SendMessage(JsonUtility.ToJson(msg));
    }
    public void onHideMenuButtonPress(bool isHideParent)
    {
        PopupController.GetInstance.HidePopup(PopupType.Menu, isHideParent);
        
    }

    public void onSurrenderButtonPres()
    {
        PopupController.GetInstance.OpenPopup(PopupType.Surrender, false);
        
    }
    public void onSurrederAccept(bool accept)
    {
        if (accept)
        {
            CallMessage msg = new CallMessage();
            msg.from = GLobalParametrs.Player.Name;
            msg.to = GLobalParametrs.Anemy.Name;
            msg.comand = "surrender";
            msg.message = "";
            WEbSocketController.GetInstance.SendMessage(JsonUtility.ToJson(msg));
        }
        PopupController.GetInstance.HidePopup(PopupType.Surrender, accept);
        if (accept) PopupController.GetInstance.OpenPopup(PopupType.wayt, false);
    }

    public void onSurrenderAnswer(bool answer)
    {
        CallMessage msg = new CallMessage();
        msg.from = GLobalParametrs.Player.Name;
        msg.to = GLobalParametrs.Anemy.Name;
        msg.comand = "surrendermess";
        msg.message = answer ? "yes" : "no";
        WEbSocketController.GetInstance.SendMessage(JsonUtility.ToJson(msg));
        PopupController.GetInstance.HidePopup(PopupType.SurrenderMessage, false);
        if (answer) PopupController.GetInstance.OpenPopup(PopupType.Win, true);


    }

    public void onDrawnButtonPress()
    {
        PopupController.GetInstance.OpenPopup(PopupType.Draw, false);
    }

    public void onDrawAccept(bool accept)
    {
        if(accept)
        {
            CallMessage msg = new CallMessage();
            msg.from = GLobalParametrs.Player.Name;
            msg.to = GLobalParametrs.Anemy.Name;
            msg.comand = "draw";
            msg.message = "";
            WEbSocketController.GetInstance.SendMessage(JsonUtility.ToJson(msg));
        }
        PopupController.GetInstance.HidePopup(PopupType.Draw, accept);
        if (accept) PopupController.GetInstance.OpenPopup(PopupType.wayt, false);
    }

    public void onDrawAnswer(bool answer)
    {
        CallMessage msg = new CallMessage();
        msg.from = GLobalParametrs.Player.Name;
        msg.to = GLobalParametrs.Anemy.Name;
        msg.comand = "drawmess";
        msg.message = answer ? "yes" : "no";
        WEbSocketController.GetInstance.SendMessage(JsonUtility.ToJson(msg));
        PopupController.GetInstance.HidePopup(PopupType.DrawMessage, false);
        if (answer) PopupController.GetInstance.OpenPopup(PopupType.Win, true);
    }

    public void OnAudioOfOnPress()
    {

    }

    public void onSoundOnOffButtonPress()
    {

    }

    public void onQuestionButtonPress()
    {

    }
}
