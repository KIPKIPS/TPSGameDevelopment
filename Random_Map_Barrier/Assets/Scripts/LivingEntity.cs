using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 生命体基类,持有生命特征的对象
/// </summary>
public class LivingEntity : MonoBehaviour, IDamageable {
    protected float HP;
    protected bool dead;//是否死亡
    public float startHP;
    public event System.Action OnDeath;//生命体死亡事件
    protected virtual void Start() {
        HP = startHP;
    }
    /// <summary>
    /// 生命体受击
    /// </summary>
    /// <param name="damage">伤害数值</param>
    public void TakeDamage(float damage) {
        //print("hit1");
        HP -= damage;
        if (HP <= 0 && !dead) {
            Die();
        }
    }

    /// <summary>
    /// 扩展的生命体受击函数
    /// </summary>
    /// <param name="damage">伤害值</param>
    /// <param name="hit">伤害来源</param>
    public void TakeHit(float damage, RaycastHit hit) {
        //print("hit2");
        TakeDamage(damage);
    }

    /// <summary>
    /// 处理死亡
    /// </summary>
    public void Die() {
        //print("die");
        dead = true;
        if (OnDeath != null) {
            OnDeath();
        }
        GameObject.Destroy(gameObject);
    }
}
