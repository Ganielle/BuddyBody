using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InventoryStageOne
{
    public enum InventoryState
    {
        NONE,
        INVENTORY
    }
    private event EventHandler changeInventoryState;
    public event EventHandler onChangeInventoryState
    {
        add
        {
            if (changeInventoryState == null || !changeInventoryState.GetInvocationList().Contains(value))
                changeInventoryState += value;
        }
        remove
        {
            changeInventoryState -= value;
        }
    }
    InventoryState inventoryState;
    public InventoryState GetSetInventoryState
    {
        get { return inventoryState; }
        set
        {
            inventoryState = value;
            changeInventoryState?.Invoke(this, EventArgs.Empty);
        }
    }

    List<BodyPartItemData> bodyPartList;


    public InventoryStageOne()
    {
        bodyPartList = new List<BodyPartItemData>();
    }

    public List<BodyPartItemData> GetBodyPartList()
    {
        return bodyPartList;
    }

    private event EventHandler bodyPartAddOnList;
    public event EventHandler onBodyPartAddOnList
    {
        add
        {
            if (bodyPartAddOnList == null || !bodyPartAddOnList.GetInvocationList().Contains(value))
                bodyPartAddOnList += value;
        }
        remove
        {
            bodyPartAddOnList -= value;
        }
    }
    public void InsertBodyPartOnList(BodyPartItemData bodyPart)
    {
        bodyPartList.Add(bodyPart);
        bodyPartAddOnList?.Invoke(this, EventArgs.Empty);
    }

    private event EventHandler bodyPartChange;
    public event EventHandler onBodyPartChange
    {
        add
        {
            if (bodyPartChange == null || !bodyPartChange.GetInvocationList().Contains(value))
                bodyPartChange += value;
        }
        remove
        {
            bodyPartChange -= value;
        }
    }
    GameObject bodyPartObj;
    BodyPartItemData bodyPart;
    public BodyPartItemData OnTriggerEnterCharactrOnBodyPart
    {
        get { return bodyPart; }
        set
        {
            bodyPart = value;
            bodyPartChange?.Invoke(this, EventArgs.Empty);
        }
    }
    public GameObject GetSetBodyPartObj
    {
        get { return bodyPartObj; }
        set { bodyPartObj = value; }
    }

    private event EventHandler changeSelectBodyPart;
    public event EventHandler onChangeSelectBodyPart
    {
        add
        {
            if (changeSelectBodyPart == null || !changeSelectBodyPart.GetInvocationList().Contains(value))
                changeSelectBodyPart += value;
        }
        remove
        {
            changeSelectBodyPart -= value;
        }
    }
    GameObject bodyPartInventoryObj;
    BodyPart bodyPartInventory;
    BodyPartItem bodyPartItem;
    public BodyPart GetSetBodyPartInventory
    {
        get { return bodyPartInventory; }
        set
        {
            bodyPartInventory = value;
            changeSelectBodyPart?.Invoke(this, EventArgs.Empty);
        }
    }
    public GameObject GetSetBodyPartInventoryObj
    {
        get { return bodyPartInventoryObj; }
        set { bodyPartInventoryObj = value; }
    }
    public BodyPartItem GetSetBodyPartItemInventory
    {
        get { return bodyPartItem; }
        set { bodyPartItem = value; }
    }

    private event EventHandler selectAnsweredChange;
    public event EventHandler onSelectAnsweredChange
    {
        add
        {
            if (selectAnsweredChange == null || !selectAnsweredChange.GetInvocationList().Contains(value))
                selectAnsweredChange += value;
        }
        remove
        {
            selectAnsweredChange -= value;
        }
    }
    BodyPartItem answerBodyPartItem;
    GameObject answerBodyPartObj;
    BodyPart answerBodyPart;
    public GameObject GetSetAnswerBodyPartObj
    {
        get { return answerBodyPartObj; }
        set
        {
            answerBodyPartObj = value;
            selectAnsweredChange?.Invoke(this, EventArgs.Empty);
        }
    }
    public BodyPartItem GetSetAnswerBodyPartItem
    {
        get { return answerBodyPartItem; }
        set { answerBodyPartItem = value; }
    }
    public BodyPart GetSetAnswerBodyPart
    {
        get { return answerBodyPart; }
        set { answerBodyPart = value; }
    }
}
