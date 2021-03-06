﻿using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DynamicButtons : MonoBehaviour
{
    // Script used to ask questions and get responses based on array of questions
    // Dynamically populate the answers and responses for selected buttons and text objects

    // Might be useful for some sort of tutorial or something

    [SerializeField]
    Text quesionTextElement;
    [SerializeField]
    Button[] buttons;
    [SerializeField]
    GameObject quizEndCanvas;
    [Space]
    [SerializeField]
    Question[] questions;

    [Range(0,10)]
    public float waitTime;

    void Start()
    {
        InitializeButtons();
        PresentCurrentQuestion();
    }

    int currentQuestionIndex = 0;

    public Question GetCurrentQuestion()
    {
        var question = questions[currentQuestionIndex];
        return question;
    }

    void PresentCurrentQuestion()
    {
        if (currentQuestionIndex >= questions.Length)
        {
            EndQuiz();
            return;
        }

        var question = GetCurrentQuestion();
        quesionTextElement.text = question.QuestionText;

        for (int i = 0; i < buttons.Length; i++)
        {
            if (i >= question.Answers.Length)
            {
                buttons[i].gameObject.SetActive(false);
                continue;
            }

            string answerText = question.Answers[i];

            buttons[i].gameObject.SetActive(true);
            buttons[i].GetComponentInChildren<Text>().text = answerText;
        }
    }

    void InitializeButtons()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            Button button = buttons[i];

            int buttonIndex = i;
            button.onClick.AddListener(() => ShowResponse(buttonIndex));
        }
    }

    void ShowResponse(int buttonIndex)
    {
        var question = GetCurrentQuestion();
        quesionTextElement.text = question.Responses[buttonIndex];

        StartCoroutine(MoveToNextQuestionAfterDelay());
    }

    void EnableDisableAllButtons(bool isEnabled)
    {
        for (int i = 0; i < buttons.Length; i++)
            buttons[i].GetComponent<Button>().enabled = isEnabled;
    }

    IEnumerator MoveToNextQuestionAfterDelay()
    {
        EnableDisableAllButtons(false);
        yield return new WaitForSeconds(waitTime);
        currentQuestionIndex++;
        EnableDisableAllButtons(true);
        PresentCurrentQuestion();
    }

    void EndQuiz()
    {
        foreach(Button button in buttons)
        {
            button.image.enabled = false;
        }
        // Move back to the main scene
    }
}

[Serializable]
public class Question
{
    public string QuestionText;
    public string[] Answers;
    public string[] Responses;
}
