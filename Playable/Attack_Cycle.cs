using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack_Cycle
{
    Search_target search;
    Animator[] animators;
    Unit_Info info;

    public Attack_Cycle(Search_target search, Animator[] animators, Unit_Info info)
    {
        this.search = search;
        this.animators = animators;
        this.info = info;
    }

    public IEnumerator Cycle()
    {
        while (true)
        {
            if (GameManager.inst.state == GameManager.State.Die)
            {
                for(int i = 0; i < animators.Length; i++)
                {
                    if (animators[i].gameObject.activeSelf)
                        animators[i].Play("Die");
                }

                break;
            }

            if (search.short_enemy == null)
                Animation_Attack(false);
            else 
                Animation_Attack(true);

            yield return new WaitForSeconds(0.1f);
        }
    }

    public void Animation_Attack(bool bo)
    {
        for (int i = 0; i < animators.Length; i++)
        {
            if (animators[i].gameObject.activeSelf)
            {
                animators[i].SetBool("isAttack", bo);
                animators[i].SetFloat("AttackSpeed", info.Attack_Speed);
            }
        }
    }
}
