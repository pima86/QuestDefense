using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Unit_Object
{
    Search_target Search { get; }
    Animator[] Anim { get; set; }
}

public interface Attack
{
    void Attack(Vector3 pos, int temp);
    
    void Effect(Transform target, int damageUp, int temp);
}
