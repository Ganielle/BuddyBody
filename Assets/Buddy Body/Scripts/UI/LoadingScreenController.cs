using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadingScreenController : MonoBehaviour
{
    [Header("SETTINGS")]
    [SerializeField] private Slider loadingbar;
    [SerializeField] private TextMeshProUGUI debugger;

    private void OnEnable()
    {
        loadingbar.value = 0;
    }

    private void OnDisable()
    {
        loadingbar.value = 0;
    }

    // Update is called once per frame
    void Update()
    {
        debugger.text = "Debugger: \n" + "Total Scene progress: " + GameManager.Instance.totalSceneProgress +
            "\nDone loading scene: " + GameManager.Instance.doneLoadingScene + "\n Total Question Generated Progress: " + 
            GameManager.Instance.totalQuestionGeneratedProgress + "\nDone Populating Questions: " + 
            GameManager.Instance.questionGenerator.isDone + "\n" + GameManager.Instance.questionGenerator.stats;

        if (GameManager.Instance.sceneController.GetSetLoadingStatus != SceneController.LoadingStatus.NONE)
        {
            loadingbar.value = Mathf.Round((GameManager.Instance.totalSceneProgress + GameManager.Instance.
                totalQuestionGeneratedProgress) / 2f);
        }
    }
}
