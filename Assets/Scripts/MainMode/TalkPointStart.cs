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
}
