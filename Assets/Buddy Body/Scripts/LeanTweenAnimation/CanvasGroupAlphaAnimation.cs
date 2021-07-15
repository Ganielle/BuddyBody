using System;
using System.Runtime.InteropServices;
using MyBox;
using UnityEngine;

public class CanvasGroupAlphaAnimation : MonoBehaviour
{
    [Header("Tween Settings")]
    public LeanTweenType easeType;
    [SerializeField] private CanvasGroup tweenCanvasGroup;
    [ConditionalField("playOnEnable")] [SerializeField] private float toAlpha;
    [ConditionalField("playOnEnable")] [SerializeField] private float speed;
    [ConditionalField("playOnEnable")] [SerializeField] private float delay;
    [SerializeField] private bool playOnEnable;

    LTDescr imageTween;

    private void OnEnable()
    {
        if (playOnEnable)
            CanvasGroupAnimation(toAlpha, easeType, speed, delay, EndTween);
    }

    public void CanvasGroupAnimation(float toAlpha, LeanTweenType ease, float speed, float delay, [Optional] Action action)
    {
        imageTween = LeanTween.alphaCanvas(tweenCanvasGroup, toAlpha, speed).setEase(ease).setDelay(delay);

        if (action != null)
            imageTween.setOnComplete(action);
    }

    private void EndTween() => LeanTween.cancel(gameObject);
}
