using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : LivingEntity {
    public NavMeshAgent pathFinder;
    private Transform target;
    private int flag;
    protected override void Start() {
        base.Start();
        pathFinder = GetComponent<NavMeshAgent>();//寻路组件
        flag = 0;
    }

    // Update is called once per frame
    void Update() {
        if (target == null) {
            target = GameObject.FindGameObjectWithTag("Player").transform;
        }
        else {
            if (flag == 0) {
                flag = 1;
                StartCoroutine(UpdatePath());//开启寻路协程,防止每一帧都执行寻路
            }
        }
    }

    //节省性能,在协程中执行寻路,不必在每一帧都去计算路径
    IEnumerator UpdatePath() {
        float refreshRate = 0.25f;
        while (target != null) {
            Vector3 targetPosition = new Vector3(target.position.x, 0, target.position.z);
            if (!dead) {
                pathFinder.SetDestination(targetPosition);//寻路,会重算寻路路径
            }
            yield return new WaitForSeconds(refreshRate);
        }
    }
}
