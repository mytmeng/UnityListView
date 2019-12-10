using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ListViewTest : ListView<ListViewTestItem, ListViewTestData>
{
    [SerializeField]
    Button Btn;
    void OnBtn()
    {
        List<ListViewTestData> datas = new List<ListViewTestData>() {
            new ListViewTestData("abc", 1),
            new ListViewTestData("123", 2),
            new ListViewTestData("123", 3),
            new ListViewTestData("ABC", 4),
            new ListViewTestData("ABC", 5),
            new ListViewTestData("ABC", 6),
            new ListViewTestData("ABC", 7),
            new ListViewTestData("ABC", 8),
            new ListViewTestData("ABC", 9),
            new ListViewTestData("ABC", 10),
            new ListViewTestData("ABC", 11),
            new ListViewTestData("ABC", 12),
            new ListViewTestData("ABC", 13),
            new ListViewTestData("ABC", 14),
            new ListViewTestData("ABC", 15),
            new ListViewTestData("ABC", 16),
            new ListViewTestData("ABC", 17),
            new ListViewTestData("ABC", 18),
            new ListViewTestData("ABC", 19),
            new ListViewTestData("ABC", 20),
            new ListViewTestData("ABC", 21),
            new ListViewTestData("ABC", 22),
            new ListViewTestData("ABC", 23),
            new ListViewTestData("ABC", 24),
            new ListViewTestData("ABC", 25),
            new ListViewTestData("ABC", 26),
            new ListViewTestData("ABC", 27),
            new ListViewTestData("ABC", 28),
            new ListViewTestData("ABC", 29)
        };
        DataSource = datas;
    }

    public override void SetData(ListViewTestItem item, ListViewTestData data)
    {
        item.SetData(data);
    }

    protected void Start() {
        Btn.onClick.AddListener(OnBtn);
        List<ListViewTestData> datas = new List<ListViewTestData>() {
            new ListViewTestData("abc", 0),
            new ListViewTestData("abc", 1),
            new ListViewTestData("123", 2),
            new ListViewTestData("123", 3),
            new ListViewTestData("ABC", 4),
            new ListViewTestData("ABC", 5),
            new ListViewTestData("ABC", 6),
            new ListViewTestData("ABC", 7),
            new ListViewTestData("ABC", 8),
            new ListViewTestData("ABC", 9),
            new ListViewTestData("ABC", 10),
            new ListViewTestData("ABC", 11),
            new ListViewTestData("ABC", 12),
            new ListViewTestData("ABC", 13),
            new ListViewTestData("ABC", 14),
            new ListViewTestData("ABC", 15),
            new ListViewTestData("ABC", 16),
            new ListViewTestData("ABC", 17),
            new ListViewTestData("ABC", 18),
            new ListViewTestData("ABC", 19),
            new ListViewTestData("ABC", 20),
            new ListViewTestData("ABC", 21),
            new ListViewTestData("ABC", 22),
            new ListViewTestData("ABC", 23),
            new ListViewTestData("ABC", 24),
            new ListViewTestData("ABC", 25),
            new ListViewTestData("ABC", 26),
            new ListViewTestData("ABC", 27),
            new ListViewTestData("ABC", 28),
            new ListViewTestData("ABC", 29),
            new ListViewTestData("ABC", 30),
            new ListViewTestData("ABC", 31),
            new ListViewTestData("ABC", 32),
            new ListViewTestData("ABC", 33),
            new ListViewTestData("ABC", 34),
            new ListViewTestData("ABC", 35),
            new ListViewTestData("ABC", 36),
            new ListViewTestData("ABC", 37),
            new ListViewTestData("ABC", 38),
            new ListViewTestData("ABC", 39),
            new ListViewTestData("ABC", 40),
            new ListViewTestData("ABC", 41),
            new ListViewTestData("ABC", 42),
            new ListViewTestData("ABC", 43),
            new ListViewTestData("ABC", 44),
            new ListViewTestData("ABC", 45),
            new ListViewTestData("ABC", 46),
            new ListViewTestData("ABC", 47),
            new ListViewTestData("ABC", 48),
            new ListViewTestData("ABC", 49),
            new ListViewTestData("ABC", 50),
            new ListViewTestData("ABC", 51),
            new ListViewTestData("ABC", 52),
            new ListViewTestData("ABC", 53),
            new ListViewTestData("ABC", 54),
            new ListViewTestData("ABC", 55),
            new ListViewTestData("ABC", 56),
            new ListViewTestData("ABC", 57),
            new ListViewTestData("ABC", 58),
            new ListViewTestData("ABC", 59),
            new ListViewTestData("ABC", 60),
            new ListViewTestData("ABC", 61),
            new ListViewTestData("ABC", 62),
            new ListViewTestData("ABC", 63),
            new ListViewTestData("ABC", 64),
            new ListViewTestData("ABC", 65),
            new ListViewTestData("ABC", 66),
            new ListViewTestData("ABC", 67),
            new ListViewTestData("ABC", 68),
            new ListViewTestData("ABC", 69),
            new ListViewTestData("ABC", 70),
            new ListViewTestData("ABC", 71),
            new ListViewTestData("ABC", 72),
            new ListViewTestData("ABC", 73),
            new ListViewTestData("ABC", 74),
            new ListViewTestData("ABC", 75),
            new ListViewTestData("ABC", 76),
            new ListViewTestData("ABC", 77),
            new ListViewTestData("ABC", 78),
            new ListViewTestData("ABC", 79),
            new ListViewTestData("ABC", 80),
            new ListViewTestData("ABC", 81),
            new ListViewTestData("ABC", 82),
            new ListViewTestData("ABC", 83),
            new ListViewTestData("ABC", 84),
            new ListViewTestData("ABC", 85),
            new ListViewTestData("ABC", 86),
            new ListViewTestData("ABC", 87),
            new ListViewTestData("ABC", 88),
            new ListViewTestData("ABC", 89),
            new ListViewTestData("ABC", 90),
            new ListViewTestData("ABC", 91),
            new ListViewTestData("ABC", 92),
            new ListViewTestData("ABC", 93),
            new ListViewTestData("ABC", 94),
            new ListViewTestData("ABC", 95),
            new ListViewTestData("ABC", 96),
            new ListViewTestData("ABC", 97),
            new ListViewTestData("ABC", 98),
            new ListViewTestData("ABC", 99),
        };
        DataSource = datas;
        OnClickItem += OnClickItemCB;
        ScrollTo(11);
    }

    private void OnClickItemCB(int i)
    {
        Select(i);
        Debug.LogError("选中了:"+ i);
    }
}
