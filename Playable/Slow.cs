using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit_Slow : MonoBehaviour
{
    public void Slow(Collider2D collider2d, float n, float time)
    {
        if (!collider2d.gameObject.activeSelf) return;

        if (collider2d.TryGetComponent(out Mob_Move mob))
        {
            mob.StartCoroutine(Timer(mob, n, time));
        }
    }

    IEnumerator Timer(Mob_Move mob, float n, float time)
    {
        mob.move_speed -= n;
        yield return new WaitForSeconds(time);
        mob.move_speed += n;
    }
}
