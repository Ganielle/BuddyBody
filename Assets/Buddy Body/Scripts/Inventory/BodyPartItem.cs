using MyBox;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class BodyPartItem : MonoBehaviour
{
    [Header("Color")]
    [SerializeField] private Color disable;
    [SerializeField] private Color enable;

    [Header("UI")]
    [SerializeField] private Image bodyPartImage;
    [SerializeField] private Button bodyPartButton;
   
    [Header("Debugger")]
    [ReadOnly] public BodyPartItemData bodyPart;

    private void OnEnable()
    {
        GameManager.Instance.inventoryStageOne.onChangeSelectBodyPart += ButtonEnabler;

        ButtonEnabler();
    }

    private void OnDisable()
    {
        GameManager.Instance.inventoryStageOne.onChangeSelectBodyPart -= ButtonEnabler;
    }

    private void Start() 
    { 
        SetSpriteToImage();
    }

    private void ButtonEnabler(object sender, EventArgs e)
    {
        ButtonEnabler();
    }

    private void ButtonEnabler()
    {
        if (GameManager.Instance.inventoryStageOne.GetSetBodyPartInventoryObj == gameObject)
        {
            bodyPartImage.color = disable;
            bodyPartButton.interactable = false;
        }

        if (GameManager.Instance.inventoryStageOne.GetSetBodyPartInventoryObj != gameObject ||
            GameManager.Instance.inventoryStageOne.GetSetBodyPartInventoryObj == null)
        {
            bodyPartImage.color = enable;
            bodyPartButton.interactable = true;
        }
    }

    private void SetSpriteToImage()
    {
        if (bodyPart != null)
        {
            bodyPartImage.sprite = bodyPart.bodyPart.bodyPartSprite;
        }
    }

    public void ClickBodyPartItem()
    {
        GameManager.Instance.inventoryStageOne.GetSetBodyPartInventoryObj = gameObject;
        GameManager.Instance.inventoryStageOne.GetSetBodyPartInventory = bodyPart.bodyPart;
        GameManager.Instance.inventoryStageOne.GetSetBodyPartItemInventory = GetComponent<BodyPartItem>();
    }
}
