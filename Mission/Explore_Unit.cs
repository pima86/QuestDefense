using UnityEngine;

public class Explore_Unit : MonoBehaviour, IMission
{
    public Mission Origin_Mission => mission;
    public Mission mission;

    [Header("Option")]
    public string unit_names;
    public int count;
    public bool bo;

    public bool Search()
    {
        if(bo) return true;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 10, LayerMask.GetMask("Player"));
        if(colliders.Length == 0) return false;

        int have = 0;
        foreach (Collider2D collider in colliders)
        {
            if (unit_names == collider.name) { have += collider.GetComponent<Unit_Control>().Count; }
        }

        if (have >= count)
        {
            bo = true;
            return true;
        }
        return false;
    }
}
