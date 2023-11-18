using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniGameManager : MonoBehaviour
{
    [SerializeField] protected GameObject onePlayerParent;                                   //1人側プレイヤーの親オブジェクト(Player1,PLayer2....みたいなやつ)
    [SerializeField] protected List<GameObject> threePlayerParent = new List<GameObject>();  //3人側プレイヤーの親オブジェクト(Player1,PLayer2....みたいなやつ)

    protected byte onePlayer;                                                           //1人側プレイヤー
    protected Dictionary<byte,bool> threePlayer = new Dictionary<byte, bool>();         //3人側プレイヤー(boolは死んだかどうか)

    protected bool isPlayerAllDead;   //プレイヤーが全員死んでいるかどうか
    protected bool isStart;           //ミニゲーム開始しているか
    protected bool isFinish;          //ミニゲームが終了しているか

    void Start()
    {
        /////初期化
        isPlayerAllDead = false;
        isStart = false;
        isFinish = false;

        //各プレイヤー番号設定
        onePlayer = PlayerManager.GetOnePlayer();

        List<byte> threeP = PlayerManager.GetThreePlayer();
        foreach(byte num in threeP)
            threePlayer[num] = false;

        //プレイヤー生成
        GameObject obj = Instantiate(PlayerManager.GetPlayerVisual(onePlayer), this.transform.position, Quaternion.identity);
        obj.transform.parent = onePlayerParent.transform;

        int i = 0;
        foreach (byte num in threePlayer.Keys)
        {
           obj = Instantiate(PlayerManager.GetPlayerVisual(num), this.transform.position, Quaternion.identity);
           obj.transform.parent = threePlayerParent[i].transform;
           i++;
        }

    }

    /////////////////////////////////プレイヤー//////////////////////////////////////

    public void SetOnePlayer(byte player) { onePlayer = player; }
    public void SetThreePlayer(byte player) { threePlayer[player] = false; }
    public void playerDead(byte player) 
    { 
        //死んでいないのなら
        if(!threePlayer[player]) threePlayer[player] = true; 

        //1人でも死んでいなかったらこの先処理しない
        foreach(var item in threePlayer.Values)
            if(!threePlayer[player]) return;

        //プレイヤー全員死んだに設定
        isPlayerAllDead = true;
        PlayerAllDead();
    }

    /////////////////////////////////ミニゲーム情報//////////////////////////////////////

    public bool IsPlayerAllDead() {  return isPlayerAllDead; }
    public bool IsStart() {  return isStart; }
    public bool IsFinish() { return isFinish; }

    //ミニゲーム開始にセット
    public void SetMiniGameStart() { isStart = true; MiniGameStart(); }

    //ミニゲーム終了にセット
    public void SetMiniGameFinish() { isFinish = true; isStart = false; MiniGameFinish(); }

    //ゲーム終了時に呼ばれる
    public virtual void MiniGameFinish(){}

    //ゲーム開始時に呼ばれる
    public virtual void MiniGameStart(){}

    //プレイヤーが全員死んだときに呼ばれる関数
    public virtual void PlayerAllDead(){}
}
