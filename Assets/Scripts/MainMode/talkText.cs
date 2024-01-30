using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class talkText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] public List<string> talk = new List<string>();
    [SerializeField] private float interval;       //インターバル
    [SerializeField] private GameObject nextImage; //次への画像
    protected int nowLookTalkNum = 0;  //現在見ている会話の要素番号
    protected int nowLookTextNum = 0;  //現在見ている文字の要素番号
    protected bool isTalkChangeWait = false;   //会話変更待機するか
    protected Dictionary<string, bool> isNextTalk = new Dictionary<string, bool>();

    // Start is called before the first frame update
    void Start()
    {
        //止まらないようにする
        for(int i = 0; i < talk.Count; i++)
            isNextTalk[talk[i]] = true;

        ChildStart();
    }

    // Update is called once per frame
    void Update()
    {
        //もし会話変更待機中にAボタンが押されたのなら次の会話へ
        if (isTalkChangeWait && Input.GetButtonDown("Abutton1"))
        {
            nextImage.SetActive(false);
            NextTalk();
        }

        ChildUpdate();
    }
    
    //次の文字表示
    IEnumerator nextTextPrint(float delay)
    {
        yield return new WaitForSeconds(delay);
     
        //会話終わったのなら
        if (nowLookTextNum >= talk[nowLookTalkNum].Length)
        {
            if (isNextTalk[talk[nowLookTalkNum]])
            {
                NextImageActive();
            }
            else
                TalkFinish();
        }
        else
            AddNextText();

    }

    //テキスト空にする
    IEnumerator TextClear(float delay)
    {
        yield return new WaitForSeconds(delay);
        text.text = "";
    }

    //次の会話へ
    private void NextTalk()
    {
        //すべての会話終了したら
        if (nowLookTalkNum >= talk.Count)
        {
            isTalkChangeWait = false;
            AllTalkFinish();
            return;
        }

        isTalkChangeWait = false;
        nowLookTextNum = 0;
        StartCoroutine(TextClear(0));
        StartCoroutine(nextTextPrint(0));
    }

    //次の文字追加
    private void AddNextText()
    {
        text.text += talk[nowLookTalkNum][nowLookTextNum];
        nowLookTextNum++;
        StartCoroutine(nextTextPrint(interval));
    }

    //次への画像をアクティブに
    public void NextImageActive()
    {
        nowLookTalkNum++;
        nextImage.SetActive(true);
        isTalkChangeWait = true;
    }

    //話すのスタート
    public void StartTalk()
    {
        StartCoroutine(nextTextPrint(2));
    }

    //すべての会話終了したときの処理
    public virtual void AllTalkFinish() { }

    //各トークが終了したときに呼ばれる関数(次の会話にいかないと設定している場合だけ)
    public virtual void TalkFinish() { }

    //子供用のスタート
    public virtual void ChildStart() { }

    //子供用の更新
    public virtual void ChildUpdate() { }

}
