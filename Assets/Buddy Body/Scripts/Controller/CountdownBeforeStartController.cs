using MyBox;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CountdownBeforeStartController : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] private AudioClip withTimerNumberClip;
    [SerializeField] private AudioClip goClip;

    [Space]
    [SerializeField] private TextMeshProUGUI timerText;

    [Header("Debugger")]
    [ReadOnly] [SerializeField] float floatTimer;
    [ReadOnly] [SerializeField] bool shouldCountdown;
    [ReadOnly] [SerializeField] bool continueCountdown;

    private void OnEnable()
    {
        shouldCountdown = true;
        floatTimer = 4f;

        StartCoroutine(PlaySound());
    }

    private void OnDisable()
    {
        timerText.text = "";
        floatTimer = 4f;
    }

    private void Update()
    {
        if (shouldCountdown)
        {
            if (floatTimer <= 0)
            {
                shouldCountdown = false;
                GameManager.Instance.soundSystem.PlayCountDownTimer(goClip);
                timerText.text = "GO!";
                floatTimer = 0f;
                GameManager.Instance.GetSetFindStartState = GameManager.FindStartState.NONE;
                GameManager.Instance.GetSetGameState = GameManager.GAMESTATE.GAMEPLAY;
                StartCoroutine(DisableObject());
            }

            if (floatTimer > 0)
            {
                floatTimer -= Time.deltaTime;

                if (Convert.ToInt32(floatTimer) == 4)
                    timerText.text = "";
                else if (Convert.ToInt32(floatTimer) == 0)
                    timerText.text = "GO!";
                else
                    timerText.text = Convert.ToString(Convert.ToInt32(floatTimer));
            }
        }
    }

    IEnumerator DisableObject()
    {
        yield return new WaitForSeconds(1f);

        continueCountdown = false;

        gameObject.SetActive(false);
    }

    IEnumerator PlaySound()
    {
        while(Convert.ToInt32(floatTimer) > 0)
        {
            yield return new WaitForSeconds(1f);

            if (Convert.ToInt32(floatTimer) > 0)
                GameManager.Instance.soundSystem.PlayCountDownTimer(withTimerNumberClip);
        }
    }
}
