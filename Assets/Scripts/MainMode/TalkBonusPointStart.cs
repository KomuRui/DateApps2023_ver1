using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkBonusPointStart : talkText
{

    [SerializeField] private Vector3 bonusGenerationPos; //ボーナス生成位置
    [SerializeField] private Vector3 bonusMoveInitializPos;  //ボーナス移動先初期位置
    [SerializeField] private List<Vector3> bonusPlayerMovePos; //ボーナス移動位置
    [SerializeField] private List<Vector3> playerPos;          //プレイヤー位置
    [SerializeField] private GameObject bonusPoint;      //ボーナスポイント
    [SerializeField] private GameObject mc;              //司会
    [SerializeField] private GameObject leftMetor;
    [SerializeField] private GameObject rightMetor;
    [SerializeField] private GameObject talkSignBorad;
    [SerializeField] private ScoreGenerationMetor scoreGenerationMetor;
    private GameObject generatipnBonusObj; //生成したボーナスオブジェ

    //子供用のスタート
    public override void ChildStart() 
    {
        isNextTalk.Clear();

        talk.Add("sajisjaisjiajsiajisajijsaij0");  //さて、皆さん、ここでサプライズです！
        talk.Add("sajisjaisjiajsiajisajijsaij1");  //なんと、最後に勝敗を大きく左右するかもしれないボーナスポイントを用意しています！
        talk.Add("sajisjaisjiajsiajisajijsaij2");  //1つずつ紹介していきます！
        talk.Add("sajisjaisjiajsiajisajijsaij3");  //1つ目の○○○○のボーナスポイントを獲得したプレイヤーは、、、
        talk.Add("sajisjaisjiajsiajisajijsaij4");  //○Pです！！
        talk.Add("sajisjaisjiajsiajisajijsaij5");  //2つ目の○○○○のボーナスポイントを獲得したプレイヤーは、、、
        talk.Add("sajisjaisjiajsiajisajijsaij6");  //○Pです！！
        talk.Add("sajisjaisjiajsiajisajijsaij7");  //3つ目の○○○○のボーナスポイントを獲得したプレイヤーは、、、
        talk.Add("sajisjaisjiajsiajisajijsaij8");  //○Pです！！
        talk.Add("sajisjaisjiajsiajisajijsaij9");  //4つ目の○○○○のボーナスポイントを獲得したプレイヤーは、、、
        talk.Add("sajisjaisjiajsiajisajijsaij10"); //○Pです！！
        talk.Add("sajisjaisjiajsiajisajijsaij11"); //さて、ここで最終的な結果を発表します！
        talk.Add("sajisjaisjiajsiajisajijsaij12"); //ボーナスポイントを加算し、最終的な勝者となったのは....！

        //止まらないようにする
        for (int i = 0; i < talk.Count; i++)
            isNextTalk[talk[i]] = true;

        //トークが終わったタイミングで演出を挟みたい時をfalseにする
        isNextTalk[talk[3]] = false;
        isNextTalk[talk[4]] = false;
        isNextTalk[talk[5]] = false;
        isNextTalk[talk[6]] = false;
        isNextTalk[talk[7]] = false;
        isNextTalk[talk[8]] = false;
        isNextTalk[talk[9]] = false;
        isNextTalk[talk[10]] = false;
        isNextTalk[talk[12]] = false;

        //トークスタート
        StartTalk();
    }

    //各トークが終了したときに呼ばれる関数(次の会話にいかないと設定している場合だけ)
    public override void TalkFinish() 
    {
        //偶数ならプレイヤーのもとへ移動
        if (nowLookTalkNum == 12)
            StartCoroutine(AllPointAddStart(1.0f));
        else if (nowLookTalkNum % 2 == 0)
            StartCoroutine(BonusToPlayerMpve(1.0f));
        else
            //ポイント生成
            StartCoroutine(BonusGeneration(1.0f));
    }

    //生成
    IEnumerator BonusGeneration(float delay)
    {
        yield return new WaitForSeconds(delay);
        generatipnBonusObj = Instantiate(bonusPoint, bonusGenerationPos, Quaternion.identity);
        generatipnBonusObj.transform.localScale = Vector3.zero;
        generatipnBonusObj.transform.localEulerAngles = new Vector3(-90,0,0);
        generatipnBonusObj.transform.DOMove(bonusMoveInitializPos, 1.0f).SetEase(Ease.OutCubic);
        generatipnBonusObj.transform.DOScale(Vector3.one * 0.55f, 1.0f).SetEase(Ease.OutCubic).OnComplete(() => NextImageActive());
    }

    //ボーナスをプレイヤーへ移動
    IEnumerator BonusToPlayerMpve(float delay)
    {
        yield return new WaitForSeconds(delay);
        
        var sequence = DOTween.Sequence(); 
        sequence.Append(generatipnBonusObj.transform.DOMove(bonusPlayerMovePos[0], 1.0f).SetEase(Ease.OutCubic))
            　　.AppendInterval(0.5f)
            　　.Append(generatipnBonusObj.transform.DOMove(playerPos[0], 1.0f).SetEase(Ease.OutCubic).OnComplete(() => NextImageActive()))
                .Join(generatipnBonusObj.transform.DOScale(Vector3.zero, 1.0f).SetEase(Ease.OutCubic));
    }

    //ポイント加算開始
    IEnumerator AllPointAddStart(float delay)
    {
        yield return new WaitForSeconds(delay);

        //アニメーション
        leftMetor.transform.DOLocalMoveX(2, 2.0f).SetEase(Ease.OutQuart);
        rightMetor.transform.DOLocalMoveX(2, 2.0f).SetEase(Ease.OutQuart);
        talkSignBorad.transform.DOMoveY(25, 2.0f).SetEase(Ease.OutQuart);
        mc.transform.DOMoveZ(30, 2.0f).SetEase(Ease.OutQuart);

        //ポイント初期化
        for(int i = 0; i < PlayerManager.PLAYER_MAX; i++)
            scoreGenerationMetor.ScoreMetorSet(i, 0, ScoreManager.GetScore((byte)(i + 1)));

        //ポイント加算スタート
        scoreGenerationMetor.GenerationStart();
    }
}
