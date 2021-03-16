using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class ButtonViewController : MonoBehaviour
{
    #region Serialize Fields
    [SerializeField]
    private Image image;
    [SerializeField]
    private Text text;
    [SerializeField]
    private GameObject lightning;
    [SerializeField]
    private GameObject lightning2;
    [Space(10)]
    [SerializeField]
    private Sprite pick, unpuck, apick;
    #endregion
    #region Private Fields
    private int xid = 0, yid = 0;
    private int id;
    private string letter = "";
    private bool ISpicked = false;
    #endregion
    #region Setters
    public void setPick(bool seter)
    {
        this.ISpicked = seter;
    }

    public void setCoord(int x, int y)
    {
        xid = x;
        yid = y;
    }

    public void setId(int _id)
    {
        this.id = _id;
    }

    public void setText(string _letter)
    {
        text.text = _letter;
        this.letter = _letter;
    }
    #endregion
    #region Getters
    public int getId()
    {
        return this.id;
    }
    public int getxpos
    {
        get { return this.xid; }
    }
    public int getypos
    {
        get { return this.yid; }
    }
    public string getLetter()
    {

        return text.text;
    }
    public bool getPick()
    {
        return ISpicked;
    }
    #endregion
    #region View Controll Methods
    public void bluespotActive(bool active)
    {
        if(GLobalParametrs.turn == Turn.Player)
            image.sprite = active ? pick : unpuck ;
        else
            image.sprite = active ? apick : unpuck;
        //Debug.Log("OnPointerEnter "+ active+ " : " + image.sprite.name);
    }

    public void lightningActivate(bool active)
    {
        lightning.SetActive(active);
    }
    public void LightRotation(float rotation)
    {
        lightning.transform.Rotate(0.0f, 0.0f, rotation, Space.Self);
    }

    public void lightningActivate1(bool active)
    {
        lightning2.SetActive(active);
    }
    public void LightRotation1(float rotation)
    {
        lightning2.transform.Rotate(0.0f, 0.0f, rotation, Space.Self);
    }
    public void DenayRotation()
    {
        lightning.transform.rotation = Quaternion.identity;
    }
    public void DenayRotation1()
    {
        lightning2.transform.rotation = Quaternion.identity;
    }
    #endregion
    #region Event Handlers
    public void OnPointerEnter()
    {
        if (GLobalParametrs.turn == Turn.Anemy) return;
        if (!GLobalParametrs.Presed) return;
        SetHAndlerActive();
        GamePLayController.GetInstance.SetWord();
    }

    public void OnClickHandler()
    {
        if (GLobalParametrs.turn == Turn.Anemy) return;
        GLobalParametrs.Presed = true;
        SetHAndlerActive();
        GamePLayController.GetInstance.SetWord();
    }   

    public void SelfActivate()
    {
        LightningActivate();
        GLobalParametrs.butContainer.Add(this);
        bluespotActive(true);
    }
    public void SelfDeavtive()
    {
        setPick(false);
        bluespotActive(false);
        lightningActivate(false);
        lightningActivate1(false);
        DenayRotation();
        DenayRotation1();
    }
    
    void SetHAndlerActive()
    {
        if (!ISpicked)
        {
            ISpicked = true;
            LightningActivate();
            GLobalParametrs.butContainer.Add(this);
            bluespotActive(ISpicked);
            
        }
        else
        {
            int len = GLobalParametrs.butContainer.Count;
            if (len - 2 < 0) return;
            if (GLobalParametrs.butContainer[len - 2] == this)
            {
                GLobalParametrs.butContainer[len - 1].setPick(false);
                GLobalParametrs.butContainer[len - 1].bluespotActive(false);
                GLobalParametrs.butContainer[len - 1].lightningActivate(false);
                GLobalParametrs.butContainer[len - 1].lightningActivate1(false);
                GLobalParametrs.butContainer[len - 1].DenayRotation();
                GLobalParametrs.butContainer[len - 1].DenayRotation1();
                GLobalParametrs.butContainer.RemoveAt(len - 1);
            }
        }
    }

    void LightningActivate()
    {
        int len = GLobalParametrs.butContainer.Count;
        if(len>0)
        {
            GLobalParametrs.butContainer[len - 1].lightningActivate1(true);
            GLobalParametrs.butContainer[len - 1].LightRotation1(
                GEtRotation(GLobalParametrs.butContainer[len - 1].getxpos, GLobalParametrs.butContainer[len - 1].getypos)
            );
            lightningActivate(true);
            LightRotation(
                GEtRotation2(GLobalParametrs.butContainer[len - 1].getxpos, GLobalParametrs.butContainer[len - 1].getypos)
            );

        }
    }

    float GEtRotation( int xpos, int ypos)
    {
        int x = this.xid - xpos;
        int y = this.yid - ypos;
        if (x == -1 && y == -1)
            return 45 * 6;
        else if (x == -1 && y == 0)
            return 45 * 5;
        else if (x == -1 && y == 1)
            return 45 * 4;
        else if (x == 0 && y == 1)
            return 45 * 3;
        else if (x == 1 && y == 1)
            return 45 * 2;
        else if (x == 1 && y == 0)
            return 45 * 1;
        else if (x == 1 && y == -1)
            return 45 * 0;
        else if (x == 0 && y == -1)
            return 45 * 7;
        return 0;
    }

    float GEtRotation2(int xpos, int ypos)
    {
        int x = this.xid - xpos;
        int y = this.yid - ypos;
        if (x == -1 && y == -1)
            return 45 * 2;
        else if (x == -1 && y == 0)
            return 45 * 1;
        else if (x == -1 && y == 1)
            return 45 * 0;
        else if (x == 0 && y == 1)
            return 45 * 7;
        else if (x == 1 && y == 1)
            return 45 * 6;
        else if (x == 1 && y == 0)
            return 45 * 5;
        else if (x == 1 && y == -1)
            return 45 * 4;
        else if (x == 0 && y == -1)
            return 45 * 3;
        return 0;
    }

    public void ResetParametrs()
    {
        bluespotActive(false);
        lightningActivate(false);
        lightningActivate1(false);
        DenayRotation();
        DenayRotation1();
        ISpicked = false;
        
    }
    #endregion

    

    private void Update()
    {
        text.text = AlfabitBuilder.GetInstance.GetLetter(this.xid, this.yid);

        
    }
}
