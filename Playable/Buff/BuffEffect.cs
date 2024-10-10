using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Effect_Attack_Up
{
    Unit_Info info;

    float num;
    float time;

    public Effect_Attack_Up(Unit_Info info, float num, float time)
    {
        this.info = info;
        this.num = num;
        this.time = time;
    }

    public IEnumerator Timer()
    {
        info.Up_Damage += info.Attack_Damage * num;

        yield return new WaitForSeconds(time);

        info.Up_Damage -= info.Attack_Damage * num;
    }
}

public class Effect_Speed_Up
{
    Unit_Info info;

    float num;
    float time;

    public Effect_Speed_Up(Unit_Info info, float num, float time)
    {
        this.info = info;
        this.num = num;
        this.time = time;
    }

    public IEnumerator Timer()
    {
        info.Attack_Speed += num;

        yield return new WaitForSeconds(time);

        info.Attack_Speed -= num;
    }
}
