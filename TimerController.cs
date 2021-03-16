using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public delegate void TimerEndDelegate();

public class TimerController : MonoBehaviour
{
    #region Singltone
    public static TimerController instance;
    private void Awake()
    {
        if (instance == null)
            instance = this;
    }
    public static TimerController GetInstance
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
    #region Serialized fields
    [SerializeField]
    private Text label;
    public TimerEndDelegate onEndTimer;
    #endregion
    #region Private fields
    private int minute = 5, seconds = 0, durSec = 5*60;
    private Color labelColor;
    #endregion

    private void Start()
    {
        labelColor = label.color;
        //StartTimer();
    }

    public void StartTimer()
    {
        label.color = labelColor;
        durSec = 5 * 60;
        minute = 5;
        seconds = 0;
        StartCoroutine(StartTimerCorutine());
    }

    IEnumerator StartTimerCorutine()
    {
        while (durSec >0)
        {
            if (!GLobalParametrs.Pause)
            {
                label.text = SetTimer();
                if (seconds == 0)
                {
                    seconds = 60;
                    minute--;
                }
                seconds--;
                durSec--;
                if (minute == 0 && seconds <= 29)
                {
                    label.color = Color.red;
                }
                //Debug.Log(label.text);
                yield return new WaitForSeconds(1);
            }
            else
            {
                yield return null;
            }
        }
        if(onEndTimer !=null)
        {
            onEndTimer();
        }
    }

    public void PauseTimer()
    {
        StopAllCoroutines();
    }

    public void StopTimer()
    {
        StopAllCoroutines();
        if (onEndTimer != null)
        {
            onEndTimer();
        }
    }

    string SetTimer()
    {
        string minuteStr = minute.ToString();
        string seconsStr = seconds.ToString();
        if(seconds <10)
            seconsStr = "0"+ seconds.ToString();
        string line = "";
        line += "0" + minuteStr + ":" + seconsStr;
        return line;
    }

}
