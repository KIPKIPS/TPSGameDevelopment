using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 生命体基类
public class LivingEntity : MonoBehaviour, IDamageable {
    protected float HP;
    protected bool dead;//是否死亡
    public float startHP;
    public event System.Action OnDeath;//生命体死亡事件
    protected virtual void Start() {
        HP = startHP;
    }
    //生命体受击
    public void TakeDamage(float damage) {
        //print("hit1");
        HP -= damage;
        if (HP <= 0 && !dead) {
            Die();
        }
    }

    //扩展的生命体受击
    public void TakeHit(float damage, RaycastHit hit) {
        //print("hit2");
        TakeDamage(damage);
    }

    public void Die() {
        //print("die");
        dead = true;
        if (OnDeath != null) {
            OnDeath();
        }
        GameObject.Destroy(gameObject);
    }
}
