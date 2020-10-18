using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : MonoBehaviour {
    public NavMeshAgent pathFinder;
    private Transform target;
    void Start() {
        pathFinder = GetComponent<NavMeshAgent>();
        StartCoroutine(UpdatePath());//开启寻路协程,防止每一帧都执行寻路
    }

    // Update is called once per frame
    void Update() {
        
        
    }

    //节省性能,在协程中执行寻路,不必在每一帧都去计算路径
    IEnumerator UpdatePath() {
        float refreshRate = 0.25f;
        if (target == null) {
            target = GameObject.FindGameObjectWithTag("Player").transform;
        }
        else {
            while (target!=null) {
                Vector3 targetPosition = new Vector3(target.position.x,0,target.position.z);
                pathFinder.SetDestination(targetPosition);//寻路,会重算寻路路径
                yield return new WaitForSeconds(refreshRate);
            }
        }
    }
}
