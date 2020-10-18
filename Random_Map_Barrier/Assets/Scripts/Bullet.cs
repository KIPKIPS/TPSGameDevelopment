using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
    public float speed = 10f;
    public LayerMask collisionMask;//检测碰撞的layer
    void Start() {

    }
    public void SetSpeed(float newSpeed) {
        speed = newSpeed;
    }
    // Update is called once per frame
    void Update() {
        float moveDistance = Time.deltaTime * speed;
        CheckCollisions(moveDistance);
        transform.Translate(Vector3.forward * moveDistance);
    }

    void CheckCollisions(float moveDistance) {
        Ray ray = new Ray(transform.position,transform.forward);
        RaycastHit hit;
        //与trigger的碰撞也要触发
        if (Physics.Raycast(ray,out hit, moveDistance, collisionMask,QueryTriggerInteraction.Collide)) {
            OnHitObject(hit);
        }
    }

    void OnHitObject(RaycastHit hit) {
        //print(hit.transform.gameObject.name);
        GameObject.Destroy(this.gameObject);
    }
}
