using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MapGenerator))]
public class MapEditor : Editor {
    public override void OnInspectorGUI() {
        //base.OnInspectorGUI();
        //仅当检视面板的值发生变化时或者点击生成地图按钮时才调用
        if (DrawDefaultInspector() || GUILayout.Button("Generate Map")) {
            MapGenerator map = target as MapGenerator;
            map.GenerateMap();
        }
    }
}
