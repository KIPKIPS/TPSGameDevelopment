using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour {
    public GameObject tilePrefab;
    public Vector2 mapSize = new Vector2(8, 8);
    public Transform mapHolder;
    [Range(0, 1)] public float outlinePercent = 0.1f; //地图板块间隔
    public List<Coord> tileCoordsList = new List<Coord>();

    public Queue<Coord> shuffleQueue;//队列
    //障碍物预制体
    public GameObject obsPrefab;
    [Header("Obstacles Count")]
    [Range(0, 1)] public float obsPercent = 0.5f;//障碍物数量

    [Header("Map Fully Accessible")]
    //障碍物前景色和背景色
    public Color foregroundColor, backgroundColor;
    public float minObsHeight = 1, maxObsHeight;

    private Coord mapCenter;//地图的中心点,每个随机地图的中心点都是不能有障碍物的
    public bool[,] mapObs;//记录地图坐标是否有障碍物的二维数组

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
                Vector3 newPos = new Vector3(-mapSize.x / 2 + 0.5f + i, 0, -mapSize.y / 2 + 0.5f + j); //计算位置
                GameObject tile = GameObject.Instantiate(tilePrefab, newPos, Quaternion.Euler(90, 0, 0), mapHolder);
                tile.transform.localScale = Vector3.one * (1 - outlinePercent); //间隔效果
                tileCoordsList.Add(new Coord(i, j, tile.transform));
            }
        }
        //随机障碍生成
        shuffleQueue = new Queue<Coord>(Utils.ShuffleCoords(tileCoordsList.ToArray()));
        mapCenter = new Coord((int)mapSize.x / 2, (int)mapSize.y / 2, null); //地图中心点
        mapObs = new bool[(int)mapSize.x, (int)mapSize.y];//初始化障碍物信息数组
        int obsCount = Convert.ToInt32(mapSize.x * mapSize.y * obsPercent);
        Debug.Log(obsCount);

        int curObsCount = 0;//当前场景中包含的障碍物个数

        for (int i = 0; i < obsCount; i++) {

            Coord randomCoord = GetRandomCoord();
            mapObs[randomCoord.x, randomCoord.y] = true; //假设当前位置可以存在障碍物
            curObsCount++;
            //不是所有随机到的坐标都可以生成障碍物,若需要连接成连通地图
            if (randomCoord != mapCenter && MapIsFullyAccessible(mapObs, curObsCount)) {
                GameObject obs = GameObject.Instantiate(obsPrefab, randomCoord.trs);
                //随机高度
                float randomHeight = UnityEngine.Random.Range(minObsHeight, maxObsHeight);
                obs.transform.localScale = new Vector3(1, 1, randomHeight);
                obs.transform.localPosition = new Vector3(0, 0, -0.1f - randomHeight / 2);

                //随机颜色
                MeshRenderer mesh = obs.GetComponent<MeshRenderer>();
                Material mat = mesh.material;
                float colorPercent = (randomCoord.x / mapSize.x + randomCoord.y / mapSize.y) / 2;
                mat.color = Color.Lerp(foregroundColor, backgroundColor, colorPercent);//插值运算

                
            }
            else {
                mapObs[randomCoord.x, randomCoord.y] = false;
                curObsCount--;
            }

        }
    }

    //填充判断算法
    private bool MapIsFullyAccessible(bool[,] mapObsInfo, int curObsCount) {
        bool[,] mapFlag = new bool[mapObsInfo.GetLength(0),mapObsInfo.GetLength(1)];//初始化标记信息
        Queue<Coord> queue = new Queue<Coord>(); //所有通过筛选的地图会存放到这个队列中
        queue.Enqueue(mapCenter);
        mapFlag[(int)mapCenter.x, (int)mapCenter.y] = true;//中心点一定不能有障碍物
        int accessibleCount = 1;
        while (queue.Count > 0) {
            Coord curTile = queue.Dequeue();
            //四领域填充算法
            for (int i = -1; i <= 1; i++) {
                for (int j = -1; j <= 1; j++) {
                    //八个位置信息
                    int neighborX = curTile.x + i;
                    int neighborY = curTile.y + j;
                    //筛选出检测的坐标信息
                    if (i == 0 || j == 0) {
                        if (neighborX >= 0 && neighborX < mapObsInfo.GetLength(0) && neighborY >= 0 && neighborY < mapObsInfo.GetLength(1)) {
                            //当前坐标没有被检测过,并且不存在障碍物
                            if (!mapFlag[neighborX,neighborY]&&!mapObsInfo[neighborX,neighborY]) {
                                mapFlag[neighborX, neighborY] = true;
                                accessibleCount++;
                                queue.Enqueue(new Coord(neighborX,neighborY,null));
                            }
                        }
                    }
                }
            }
        }

        int obsTargetCount = (int) (mapSize.x * mapSize.y - curObsCount);
        return obsTargetCount == accessibleCount;
    }

    public Coord GetRandomCoord() {
        Coord random = shuffleQueue.Dequeue();//出队列,此时的队列是打乱顺序的队列
        shuffleQueue.Enqueue(random);//将出队的元素放置到队尾,保持队列的完整
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
    //重载运算符,static, 必须返回bool类型
    public static bool operator !=(Coord c1, Coord c2) {
        return c1.x != c2.x || c1.y != c2.y;
    }
    public static bool operator ==(Coord c1, Coord c2) {
        return c1.x == c2.x && c1.y == c2.y;
    }
}
