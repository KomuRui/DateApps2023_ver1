using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkBonusPointStart : talkText
{

    [SerializeField] private Vector3 bonusGenerationPos; //ボーナス生成位置
    [SerializeField] private List<Vector3> bonusMovePos; //ボーナス移動位置
    [SerializeField] private GameObject bonusPoint;      //ボーナスポイント
    [SerializeField] private GameObject mc;              //司会

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
        isNextTalk[talk[5]] = false;
        isNextTalk[talk[7]] = false;
        isNextTalk[talk[9]] = false;

        //トークスタート
        StartTalk();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //各トークが終了したときに呼ばれる関数(次の会話にいかないと設定している場合だけ)
    public override void TalkFinish() 
    {
        //ポイント生成
        GameObject point = Instantiate(bonusPoint, bonusGenerationPos, Quaternion.identity);
        point.transform.localScale = Vector3.zero;
        point.transform.DOMove(bonusMovePos[0], 1.0f).SetEase(Ease.OutCubic);
        point.transform.DOScale(Vector3.one * 0.55f, 1.0f).SetEase(Ease.OutCubic);
    }
}
