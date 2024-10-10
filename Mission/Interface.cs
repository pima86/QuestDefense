using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public interface IMission
{
    Mission Origin_Mission { get; }

    bool Search();
}

[Serializable]
public class Mission
{
    public GameObject gameObject;

    [Header("Localization")]
    public string title;

    [Header("Units")]
    public UnitUniqueID[] unitID;

    [Header("Option")]
    public string reward;
    public int reward_buy_coin;

    [HideInInspector] 
    public Mission_Page_Control control;
    

    public void Reward()
    {
        control.Clear();

        if (reward.Contains("Ticket")) RuleManager.inst.Ticket += reward_buy_coin;
        else if (reward.Contains("Damage")) RuleManager.inst.plus_damage += (reward_buy_coin * 0.01f);
        else if (reward.Contains("Speed")) RuleManager.inst.minus_speed -= (reward_buy_coin * 0.01f);

        gameObject.SetActive(false);
    }
}
