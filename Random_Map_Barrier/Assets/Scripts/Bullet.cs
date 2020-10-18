using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
    public float speed = 10f;
    void Start() {

    }
    public void SetSpeed(float newSpeed) {
        speed = newSpeed;
    }
    // Update is called once per frame
    void Update() {
        transform.Translate(Vector3.forward * Time.deltaTime * speed);
    }
}
