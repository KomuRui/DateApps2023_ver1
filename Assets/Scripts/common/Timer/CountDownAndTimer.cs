using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using System;

public class CountDownAndTimer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timeText;               //���Ԑ����e�L�X�g
    [SerializeField] private TextMeshProUGUI countDownText;          //�J�E���g�_�E���e�L�X�g
    [SerializeField] private TextMeshProUGUI tutorialTimeText;       //���Ԑ����e�L�X�g(�`���[�g���A���p)
    [SerializeField] private TextMeshProUGUI tutorialCountDownText;  //�J�E���g�_�E���e�L�X�g(�`���[�g���A���p)
    [SerializeField] public Fade fade;  //�t�F�[�h

    private int nowCountDownTime = 3;
    private Vector3 beforeScale;
    [SerializeField] public float time = 30.0f;
    private bool isStop = true;
    public bool isfinish = false;

    // Start is called before the first frame update
    void Start()
    {

        //�����`���[�g���A�����I����Ă���̂Ȃ�
        if (TutorialManager.isTutorialFinish && fade != null)
        {
            fade.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
            timeText.gameObject.SetActive(true);
            countDownText.gameObject.SetActive(true);
            tutorialTimeText.gameObject.SetActive(false);
            tutorialCountDownText.gameObject.SetActive(false);
        }
        else if (tutorialTimeText != null)
        {
            timeText = tutorialTimeText;
            countDownText = tutorialCountDownText;
        }

        //�J�E���g�_�E��
        beforeScale = countDownText.transform.localScale;
        countDownText.transform.DOScale(5.0f, 1.0f).SetEase(Ease.InCubic);

        //�t�F�[�h�����Ȃ��̂Ȃ�
        if (fade != null)
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
