using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// //Damage接口
/// </summary>
public interface IDamageable {
    void TakeHit(float damage, RaycastHit hit);//受击
    void TakeDamage(float damage);//受击
}
