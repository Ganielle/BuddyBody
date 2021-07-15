using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimationEventController : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] private AudioClip pickupItemClip;

    public void PickUpItem()
    {
        GameManager.Instance.inventoryStageOne.InsertBodyPartOnList(
            GameManager.Instance.inventoryStageOne.OnTriggerEnterCharactrOnBodyPart);

        GameManager.Instance.soundSystem.PlaySFX(pickupItemClip);

        //  for destroying game object
        //  and for nullifying body part
        Destroy(GameManager.Instance.inventoryStageOne.GetSetBodyPartObj);
        GameManager.Instance.inventoryStageOne.GetSetBodyPartObj = null;
        GameManager.Instance.inventoryStageOne.OnTriggerEnterCharactrOnBodyPart = null;
    }

    public void StopIdlePickup() => GameManager.Instance.idlePickup = false;
}
