using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeAnimationItemIndicator : MonoBehaviour
{
    [Header("Lean Tween")]
    [SerializeField] private LeanTweenType easeType;
    [SerializeField] private float fadeTime;

    [Header("GameObject")]
    [SerializeField] private GameObject headObj;
    [SerializeField] private GameObject footObj;

    [Header("Image")]
    [SerializeField] private Image headImage;
    [SerializeField] private Image footImage;

    private void OnEnable()
    {
        headImage.color = new Color(headImage.color.r, headImage.color.g, headImage.color.b, 0f);
        footImage.color = new Color(footImage.color.r, footImage.color.g, footImage.color.b, 0f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
            FadeIn(headObj, footObj);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
            FadeOut(headObj, footObj);
    }

    private void FadeIn(GameObject image1, GameObject image2)
    {
        LeanTween.alpha(image1.GetComponent<RectTransform>(), 1f, fadeTime).setEase(easeType);
        LeanTween.alpha(image2.GetComponent<RectTransform>(), 1f, fadeTime).setEase(easeType);
    }

    private void FadeOut(GameObject image1, GameObject image2)
    {
        LeanTween.alpha(image1.GetComponent<RectTransform>(), 0f, fadeTime).setEase(easeType);
        LeanTween.alpha(image2.GetComponent<RectTransform>(), 0f, fadeTime).setEase(easeType);
    }
}
