using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlUIController : MonoBehaviour
{
    #region Serializable Fields
    [SerializeField]
    private Slider playerlife;
    [SerializeField]
    private Slider anemylife;
    [SerializeField]
    private Text LifeLabel;
    [SerializeField]
    private Text AnemyLifeLabel;
    [SerializeField]
    private Text WordLabel;
    [SerializeField]
    private Text TranslateLanel;
    [SerializeField]
    private Text PlayerName;
    [SerializeField]
    private Text AnemyName;
    [SerializeField]
    private Text CoinsLabel;
    #endregion
    #region Private Fields
    private int pLife, aLife;
    #endregion
    private void Start()
    {
        GLobalParametrs.Player.Life = 100;
        GLobalParametrs.Anemy.Life = 100;

        pLife = 100;
        aLife = 100;

        PlayerName.text = GLobalParametrs.Player.Name;
        AnemyName.text = GLobalParametrs.Anemy.Name;
    }
    private void Update()
    {
        if(GLobalParametrs.Player.Life < pLife)
        {
            pLife -= 1;
            LifeLabel.text = pLife.ToString();
            playerlife.value = pLife;
        }

        if (GLobalParametrs.Anemy.Life < aLife)
        {
            aLife -= 1;
            AnemyLifeLabel.text = aLife.ToString();
            anemylife.value = aLife;
        }
        if (GLobalParametrs.Presed)
            WordLabel.text = GLobalParametrs.Word;
        TranslateLanel.text = GLobalParametrs.Translate;
    }
}
