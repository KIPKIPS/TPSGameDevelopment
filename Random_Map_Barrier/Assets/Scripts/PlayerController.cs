using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour {
    private Rigidbody rigid;
    private Vector3 moveVelocity;
    void Start() {
        rigid = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update() {

    }

    public void Move(Vector3 velocity) {
        moveVelocity = velocity;
    }

    void FixedUpdate() {
        rigid.MovePosition(rigid.position + moveVelocity * Time.fixedDeltaTime);
    }

    public void LookAt(Vector3 point) {
        transform.LookAt(new Vector3(point.x, this.transform.position.y, point.z));
    }
}
