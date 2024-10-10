using System;
using UnityEngine;

public class Unit_DefaultAttack : MonoBehaviour, Attack
{
    //Interface
    public Unit_Info info;
    public AudioClip attackClip;

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

        Collider2D collider2D = info.search.GetTarget();
        if (collider2D == null) return;

        Char_Manager.inst.Get_Effect(info.Attack_Effect, collider2D.transform.position);
        if (collider2D != null)
            collider2D.GetComponent<Mob_Control>().HP = -(info.Attack_Damage + info.Up_Damage) * damageUp;
    }
}
