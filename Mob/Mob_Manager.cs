using System.Collections;
using TMPro;
using UnityEngine;

public class Mob_Manager : MonoBehaviour
{
    #region 싱글톤
    public static Mob_Manager inst;
    private void Awake() => inst = this;
    #endregion

    [System.Serializable]
    public enum Mob_Type { Mob, Mini, Boss, Box }
    public enum Mob_Move_State { Down, Right, Up, Left }
    public int Limit_Count
    {
        get
        {
            return limit_count;
        }
        set
        {
            limit_count = value;
            limit_count_tmp.text = "/ " + value.ToString();
        }
    }
    int limit_count = 100;
    public int Round
    {
        set
        {
            round = value;

            Round_Count.text = value.ToString();
        }
        get
        {
            return round;
        }
    }
    


    [Header("컴포넌트")]
    public Pool_Manager PM;
    public Pool_Manager TmpPm;
    public TextMeshProUGUI Round_Count;
    public TextMeshProUGUI Unit_Count;

    [Header("설정")]
    public Transform spawn_pos;
    public float spawn_delay;
    public int wave;
    public int round;

    [Header("남은 몬스터 수")]
    [SerializeField] GameObject Unit_Limit_Count;
    [SerializeField] TextMeshProUGUI limit_count_tmp;

    [Header("남은 시간")]
    [SerializeField] GameObject Unit_Timer;
    [SerializeField] TextMeshProUGUI Timer;

    [Header("몬스터 애니메이터")]
    public RuntimeAnimatorController[] mob_anims;

    IEnumerator coroutine;
    bool box_spawn = true;
    public void Game_Start()
    {
        if (box_spawn)
        {
            Box_Manager.inst.Spawn();
            box_spawn = false;
        }

        if (coroutine == null)
        {
            Unit_Limit_Count.SetActive(true);
            Unit_Timer.SetActive(false);

            coroutine = Spawn();
            StartCoroutine(coroutine);
        }
    }

    public void Boss_Clear()
    {
        clear_boss_timer = boss_timer;
    }

    [HideInInspector] public Collider2D[] colliders = new Collider2D[0];
    private void Update()
    {
        colliders = Physics2D.OverlapCircleAll(transform.position, 10, LayerMask.GetMask("Mob"));
        int n = colliders.Length;

        Unit_Count.text = n.ToString();
        if (Unit_Timer.activeSelf)
        {
            Boss_Timer();
        }
        else
        {
            if (n >= Limit_Count - 30)
            {
                Unit_Count.color = Color.red;
                limit_count_tmp.color = Color.red;
            }
            else
            {
                Unit_Count.color = Color.white;
                limit_count_tmp.color = Color.white;
            }

            if (n >= Limit_Count) 
                GameManager.inst.state = GameManager.State.Die;
        }
    }

    IEnumerator Spawn()
    {
        var Spawn_Delay = new WaitForSeconds(spawn_delay);
        var Wave_Delay = new WaitForSeconds(5f);

        while (GameManager.inst.state != GameManager.State.Die)
        {
            if (Round == 100)
            {
                GameManager.inst.state = GameManager.State.Win;
                break;
            }

            Round++;
            wave = WaveMathf(Round);

            if (Round % 10 == 0)
            {
                var obj = PM.Pool.Get();
                obj.transform.position = spawn_pos.position;
                obj.GetComponent<Mob_Control>().SetUp(wave, Mob_Type.Boss);

                Unit_Limit_Count.SetActive(false);
                Unit_Timer.SetActive(true);

                boss_timer = 60;
                break;
            }
            else
            {
                for (int i = 0; i < 30; i++)
                {
                    var obj = PM.Pool.Get();
                    obj.transform.position = spawn_pos.position;
                    obj.GetComponent<Mob_Control>().SetUp(wave, Mob_Type.Mob);

                    yield return Spawn_Delay;
                }
            }
            yield return Wave_Delay;
        }

        coroutine = null;
    }

    int WaveMathf(int n)
    {
        return ((n + (5 * n)) * n);
    }

    #region 보스 타이머
    float boss_timer = 60;
    public float clear_boss_timer = -1;

    public void Boss_Timer()
    {
        boss_timer -= Time.deltaTime;
        Timer.text = (Mathf.Floor(boss_timer * 100f) * 0.01f).ToString("F2");

        if (boss_timer <= 10)
        {
            Timer.color = Color.red;
        }
        else
        {
            Timer.color = Color.white;
        }

        if (boss_timer <= 0)
        {
            Unit_Timer.SetActive(false);

            GameManager.inst.state = GameManager.State.Die;
        }
    }
    #endregion
}
