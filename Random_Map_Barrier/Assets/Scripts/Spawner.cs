//敌人AI制造机
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {
    public Wave[] waves;
    private Wave curWave;//当前波
    private int curWaveNum;//当前波数的数量
    public Enemy enemy;
    public int remainEnemiesToSpawn;//剩余需要创建的敌人
    private float nextSpawnTime;

    private int aliveEnemies;//剩余存活的敌人
    //public event System.Action<int> OnNewWave;
    void Start() {
        enemy = Resources.Load<GameObject>("Prefabs/Enemy").GetComponent<Enemy>();
        NextWave();
    }

    void Update() {
        if (remainEnemiesToSpawn > 0 && Time.time > nextSpawnTime) {
            remainEnemiesToSpawn--;
            nextSpawnTime = Time.time + curWave.timeBetweenSpawns;
            Enemy spawnEnemy = GameObject.Instantiate(enemy, Vector3.zero, Quaternion.identity) as Enemy;
            spawnEnemy.OnDeath += OnEnemyDeath;//订阅事件
        }
    }
    /// <summary>
    /// 处理敌人死亡的后续事务,若当前波敌人全部死亡,开启下一波
    /// </summary>
    void OnEnemyDeath() {
        aliveEnemies--;
        //print("enemy death");
        //存活敌人数为0,开启下一波
        if (aliveEnemies == 0) {
            NextWave();
        }
    }

    /// <summary>
    /// 开始制造下一波敌人
    /// </summary>
    void NextWave() {
        curWaveNum++;
        //print("wave num:" + curWaveNum);
        if (curWaveNum - 1 < waves.Length) {
            curWave = waves[curWaveNum - 1];

            remainEnemiesToSpawn = curWave.enemyCount;
            aliveEnemies = remainEnemiesToSpawn;
        }
    }

    [System.Serializable]
    //波数对象
    public class Wave {
        public int enemyCount;//一波包含敌人数
        public float timeBetweenSpawns;//两波生成间隔时间

    }
}
