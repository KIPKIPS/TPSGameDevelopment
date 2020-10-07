using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour {
    public GameObject tilePrefab;
    public Vector2 mapSize;
    public Transform mapHolder;
    [Range(0, 1)] public float outlinePercent; //地图板块间隔
    // Start is called before the first frame update
    void Start() {
        mapSize = new Vector2(8, 8);
        outlinePercent = 0.1f;
        tilePrefab = Resources.Load<GameObject>("Prefabs/Tile");
        mapHolder = transform.Find("MapHolder").transform;
        GenerateMap();
    }

    // Update is called once per frame
    void Update() {

    }

    void GenerateMap() {
        for (int i = 0; i < mapSize.x; i++) {
            for (int j = 0; j < mapSize.y; j++) {
                Vector3 newPos = new Vector3(-mapSize.x / 2 + 0.5f + i+outlinePercent*i, 0,-mapSize.y / 2 + 0.5f + j+ outlinePercent*j); //计算位置
                GameObject tile = GameObject.Instantiate(tilePrefab, newPos, Quaternion.Euler(90,0,0), mapHolder);
            }
        }
    }
}
