using MyBox;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionGenerator : MonoBehaviour
{
    [SerializeField] private GameObject questionUIInstantiate;

    [Header("Transform")]
    public Transform questionListTF;

    [Header("Questions")]
    [SerializeField] private List<Question> questionStageOne;

    [Header("Debugger")]
    [ReadOnly] public List<Question> selectedQuestionStageOne;
    [ReadOnly] public int count;
    [ReadOnly] public float progress;
    [ReadOnly] public bool isDone;
    [ReadOnly] public string stats;
    [ReadOnly] [SerializeField] bool isDonePopulating;

    IEnumerator PopulateStageOneQuestions()
    {
        isDonePopulating = false;

        count = 0;

        foreach (Question question in questionStageOne)
        {
            if (question.subTopic == GameManager.Instance.levelGeneratorDifficulty.GetSetSubTopic && question.stageOne)
                selectedQuestionStageOne.Add(question);

            progress = (float)count / (selectedQuestionStageOne.Count + questionStageOne.Count);

            count++;

            yield return null;
        }

        yield return StartCoroutine(selectedQuestionStageOne.Shuffle());

        stats += " Done shuffle";

        isDonePopulating = true;
    }

    public IEnumerator InstantiateStageOneQuestions()
    {
        isDone = false;

        StartCoroutine(PopulateStageOneQuestions());

        yield return new WaitWhile(() => !isDonePopulating);

        stats += " Done populate questions";

        for (int a = 0; a < selectedQuestionStageOne.Count; a++)
        {
            GameObject questions = Instantiate(questionUIInstantiate, questionListTF);

            questions.GetComponent<QuestionItemStageOne>().question = selectedQuestionStageOne[a];

            GameManager.Instance.questionList.Add(questions.GetComponent<QuestionItemStageOne>());

            progress = (float)count / (selectedQuestionStageOne.Count + questionStageOne.Count);

            GameManager.Instance.questCount++;

            count++;
        }

        progress = 1f;

        yield return new WaitForSecondsRealtime(1f);

        isDone = true;
    }
}
