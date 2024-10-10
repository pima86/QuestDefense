using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit_RangeAttack : MonoBehaviour, Attack
{
    //Interface
    public Unit_Info info;
    public AudioClip attackClip;

    [Header("범위")]
    [SerializeField] float attackRange;

    [Header("슬로우")]
    [SerializeField] bool isSlow;
    [SerializeField] Unit_Slow unitSlow;
    [SerializeField] float slow;
    [SerializeField] float slowTime;

    public void OnEnable()
    {
        Attack_Cycle attack_cycle = new Attack_Cycle(info.search, info.anim, info);
        StartCoroutine(attack_cycle.Cycle());
    }

    public void Attack(Vector3 pos, int temp)
    {

    }

    public void Effect(Transform target, int damageUp, int temp)
    {
        SoundManager.Inst.SFXPlay(attackClip);

        Collider2D[] collider2D = info.search.GetTargets(transform.position, attackRange);
        if (collider2D.Length == 0) return;

        for (int i = 0; i < collider2D.Length; i++)
        {
            Char_Manager.inst.Get_Effect(info.Attack_Effect, collider2D[i].transform.position);

            if (collider2D[i] != null)
                collider2D[i].GetComponent<Mob_Control>().HP = -(info.Attack_Damage + info.Up_Damage) * damageUp;

            if (temp == 2)
            {
                if (isSlow)
                {
                    if (collider2D[i] != null)
                        unitSlow.Slow(collider2D[i], slow, slowTime);
                }
            }
        }
    }
}
