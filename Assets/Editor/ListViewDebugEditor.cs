using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ListViewDebugEditor:EditorWindow
{
    [MenuItem("DebugTool/ListView")]
    public static void Open()
    {
        GetWindow<ListViewDebugEditor>();
    }
    ListViewTest listView;
    int scrollTarget;
    private void OnGUI()
    {
        listView = EditorGUILayout.ObjectField(listView, typeof(ListViewTest), true) as ListViewTest;
        if(listView == null)
        {
            return;
        }
        scrollTarget = EditorGUILayout.IntSlider(scrollTarget, 0, listView.DataSource.Count - 1);
        if(GUILayout.Button("滑动至"))
        {
            listView.ScrollTo(scrollTarget);
        }
    }
}
