using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniGameManager : MonoBehaviour
{

    private byte onePlayer;                            //1人側プレイヤー
    private Dictionary<byte,bool> threePlayer;         //3人側プレイヤー(boolは死んだかどうか)

    private bool isPlayerAllDead;   //プレイヤーが全員死んでいるかどうか
    private bool isStart;           //ミニゲーム開始しているか
    private bool isFinish;          //ミニゲームが終了しているか

    void Start()
    {
        /////初期化
        threePlayer = new Dictionary<byte, bool>();
        isPlayerAllDead = false;
        isStart = false;
        isFinish = false;
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

        isPlayerAllDead = true;
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
