using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public class Player : MonoBehaviour {
    public float moveSpeed = 5;
    public Camera viewCamera;
    private PlayerController controller;
    // Start is called before the first frame update
    void Start() {
        controller = GetComponent<PlayerController>();
        viewCamera = Camera.main;
    }

    // Update is called once per frame
    void Update() {
        Vector3 moveInput = new Vector3(Input.GetAxis("Horizontal"),0,Input.GetAxis("Vertical"));
        Vector3 moveVelocity = moveInput.normalized * moveSpeed;
        controller.Move(moveVelocity);

        Ray ray = viewCamera.ScreenPointToRay(Input.mousePosition);//相机指向鼠标的射线
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
        float rayDis;
        if (groundPlane.Raycast(ray,out rayDis)) {
            Vector3 point = ray.GetPoint(rayDis);
            Debug.DrawLine(ray.origin,point,Color.red);
            controller.LookAt(point);
        }
    }
}
