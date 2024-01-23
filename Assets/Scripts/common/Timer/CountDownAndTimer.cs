using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using System;
using UnityEngine.UI;
using Unity.VisualScripting;

public class CountDownAndTimer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timeText;               //時間制限テキスト
    [SerializeField] private Image timeImage;                        //時間制限テキスト
    [SerializeField] private StartAnimation startText;               //カウントダウンテキスト
    [SerializeField] public Fade fade;  //フェード
    [SerializeField] public Sprite timeImageRed;
    [SerializeField] public Sprite timeImageAlpha;

    private Tweener bounceTweenText;
    private Tweener bounceTweenImage;
    private Vector3 originalScale;
    private int beforeTime = 0;
    [SerializeField] public float time = 30.0f;
    public bool isfinish = false;

    // Start is called before the first frame update
    void Start()
    {
        if(timeText)
            originalScale = timeText.transform.localScale;

        //チュートリアルなら
        if(!TutorialManager.isTutorialFinish)
        {
            GameManager.nowMiniGameManager.SetMiniGameStart();
        }
        else
            StartCoroutine(startText.StartBackGroundAnimation(1.0f));

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
        if (!GameManager.nowMiniGameManager.IsStart() || isfinish || GameManager.nowMiniGameManager.IsFinish() || timeText == null) return;
        time -= Time.deltaTime;
        time = Mathf.Max(time, 0);
        timeText.text = ((int)time).ToString();
        if (time <= 1)
        {
            isfinish = true;
            timeText.text = "0";
            GameManager.nowMiniGameManager.SetMiniGameFinish();
        }
        if(((int)time) <= 5)
        {
            //赤色に変換
            timeImage.sprite = timeImageRed;

            //前回と時間が違うのなた
            if (beforeTime != ((int)time))
            {
                BounceAnimation();
                beforeTime = ((int)time);
            }

        }
    }

    //落とす
    IEnumerator CountDownText(float delay)
    {
        yield return new WaitForSeconds(delay);
    }

    //カウントダウンとタイマーを設定する
    public void SetCountDownAndTimer()
    {
        //もしチュートリアルが終わっているのなら
        if (TutorialManager.isTutorialFinish && fade != null)
        {
            fade.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;

            if (timeText != null) timeText.gameObject.SetActive(true);
            if (timeImage != null) timeImage.gameObject.SetActive(true);
        }

        //フェードが情報ないのなら
        if (fade != null)
            fade.FadeOut(1.0f);

        StartCoroutine(CountDownText(1.0f));
    }

    private void BounceAnimation()
    {
        //前回のアニメーション削除
        bounceTweenText.Kill();
        bounceTweenImage.Kill();

        //テキストのアニメーション
        timeText.transform.localScale = transform.localScale + new Vector3(3,3,3);
        bounceTweenText = timeText.transform.DOScale(originalScale, 1)
            .SetEase(Ease.OutBounce);

        //画像のアニメーション
        GameObject newImageObject = Instantiate(timeImage.gameObject, timeImage.transform.position, timeImage.transform.rotation, timeImage.transform.parent);
        newImageObject.transform.localScale = timeImage.transform.localScale;
        newImageObject.GetComponent<Image>().sprite = timeImageAlpha;

        //拡大
        Vector3 afterScale = transform.localScale + new Vector3(3.5f, 3.5f, 3.5f);
        bounceTweenImage = newImageObject.transform.DOScale(afterScale, 0.8f)
            .SetEase(Ease.OutQuad);

        //透明に
        newImageObject.GetComponent<Image>().DOFade(0f, 0.8f)
            .SetEase(Ease.OutQuad).OnComplete(() => Destroy(newImageObject.gameObject));
    }


}
