using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
    public float speed = 10f;
    public LayerMask collisionMask;//检测碰撞的layer
    private float damage = 1;
    private float existTime = 3.0f;//在场景保存的时间

    private float skinWidth = 0;
    void Start() {
        Destroy(gameObject, existTime);
    }
    /// <summary>
    /// 设置子弹速度
    /// </summary>
    /// <param name="newSpeed">速度大小</param>
    public void SetSpeed(float newSpeed) {
        speed = newSpeed;
        Collider[] nearColliders = Physics.OverlapSphere(transform.position, 0.2f, collisionMask);//检测附近有无敌人
        if (nearColliders.Length > 0) {
            OnHitObject(nearColliders[0]);//damage 第一个(最近的敌人)
        }
    }
    // Update is called once per frame
    void Update() {
        float moveDistance = Time.deltaTime * speed;
        CheckCollisions(moveDistance);
        transform.Translate(Vector3.forward * moveDistance);
    }

    /// <summary>
    /// 检测碰撞
    /// </summary>
    /// <param name="moveDistance">检测距离</param>
    void CheckCollisions(float moveDistance) {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        //与trigger的碰撞也要触发,加上QueryTriggerInteraction.Collide
        if (Physics.Raycast(ray, out hit, moveDistance + skinWidth, collisionMask, QueryTriggerInteraction.Collide)) {
            OnHitObject(hit);
        }
    }

    /// <summary>
    /// 碰撞触发
    /// </summary>
    /// <param name="hit">击中的目标</param>
    void OnHitObject(RaycastHit hit) {
        //print(hit.transform.gameObject.name);
        IDamageable damageObject = hit.collider.GetComponent<IDamageable>();
        if (damageObject != null) {
            //print("hit");
            damageObject.TakeHit(damage, hit);
        }
        GameObject.Destroy(this.gameObject);
    }

    /// <summary>
    /// 重写碰撞触发方法
    /// </summary>
    /// <param name="collider"></param>
    void OnHitObject(Collider collider) {
        IDamageable damageObject = collider.GetComponent<IDamageable>();
        if (damageObject != null) {
            damageObject.TakeDamage(damage);
        }
        GameObject.Destroy(this.gameObject);
    }
}
