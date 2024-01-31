using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
//using System.Numerics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

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
    [SerializeField] private StageSelect stageSelectInfo;

    [SerializeField] private List<Transform> resultTrans = new List<Transform>();

    [SerializeField] private List<TextMeshProUGUI> resultTextList = new List<TextMeshProUGUI>();


    private GameObject generatipnBonusObj; //生成したボーナスオブジェ
    private bool isTalkFinish = false;


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
        isNextTalk[talk[3]] = true;
        isNextTalk[talk[4]] = true;
        isNextTalk[talk[5]] = true;
        isNextTalk[talk[6]] = true;
        isNextTalk[talk[7]] = true;
        isNextTalk[talk[8]] = true;
        isNextTalk[talk[9]] = true;
        isNextTalk[talk[10]] = true;
        isNextTalk[talk[12]] = false;

        //トークスタート
        StartTalk();
    }

    //子供用のUpdate
    public override void ChildUpdate()
    {
        //会話が終了してなければ終わる
        if (!isTalkFinish) return;

        //ボタンを押していなかったら終わる
        if (!Input.GetButtonDown("Abutton1")) return;

            //フェードが情報あるのなら
            if (stageSelectInfo.GetFade())
            stageSelectInfo.GetFade().FadeOut(1.0f);

        //シーン変更
        StartCoroutine(ChangeScene("ModeSelect", 1f));
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

        ////アニメーション
        //leftMetor.transform.DOLocalMoveX(2, 2.0f).SetEase(Ease.OutQuart);
        //rightMetor.transform.DOLocalMoveX(2, 2.0f).SetEase(Ease.OutQuart);
        //talkSignBorad.transform.DOMoveY(25, 2.0f).SetEase(Ease.OutQuart);
        //mc.transform.DOMoveZ(30, 2.0f).SetEase(Ease.OutQuart);

        ////ポイント初期化
        //for(int i = 0; i < PlayerManager.PLAYER_MAX; i++)
        //    scoreGenerationMetor.ScoreMetorSet(i, 0, ScoreManager.GetScore((byte)(i + 1)));

        ////ポイント加算スタート
        //scoreGenerationMetor.GenerationStart();

        //最終発表のアニメーション
        ResultAnimation();
    }

    //最終発表のアニメーション
    void ResultAnimation()
    {
        //順位再計算
        ScoreManager.ReCalcRank();

        //1位のプレイヤーを取得
        List<byte> topPlayerList = ScoreManager.GetNominatePlayerRank(1);

        int val = Factorial(topPlayerList.Count) - topPlayerList.Count;

        int kaisu = 0;
        //アニメーション
        foreach (byte playerNum in topPlayerList)
        {
            //現在の角度を保存
            Vector3 tmp = stageSelectInfo.playerList[playerNum - 1].transform.localEulerAngles;

            //リザルトの位置を向く
            stageSelectInfo.playerList[playerNum - 1].transform.LookAt(
                new Vector3 (resultTrans[val + kaisu].position.x, stageSelectInfo.playerList[playerNum - 1].transform.position.y, resultTrans[val + kaisu].position.x));

            //角度を保存
            Vector3 goalTrans = stageSelectInfo.playerList[playerNum - 1].transform.localEulerAngles;

            //最初の角度に戻す
            stageSelectInfo.playerList[playerNum - 1].transform.localEulerAngles = tmp;

            //回転した後に移動する
            stageSelectInfo.playerList[playerNum - 1].transform.DOLocalRotate(new Vector3(goalTrans.x, goalTrans.y, goalTrans.z), 1f).SetEase(Ease.OutQuad);
            StartCoroutine(ResultAnimationMove((byte)(playerNum - 1), resultTrans[val + kaisu].position, 1f));

            kaisu++;
        }
    }

    //結果発表の回転アニメーション
    void ResultAnimationRotate(byte num)
    {
            stageSelectInfo.playerList[num].transform.DOLocalRotate(new Vector3(0, 180, 0), 1f).SetEase(Ease.OutQuad).OnComplete(() => ResultAnimationText());
    }

    //結果発表の移動アニメーション
    IEnumerator ResultAnimationMove(byte num, Vector3 targetPos, float delay)
    {
        yield return new WaitForSeconds(delay);
        //移動アニメーションの後に回転
        stageSelectInfo.playerList[num].transform.DOMove(targetPos, 2.0f).SetEase(Ease.OutCubic).OnComplete(() => ResultAnimationRotate(num));
    }
    
    public int Factorial(int n)
    {
        if (n == 0)
            return 0;
        return n + Factorial(n - 1);
    }

    //結果発表のテキストアニメーション
    void ResultAnimationText()
    {
        //1位のプレイヤーを取得
        List<byte> topPlayerList = ScoreManager.GetNominatePlayerRank(1);

        //勝ったプレイヤーを表示
        string winPlayerText = "";
        for (int i = 0; i < topPlayerList.Count; i++)
        {
            //勝ったプレイヤーが1人じゃなかったら
            if (i != 0)
                winPlayerText += "&";

            winPlayerText += topPlayerList[i] + "P";
        }
        resultTextList[0].SetText(winPlayerText);
        resultTextList[1].SetText("Win!");

        //拡大
        Vector3 afterScale = transform.localScale + new Vector3(0.45f, 0.45f, 0.45f);
        resultTextList[1].transform.DOScale(afterScale, 1f).SetEase(Ease.OutBounce).OnComplete(() => SetIsTalkFinish(true, 1f));

        isTalkFinish = true;
    }

    //シーン変更
    public IEnumerator ChangeScene(string scene, float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(scene);
    }

    public IEnumerator SetIsTalkFinish(bool a, float delay)
    {
        yield return new WaitForSeconds(delay);
        isTalkFinish = a;
    }
}
