using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//工具类
public static class Utils {
    /// <summary>
    /// 洗牌算法,传入目标列表,返回打乱顺序之后的列表,支持泛型
    /// </summary>
    /// <param name="list">打乱的目标列表</param>
    /// <param name="seed">随机种子</param>
    /// <typeparam name="T">目标列表类型</typeparam>
    /// <returns></returns>
    public static T[] ShuffleCoords<T>(T[] list, int seed) {
        System.Random random = new System.Random(seed);//根据随机种子获得一个随机数
        //遍历并随机交换
        for (int i = 0; i < list.Length - 1; i++) {
            int randomIndex = random.Next(i, list.Length);
            T tempItem = list[randomIndex];
            list[randomIndex] = list[i];
            list[i] = tempItem;
        }

        return list;
    }
}
