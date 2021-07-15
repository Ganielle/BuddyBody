using MyBox;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemLocatorController : MonoBehaviour
{
    [SerializeField] private List<GameObject> bodyPartList;
    [SerializeField] private ItemArea itemArea;

    [Header("Debugger")]
    [ReadOnly] [SerializeField] List<GameObject> itemList;

    // ========================================================

    private event EventHandler changeItemList;
    public event EventHandler onChangeItemList
    {
        add
        {
            if (changeItemList == null || !changeItemList.GetInvocationList().Contains(value))
                changeItemList += value;
        }
        remove
        {
            changeItemList -= value;
        }
    }
    public void AddRemoveItemList(GameObject item, bool shouldAdd)
    {
        if (shouldAdd)
            itemList.Add(item);
        else
            itemList.Remove(item);

        changeItemList?.Invoke(this, EventArgs.Empty);
    }
    public List<GameObject> GetItemList
    {
        get { return itemList; }
    }

    private void OnEnable()
    {
        onChangeItemList += ItemChecker;
        GameManager.Instance.onpickedupChange += RemoveItem;
    }

    private void OnDisable()
    {
        onChangeItemList -= ItemChecker;
        GameManager.Instance.onpickedupChange -= RemoveItem;
    }

    private void Start()
    {
        StartCoroutine(InsertBodyPartsOnList());
    }

    private void RemoveItem(object sender, EventArgs e)
    {
        if (!GameManager.Instance.GetSetPickedupState)
            return;

        AddRemoveItemList(GameManager.Instance.inventoryStageOne.GetSetBodyPartObj, false);
    }

    private void ItemChecker(object sender, EventArgs e)
    {
        if (itemList.Count == 0)
            gameObject.SetActive(false);
    }

    IEnumerator InsertBodyPartsOnList()
    {
        yield return new WaitForEndOfFrame();

        for (int a = 0; a < bodyPartList.Count; a++)
        {
            AddRemoveItemList(bodyPartList[a].transform.GetChild(0).gameObject, true);
            yield return null;
        }

        if (GameManager.Instance.levelGeneratorDifficulty.GetSetDifficulty == Question.Difficulty.Hard)
            StartCoroutine(itemArea.QuestSetter(GetItemList));
    }
}
