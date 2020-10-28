using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//接口
public interface IDamageable {
    void TakeHit(float damage, RaycastHit hit);//受击
    void TakeDamage(float damage);//受击
}
