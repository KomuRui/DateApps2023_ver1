using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class TalkMiniGameRandomStart : talkText
{
    [SerializeField] private GameObject mainMiniGameBoard;
    [SerializeField] private GameObject talkSignBorad;
    [SerializeField] private GameObject mc;
    [SerializeField] private StageSelect stageSelect;

    //子供用のスタート
    public override void ChildStart()
    {
        StartTalk();
    }

    //すべての会話終了したときの処理
    public override void AllTalkFinish() 
    {
        //アニメーション
        mainMiniGameBoard.transform.DOMoveY(-0.2f, 2.0f).SetEase(Ease.OutQuart);
        talkSignBorad.transform.DOMoveY(25, 2.0f).SetEase(Ease.OutQuart);
        mc.transform.DOMoveZ(30, 2.0f).SetEase(Ease.OutQuart);

        //ミニゲームランダムスタート
        StartCoroutine(stageSelect.MiniGameRandom(3.0f));
    }

}
