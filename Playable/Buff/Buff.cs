using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit_Buff : MonoBehaviour, Attack
{
    //Interface
    public Unit_Info info;
    public AudioClip attackClip;

    [Header("버프")]
    [SerializeField] float coolTime;
    [SerializeField] float attackUp;

    public void OnEnable()
    {
        Attack_Cycle attack_cycle = new Attack_Cycle(info.search, info.anim, info);
        StartCoroutine(attack_cycle.Cycle());
    }

    float timer = 0;
    void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
        else
        {
            timer = coolTime;

            for (int i = 0; i < 3; i++)
                info.anim[i].Play("Buff");

            
        }
    }

    public void Attack(Vector3 pos, int temp)
    {

    }

    public void Effect(Transform target, int damageUp, int temp)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 1.5f, LayerMask.GetMask("Player"));

        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject.activeSelf && colliders[i].gameObject != gameObject)
            {
                Buff_Manager.inst.Get("Attack_Up", colliders[i].transform.position);
                colliders[i].GetComponent<Unit_Control>().Attack_Up(attackUp, 3f);
            }
        }
    }
}
