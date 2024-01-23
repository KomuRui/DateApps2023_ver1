using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkPointStart : talkText
{
    [SerializeField] private GameObject talkSignBorad;
    [SerializeField] private GameObject mc;
    [SerializeField] private ScoreGenerationMetor scoreGenerationMetor;
    [SerializeField] private GameObject leftMetor;
    [SerializeField] private GameObject rightMetor;
    [SerializeField] private GameObject nextTalk;
    private bool isReturnPos = false;

    //子供用のスタート
    public override void ChildStart() 
    { 
         StartTalk();
    }

    //子供用の更新
    public override void ChildUpdate()
    {
        if (scoreGenerationMetor.IsGeneratioonFinish() && !isReturnPos)
            ReturnPosGameObject();
    }

    //すべての会話終了したときの処理
    public override void AllTalkFinish()
    {
        //アニメーション
        leftMetor.transform.DOLocalMoveX(2, 2.0f).SetEase(Ease.OutQuart);
        rightMetor.transform.DOLocalMoveX(2, 2.0f).SetEase(Ease.OutQuart);
        talkSignBorad.transform.DOMoveY(25, 2.0f).SetEase(Ease.OutQuart);
        mc.transform.DOMoveZ(30, 2.0f).SetEase(Ease.OutQuart);

        //ポイント加算スタート
        scoreGenerationMetor.GenerationStart();
    }

    //元の位置に戻す
    private void ReturnPosGameObject()
    {
        isReturnPos = true;
        this.gameObject.SetActive(false);
        leftMetor.transform.DOLocalMoveX(-20, 2.0f).SetEase(Ease.OutQuart);
        rightMetor.transform.DOLocalMoveX(20, 2.0f).SetEase(Ease.OutQuart);
        talkSignBorad.transform.DOMoveY(0.8f, 2.0f).SetEase(Ease.OutQuart);
        mc.transform.DOMoveZ(15.6f, 2.0f).SetEase(Ease.OutQuart).OnComplete(TalkStart);
    }

    //話スタート
    private void TalkStart()
    {
        nextTalk.SetActive(true);
        nextTalk.GetComponent<talkText>().StartTalk();
    }
}
