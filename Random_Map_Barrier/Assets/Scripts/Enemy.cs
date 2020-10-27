using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : LivingEntity {
    public enum State {
        Idle,//默认
        Chase,//追逐目标
        Attack,//攻击
    }
    private State curState;
    public NavMeshAgent pathFinder;//寻路组件
    private Transform target;//寻路目标
    //private int flag;//标志,控制协程只执行一次
    public float attackDis = 0.5f;//攻击距离
    private Material mat;
    private Color oriColor;

    public float timeBetweenAttack = 1;//两次攻击的间隔时间
    private float nextAttackTime = 0;

    private float targetRadius;
    private float selfRadius;

    protected override void Start() {
        base.Start();
        pathFinder = GetComponent<NavMeshAgent>();//寻路组件
        curState = State.Chase;//默认追赶目标
        mat = GetComponent<Renderer>().material;
        oriColor = mat.color;
        //flag = 0;
        target = GameObject.FindGameObjectWithTag("Player").transform;
        targetRadius = target.GetComponent<CapsuleCollider>().radius;//获取半径
        selfRadius = this.GetComponent<CapsuleCollider>().radius;
        StartCoroutine(UpdatePath());//开启寻路协程,防止每一帧都执行寻路
    }

    // Update is called once per frame
    void Update() {
        //当前时间大于下一次攻击时间
        if (Time.time > nextAttackTime) {
            //Vector3.Distance(target.transform.position,transform.position)
            float sqrtDis = (target.transform.position - transform.position).sqrMagnitude;//与目标距离的平方
            if (sqrtDis < Mathf.Pow(attackDis +selfRadius+targetRadius, 2)) {
                nextAttackTime = Time.time + timeBetweenAttack;//为下一次攻击时间赋值,now + 攻击间隔
                //开启攻击协程
                StartCoroutine(Attack());
            }
        }
    }
    //攻击协程
    IEnumerator Attack() {
        curState = State.Attack;
        //进攻期间关闭寻路
        pathFinder.enabled = false;
        float attackSpeed = 3;
        Vector3 oriPos = transform.position;
        Vector3 tarPos = target.transform.position;
        float percent = 0;
        while (percent <= 1) {
            mat.color = Color.black;

            percent += Time.deltaTime * attackSpeed;
            Vector3 dirToTarget = (tarPos - oriPos).normalized;
            float parabola = (-percent * percent + percent) * 4; //抛物线方程 y=-4X² + 4X
            transform.position = Vector3.Lerp(oriPos,tarPos - dirToTarget*targetRadius, parabola); //插值,攻击敌人后又会返回初始位置
            yield return null;
        }
        //回到初始位置后开启寻路
        pathFinder.enabled = true;
        curState = State.Chase;

        mat.color = oriColor;
    }

    //节省性能,在协程中执行寻路,不必在每一帧都去计算路径
    IEnumerator UpdatePath() {
        float refreshRate = 0.25f;
        while (target != null) {
            if (curState == State.Chase) {
                Vector3 dirToTarget = (target.position - transform.position).normalized;//计算目标和自身的方向向量,由自身指向目标
                //防止穿模,接触到target时停下来
                Vector3 targetPosition = target.position - dirToTarget * (selfRadius + targetRadius + attackDis/2);//目标位置减去方向向量乘半径和
                if (!dead) {
                    pathFinder.SetDestination(targetPosition);//寻路,会重算寻路路径
                }
            }
            yield return new WaitForSeconds(refreshRate);
        }
    }
}
