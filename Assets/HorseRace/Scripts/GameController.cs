using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class GameController : MonoBehaviour
{
    [SerializeField] Button StartButton;
    [SerializeField] TextMeshProUGUI CountDownText;
    public UnityEvent GameStarted;
    public UnityEvent GameEnded;
    private bool running = false;

    private void OnEnable()
    {
        StartButton.onClick.AddListener(StartButton_clicked);
    }

    private void OnDisable()
    {
        StartButton.onClick.RemoveListener(StartButton_clicked);
    }

    private void StartButton_clicked()
    {
        if (!running)
        {
            running = true;
            StartCoroutine(StartGameRoutine());
            StartButton.GetComponentInChildren<TextMeshProUGUI>().text = "End Game";
        }
        else
        {
            Finished("niemand", Color.black);
            StartButton.GetComponentInChildren<TextMeshProUGUI>().text = "Start Game";
        }
    }

    public void Finished(string naam, Color playercolor)
    {
        if (running)
        {
            running = false;
            GameEnded.Invoke();
            StartCoroutine(ShowWinnerRoutine(naam, playercolor));
            StartButton.GetComponentInChildren<TextMeshProUGUI>().text = "Start Game";
        }
    }

    private IEnumerator ShowWinnerRoutine(string naam, Color playercolor)
    {
        CountDownText.gameObject.SetActive(true);
        CountDownText.text = naam;
        CountDownText.color = playercolor;
        for (int angle = 0; angle < 361; angle += 10)
        {
            CountDownText.transform.localRotation = Quaternion.Euler(0, 0, angle);
            CountDownText.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, angle * 4);
            CountDownText.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, angle);

            yield return new WaitForSeconds(0.01f);
        }

    }

    private IEnumerator StartGameRoutine()
    {
        CountDownText.gameObject.SetActive(true);
        for (int i = 3; i>0; --i)
        {
            CountDownText.text = i.ToString();
            CountDownText.color = Color.red;
            for (int angle = 0; angle < 361; angle+=10)
            {
                CountDownText.transform.localRotation = Quaternion.Euler(0, 0, angle);
                CountDownText.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, angle * 4);
                CountDownText.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, angle);

                yield return new WaitForSeconds(0.01f);
            }
            yield return new WaitForSeconds(1.0f);
        }

        CountDownText.text = "START!";
        CountDownText.color = Color.green;
        for (int angle = 0; angle < 361; angle += 10)
        {
            CountDownText.transform.localRotation = Quaternion.Euler(0, 0, angle);
            CountDownText.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, angle * 4);
            CountDownText.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, angle);

            yield return new WaitForSeconds(0.01f);
        }
        GameStarted.Invoke();
        yield return new WaitForSeconds(0.5f);
        CountDownText.gameObject.SetActive(false);

    }
}
