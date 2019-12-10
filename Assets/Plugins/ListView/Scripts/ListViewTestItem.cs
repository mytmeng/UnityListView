using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class ListViewTestItem : ListViewItem
{
    [SerializeField]
    Text _nameTxt;
    [SerializeField]
    Text _idTxt;
    [SerializeField]
    Transform _selectedFlag;


    public void SetData(ListViewTestData data) {
        _nameTxt.text = data.name;
        _idTxt.text = data.id.ToString();
    }
    public override void SetHighLight(bool selected)
    {
        _selectedFlag.gameObject.SetActive(selected);
    }
}
