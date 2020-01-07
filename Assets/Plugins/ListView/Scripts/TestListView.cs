using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestListView: ListView<ListViewTestItem, ListViewTestData>
{


    public override void SetData(ListViewTestItem item, ListViewTestData data)
    {
        item.SetData(data);
    }

}
