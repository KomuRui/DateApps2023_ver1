using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class StartAnimation : MonoBehaviour
{
    [SerializeField] private GameObject startObj;                                  //スタートの背景
    [SerializeField] private GameObject startBackGround;                           //スタートの背景
    [SerializeField] private List<Image> startBackGroundImage = new List<Image>(); //スタートの背景
    [SerializeField] private List<Image> startTextImage = new List<Image>();       //スタート文字の画像
    [SerializeField] private Vector3 initializScaleBackGround;
    [SerializeField] private Vector3 initializScaleText;
    private List<Vector3> beforeScaleText = new List<Vector3>();
    private Vector3 beforeScaleBackGround;
    private int lookNum = 0;

    void Start()
    {

        
    }

    void Update()
    {
        
    }

    //アニメーション
    public IEnumerator StartBackGroundAnimation(float delay)
    {
        yield return new WaitForSeconds(delay);

        StartInitializ();
        startBackGround.transform.DOScale(beforeScaleBackGround, 0.5f).SetEase(Ease.OutBack);
        for (int i = 0; i < startBackGroundImage.Count; i++)
        {
            startBackGroundImage[i].DOFade(1, 0.5f).SetEase(Ease.OutQuad);
        }
        StartCoroutine(StartTextImage(0.5f));
    }

    IEnumerator StartTextImage(float delay)
    {
        yield return new WaitForSeconds(delay);
        startTextImage[lookNum].transform.DOScale(beforeScaleText[lookNum], 0.3f).SetEase(Ease.OutBack);
        startTextImage[lookNum].DOFade(1, 0.3f).SetEase(Ease.OutQuad);
        startTextImage[lookNum].transform.DORotate(new Vector3(0,0,360), 0.3f).SetEase(Ease.OutQuad);
        lookNum++;

        if (lookNum < startTextImage.Count)
            StartCoroutine(StartTextImage(0.1f));
        else
            StartCoroutine(StartOutSide(1.0f));
    }

    IEnumerator StartOutSide(float delay)
    {
        yield return new WaitForSeconds(delay);

        // DoTweenのシーケンスを作成
        Sequence sequence = DOTween.Sequence();
        sequence.Append(startObj.transform.DOScaleY(1.3f, 0.2f).SetEase(Ease.OutQuad));
        sequence.Append(startObj.transform.DOScaleY(0, 0.3f).SetEase(Ease.OutQuad)).OnComplete(()=> GameManager.nowMiniGameManager.SetMiniGameStart());
        
    }

    private void StartInitializ()
    {
        this.gameObject.SetActive(true);

        //全部透明に
        for (int i = 0; i < startTextImage.Count; i++)
        {
            Color c = startTextImage[i].color;
            Color c1 = startBackGroundImage[i].color;
            c.a = 0.0f;
            c1.a = 0.0f;
            startTextImage[i].color = c;
            beforeScaleText.Add(startTextImage[i].transform.localScale);
            startTextImage[i].transform.localScale = initializScaleText;
            startBackGroundImage[i].color = c1;
        }

        //拡大率保存
        beforeScaleBackGround = startBackGround.transform.localScale;
        startBackGround.transform.localScale = initializScaleBackGround;
    }
}
