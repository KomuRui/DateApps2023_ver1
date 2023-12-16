using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using System;

public class CountDownAndTimer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timeText;       //時間制限テキスト
    [SerializeField] private TextMeshProUGUI countDownText;  //カウントダウンテキスト
    [SerializeField] private Fade fade;  //フェード

    private int nowCountDownTime = 3;
    private Vector3 beforeScale;
    [SerializeField] public float time = 30.0f;
    private bool isStop = true;
    public bool isfinish = false;
    private float beforeTime;

    // Start is called before the first frame update
    void Start()
    {
        beforeTime = time;
        beforeScale = countDownText.transform.localScale;
        countDownText.transform.DOScale(5.0f, 1.0f).SetEase(Ease.InCubic);

        if(fade != null)
            fade.FadeOut(1.0f);

        StartCoroutine(CountDownText(1.0f));
    }

    // Update is called once per frame
    void Update()
    {
        //時間計算
        TimeCalc();
    }

    //時間計算
    private void TimeCalc()
    {
        if (isStop || isfinish || GameManager.nowMiniGameManager.IsFinish() || timeText == null) return;
        time -= Time.deltaTime;
        time = Mathf.Max(time, 0);
        timeText.text = ((int)time).ToString() + ":00";
        if (time <= 1)
        {
            isfinish = true;
            timeText.text = "";
            GameManager.nowMiniGameManager.SetMiniGameFinish();
        }
    }

    //落とす
    IEnumerator CountDownText(float delay)
    {
        yield return new WaitForSeconds(delay);

        nowCountDownTime--;
        if (nowCountDownTime > 0)
        {
            countDownText.SetText(nowCountDownTime.ToString());
            countDownText.transform.localScale = beforeScale;
            countDownText.transform.DOScale(5.0f, 1.0f).SetEase(Ease.InCubic);
            StartCoroutine(CountDownText(1.0f));
        }
        else
        {
            countDownText.SetText("Start");
            countDownText.transform.localScale = new Vector3(10.0f, 10.0f, 10.0f);
            countDownText.DOFade(0, 0.5f).SetEase(Ease.InCubic);
            isStop = false;
            GameManager.nowMiniGameManager.SetMiniGameStart();
        }

    }
}
