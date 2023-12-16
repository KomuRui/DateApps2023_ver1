using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using System;

public class CountDownAndTimer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timeText;       //���Ԑ����e�L�X�g
    [SerializeField] private TextMeshProUGUI countDownText;  //�J�E���g�_�E���e�L�X�g
    [SerializeField] private Fade fade;  //�t�F�[�h

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
        //���Ԍv�Z
        TimeCalc();
    }

    //���Ԍv�Z
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

    //���Ƃ�
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
