using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FiedController : MonoBehaviour
{
    #region Singlton
    private static FiedController instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private static bool isNullreference
    {
        get
        {
            return instance == null;
        }
    }

    public static FiedController GetInstance
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
    [SerializeField]
    private GameObject[] fields = new GameObject[25];

    private ButtonViewController[,] buttons = new ButtonViewController[5, 5];

    private void Start()
    {
        int i = 0, j = 0, k = 0;

        for(i = 0; i<5; i++)
        {
            for ( j = 0; j < 5; j++)
            {
                buttons[i, j] = fields[k].GetComponent<ButtonViewController>();
                buttons[i, j].setId(k);
                buttons[i, j].setCoord(i, j);
                k++;
            }
        }
    }

    private void Update()
    {
        if(Input.GetMouseButtonUp(0) && GLobalParametrs.turn == Turn.Player)
        {
            GLobalParametrs.Presed = false;
            RestAllButtons();
        }
    }

    public void RestAllButtons()
    {
        int i = 0, j = 0;
        for (i = 0; i < 5; i++)
            for (j = 0; j < 5; j++)
                buttons[i, j].ResetParametrs();

        GamePLayController.GetInstance.onFinishStep();

        GLobalParametrs.butContainer.Clear();
    }

    public void AnemyVisulizeFromIO()
    {
        GLobalParametrs.butContainer.Clear();
        int i = 0, j = 0;
        for (i = 0; i < 5; i++)
            for (j = 0; j < 5; j++)
                buttons[i, j].ResetParametrs();
        GLobalParametrs.Word = "";
        
        for (i = 0; i < GLobalParametrs.gmessage.message.Count; i++)
        {
            buttons[GLobalParametrs.gmessage.message[i].x, GLobalParametrs.gmessage.message[i].y].SelfActivate();
            GLobalParametrs.Word += buttons[GLobalParametrs.gmessage.message[i].x, GLobalParametrs.gmessage.message[i].y].getLetter();

        }
    }
}
