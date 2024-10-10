using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    #region 싱글톤
    public static GameManager inst;
    #endregion

    public enum State { Main, Wave, Die, Win };
    public State state
    {
        set 
        {
            if(st == value) return;

            st = value;

            switch (value)
            {
                case State.Wave:
                    Mob_Manager.inst.Game_Start();
                    ui_ad.SetActive(false);
                    ui_buy.SetActive(true);
                    break;
                
                case State.Die:
                    SoundManager.Inst.SFXPlay(SoundManager.Inst.Fail, 0.15f);

                    Char_Manager.inst.End_Sell_And_Buy();
                    ui_end.Game_End();
                    break;

                case State.Win:
                    SoundManager.Inst.SFXPlay(SoundManager.Inst.Victory, 0.15f);

                    ui_end.Game_End(true);
                    break;
            }
        }
        get { return st; }
    }public State st;

    [Header("UI")]
    [SerializeField] GameObject ui_buy;
    [SerializeField] GameObject ui_ad;
    [SerializeField] UI_End ui_end;

    [Header("구매")]
    public bool BuyAdsBlocking;

    void Awake()
    {
        inst = this;
        Application.targetFrameRate = 60;
    }
}
