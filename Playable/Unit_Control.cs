using UnityEngine;

public class Unit_Control : MonoBehaviour
{
    public Char_Manager.Rate rate;
    public Char_Manager.State state;

    [Header("컴포넌트")]
    public Unit_Info info;

    [Header("이미지")]
    public GameObject Range;

    public int Count
    {
        get { return count; }
        set
        {
            count = value;

            for (int i = 0; i < 3; i++)
            {
                if (count >= i + 1)
                {
                    info.anim[i].gameObject.SetActive(true);
                }
                else
                {
                    info.anim[i].gameObject.SetActive(false);
                }
            }

            for (int i = 0; i < 3; i++)
            {
                info.anim[i].Rebind();
            }
        }
    } int count;

    public void Drag_Set_Layer()
    {
        for (int i = 0; i < 3; i++)
        {
            info.anim[i].GetComponent<SpriteRenderer>().sortingOrder = 10;
        }

        float r = info.search.radius * 2f;
        Range.transform.localScale = new Vector3(r, r, r);
        Range.SetActive(true);
    }

    public void Drag_Set_Origin(bool bo)
    {
        for (int i = 0; i < 3; i++)
        {
            Color color = new Color();
            switch (bo)
            {
                case true:
                    color = new Color(1, 1, 1, 1);
                    break;
                case false:
                    color = new Color(0, 0, 0, 0);
                    break;
            }

            info.anim[i].GetComponent<SpriteRenderer>().color = color;
        }
    }

    public void Attack_Up(float att, float time)
    {
        Effect_Attack_Up effect = new Effect_Attack_Up(info, att, time);
        StartCoroutine(effect.Timer());
    }

    public void Speed_Up(float slow, float time)
    {
        Effect_Speed_Up effect = new Effect_Speed_Up(info, slow, time);
        StartCoroutine(effect.Timer());
    }
}
