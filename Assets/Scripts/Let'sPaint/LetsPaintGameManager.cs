using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LetsPaintGameManager : MonoBehaviour
{
    [SerializeField] private PaintTarget target;
    [SerializeField] private TextMeshProUGUI[] playerPercentText; //パーセント
    [SerializeField] private TextMeshProUGUI timeText;          　//時間制限テキスト
    [SerializeField] private TextMeshProUGUI countDownText;       //カウントダウンテキスト

    private int[] playerPercent;
    private int nowCountDownTime = 3;
    private Vector3 beforeScale;
    private float time = 45.0f;
    private bool isStop = true;

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < playerPercentText.Length; i++)
            playerPercentText[i].SetText("0%");

        beforeScale = countDownText.transform.localScale;
        countDownText.transform.DOScale(5.0f, 1.0f).SetEase(Ease.InCubic);
        StartCoroutine(CountDownText(1.0f));
    }

    // Update is called once per frame
    void Update()
    {
        //各プレイヤーのパーセント計算
        playerPercentCalc();

        //時間計算
        TimeCalc();
    }

    //各プレイヤーのパーセント計算
    private void playerPercentCalc()
    {
        playerPercent = target.GetPercent(target);
        for (int i = 0; i < playerPercentText.Length; i++)
            playerPercentText[i].SetText(playerPercent[i].ToString() + "%");
    }

    //時間計算
    private void TimeCalc()
    {
        if (isStop) return;
        time -= Time.deltaTime;
        time = Mathf.Max(time, 0);
        timeText.text = ((int)time).ToString() + ":00";
    }

    //落とす
    IEnumerator CountDownText(float delay)
    {
        yield return new WaitForSeconds(delay);

        nowCountDownTime--;
        if(nowCountDownTime > 0)
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
        }

    }
}
