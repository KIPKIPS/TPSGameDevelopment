using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//工具类
public class Utils:MonoBehaviour {
    // Start is called before the first frame update
    //洗牌算法,传入目标列表,返回打乱顺序之后的列表
    public static Coord[] ShuffleCoords(Coord[] list) {
        for (int i = 0; i < list.Length; i++) {
            int randomNum = Random.Range(i, list.Length);

            Coord temp = list[randomNum];
            list[randomNum] = list[i];
            list[i] = temp;
        }
        return list;
    }
}
