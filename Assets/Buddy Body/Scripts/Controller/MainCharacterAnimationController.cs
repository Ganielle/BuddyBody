using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCharacterAnimationController : MonoBehaviour
{
    [Header("Falling values")]
    [SerializeField] private float falling;
    [SerializeField] private float shortFallToGround;
    [SerializeField] private float highFallToGround;

    [Space]
    [SerializeField] private PlayerMovement playerMovement;

    //  Animator state hash info
    AnimatorStateInfo stateInfo;

    AnalogStick analogStick;
    GenderSetter genderSetter;

    //  movement
    float hor, ver;

    private void OnEnable()
    {
        analogStick = GameManager.Instance.analogStick;
        genderSetter = GameManager.Instance.genderSetter;
    }

    private void OnDisable()
    {
        analogStick = null;
        genderSetter = null;
    }

    private void Update()
    {
        //  StateInfo
        if(GameManager.Instance.GetSetGameState == GameManager.GAMESTATE.GAMEPLAY &&
            (GameManager.Instance.sceneController.GetSetLoadingStatus == SceneController.LoadingStatus.NONE ||
            GameManager.Instance.sceneController.GetSetLoadingStatus == SceneController.LoadingStatus.FINISHLOADING))
        {
            stateInfo = GameManager.Instance.genderSetter.GetSetGenderAnimator.GetCurrentAnimatorStateInfo(0);
            CharacterAnimation();
        }
    }

    private void CharacterAnimation()
    {
        hor = analogStick.Horizontal();
        ver = analogStick.Vertical();

        

        genderSetter.GetSetGenderAnimator.SetBool("isIdle", false);
        genderSetter.GetSetGenderAnimator.SetBool("isWalking", false);
        genderSetter.GetSetGenderAnimator.SetBool("isRunning", false);

        if (playerMovement.speed == 0)
        {
            genderSetter.GetSetGenderAnimator.SetBool("isIdle", true);
        }

        //  walk
        if (playerMovement.speed == playerMovement.walkSpeed)
        {
            genderSetter.GetSetGenderAnimator.SetBool("isWalking", true);
        }

        //  run
        if (playerMovement.speed == playerMovement.runSpeed)
        {
            genderSetter.GetSetGenderAnimator.SetBool("isRunning", true);
        }

    }
}
