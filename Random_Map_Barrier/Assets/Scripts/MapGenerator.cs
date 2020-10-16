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
    public float obsCount = 10;//障碍物数量

    //障碍物前景色和背景色
    public Color foregroundColor, backgroundColor;
    public float minObsHeight = 1, maxObsHeight;

    // Start is called before the first frame update
    void Start() {
        obsPrefab = Resources.Load<GameObject>("Prefabs/Obstacle");//障碍物
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
            GameObject obs = GameObject.Instantiate(obsPrefab,randomCoord.trs );
            //随机高度
            float randomHeight = UnityEngine.Random.Range(minObsHeight, maxObsHeight);
            obs.transform.localScale = new Vector3(1,1, randomHeight);
            obs.transform.localPosition = new Vector3(0, 0, -0.1f - randomHeight/2);

            //随机颜色
            MeshRenderer mesh = obs.GetComponent<MeshRenderer>();
            Material mat = mesh.material;
            float colorPercent = (randomCoord.x / mapSize.x + randomCoord.y / mapSize.y)/2;
            mat.color = Color.Lerp(foregroundColor, backgroundColor, colorPercent);//插值运算
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
