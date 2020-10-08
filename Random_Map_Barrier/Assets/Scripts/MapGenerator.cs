using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour {
    public GameObject tilePrefab;
    public Vector2 mapSize = new Vector2(8, 8);
    public Transform mapHolder;
    [Range(0, 1)] public float outlinePercent  = 0.1f; //地图板块间隔
    public List<Coord> tileCoordsList = new List<Coord>();

    public Queue<Coord> shuffleQueue;//队列
    //障碍物预制体
    public GameObject obsPrefab;
    public float obsCount;

    
    // Start is called before the first frame update
    void Start() {
        obsPrefab = Resources.Load<GameObject>("Prefabs/Obstacle");//障碍物
        obsCount = 10;
        tilePrefab = Resources.Load<GameObject>("Prefabs/Tile");//地图瓦片
        mapHolder = transform.Find("MapHolder").transform;
        GenerateMap();
    }

    // Update is called once per frame
    void Update() {

    }

    //生成地图算法
    void GenerateMap() {
        for (int i = 0; i < mapSize.x; i++) {
            for (int j = 0; j < mapSize.y; j++) {
                Vector3 newPos = new Vector3(-mapSize.x / 2 + 0.5f + i, 0,-mapSize.y / 2 + 0.5f + j); //计算位置
                GameObject tile = GameObject.Instantiate(tilePrefab, newPos, Quaternion.Euler(90,0,0), mapHolder);
                tile.transform.localScale = Vector3.one * (1 - outlinePercent); //间隔效果

                tileCoordsList.Add(new Coord(i,j, tile.transform));
            }
        }
        //随机障碍生成
        shuffleQueue = new Queue<Coord>(Utils.ShuffleCoords(tileCoordsList.ToArray()));
        for (int i = 0; i < obsCount; i++) {
            Coord randomCoord = GetRandomCoord();
            Vector3 newPos = new Vector3(-mapSize.x / 2 + 0.5f + randomCoord.x, 0.6f, -mapSize.y / 2 + 0.5f + randomCoord.y); //计算位置
            GameObject obs = GameObject.Instantiate(obsPrefab, newPos, Quaternion.Euler(90, 0, 0), randomCoord.trs);
        }
    }

    public Coord GetRandomCoord() {
        Coord random = shuffleQueue.Dequeue();//出队列,此时的队列是打乱顺序的队列
        shuffleQueue.Enqueue((random));//将出队的元素放置到队尾,保持队列的完整
        return random;
    }
}

[System.Serializable]
public struct Coord {
    public int x;
    public int y;
    public Transform trs;
    public Coord(int x, int y, Transform self) {
        this.x = x;
        this.y = y;
        this.trs = self;
    }
}
