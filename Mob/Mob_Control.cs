using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mob_Control : MonoBehaviour
{
    public Mob_Manager.Mob_Type type;

    [Header("GetComponet")]
    [SerializeField] Mob_Move move;
    [SerializeField] Pool_Control pool;
    public Animator animator;

    [Header("HP")]
    [SerializeField] Canvas hpCanvas;
    [SerializeField] Image hp_filled;

    public float HP
    {
        set
        {
            if (GameManager.inst.state == GameManager.State.Die) return;

            value = (int)(value *( 1 + RuleManager.inst.plus_damage));
            Mob_Manager.inst.TmpPm.Pool.Get().GetComponent<Damage_Control>().Request_Return(transform.position, (int)value);

            if (0 >= HP + value)
            {
                if(hp != 0) Death();
                hp = 0;
            }
            else if (max_hp >= HP + value) hp = HP + value;
            else hp = max_hp;

            hp_filled.fillAmount = hp / max_hp;
        }
        get
        {
            return hp;
        }
    } float hp; float max_hp;

    void Death()
    {
        if (!gameObject.activeSelf) return;

        switch (type)
        {
            case Mob_Manager.Mob_Type.Mob:
                RuleManager.inst.Ticket += 1;
                RuleManager.inst.Kill_Count++;

                pool.Return_pool();
                break;

            case Mob_Manager.Mob_Type.Mini:
                RuleManager.inst.Ticket += 50;
                RuleManager.inst.Elite_Kill_Count++;
                RuleManager.inst.Ordeal_Count++;

                UI_Mini_Boss.inst.Clear();
                gameObject.SetActive(false);
                break;

            case Mob_Manager.Mob_Type.Boss:
                Mob_Manager.inst.Game_Start();
                Mob_Manager.inst.Boss_Clear();

                RuleManager.inst.Ticket += 50;
                RuleManager.inst.Kill_Count++;

                pool.Return_pool();
                break;

            case Mob_Manager.Mob_Type.Box:
                Box_Manager.inst.Clear(true);
                break;
        }
    }

    public void SetUp(float hp, Mob_Manager.Mob_Type type)
    {
        switch (this.type = type)
        {
            case Mob_Manager.Mob_Type.Mob:
                transform.localScale = Vector3.one;
                animator.GetComponent<SpriteRenderer>().sortingOrder = 5;
                hpCanvas.sortingOrder = 5;
                max_hp = hp;
                break;

            case Mob_Manager.Mob_Type.Mini:
                animator.GetComponent<SpriteRenderer>().sortingOrder = 6;
                hpCanvas.sortingOrder = 6;
                max_hp = hp * 3 + 100f;
                break;

            case Mob_Manager.Mob_Type.Boss:
                transform.localScale = new Vector3(2, 2, 2);
                animator.GetComponent<SpriteRenderer>().sortingOrder = 7;
                hpCanvas.sortingOrder = 7;
                max_hp = Mathf.Pow(hp, (0.003f * Mob_Manager.inst.round) + 1) + 600; 
                break;

            case Mob_Manager.Mob_Type.Box:
                max_hp = hp;
                break;
        }
        this.hp = max_hp;
        hp_filled.fillAmount = 1;

        if (pool != null)
            animator.runtimeAnimatorController = Mob_Manager.inst.mob_anims[Mob_Manager.inst.Round / 20];

        if(type != Mob_Manager.Mob_Type.Box)
            StartCoroutine(move.Move());
    }
}
