using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour {
    public Transform fireTrs;//枪口位置
    public Bullet bullet;
    public float msBetweenShots = 100;//射击间隔
    public float fireSpeed = 35;//射击速度

    private float nextShotTime;
    void Start() {
        bullet = Resources.Load<GameObject>("Prefabs/Bullet").GetComponent<Bullet>();
    }

    // Update is called once per frame
    void Update() {

    }

    /// <summary>
    /// 触发射击
    /// </summary>
    public void Shoot() {
        if (Time.time > nextShotTime) {
            nextShotTime = Time.time + msBetweenShots / 1000;
            Bullet newBullet = Instantiate(bullet, fireTrs.position, fireTrs.rotation) as Bullet;
            newBullet.SetSpeed(fireSpeed);
        }
    }
}
