using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 生命体基类
public class LivingEntity : MonoBehaviour, IDamageable {
    protected float HP;
    protected bool dead;//是否死亡
    public float startHP;

    public virtual void Start() {
        HP = startHP;
    }

    public void TakeHit(float damage, RaycastHit hit) {
        HP -= damage;
        if (HP <= 0 && !dead) {
            Die();
        }
    }

    public void Die() {
        dead = true;
        GameObject.Destroy(gameObject);
    }
}
