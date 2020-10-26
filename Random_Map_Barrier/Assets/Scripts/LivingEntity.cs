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
    public void TakeHit(float damage, RaycastHit hit) {
        HP -= damage;
        if (HP <= 0 && !dead) {
            Die();
        }
    }

    public void Die() {
        dead = true;
        if (OnDeath != null) {
            OnDeath();
        }
        GameObject.Destroy(gameObject);
    }
}
