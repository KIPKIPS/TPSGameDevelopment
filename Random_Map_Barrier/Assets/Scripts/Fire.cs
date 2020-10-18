using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour {
    public float speed = 10f;

    public void SetSpeed(float newSpeed) {
        speed = newSpeed;
    }

    void Start() {

    }

    // Update is called once per frame
    void Update() {
        transform.Translate(Vector3.forward * Time.deltaTime *speed);
    }
}
