using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniGameManager : MonoBehaviour
{
    private GameManager gameManager; //ゲームマネージャー

    private GameObject onePlayer;                                                           //1人側プレイヤー
    private Dictionary<GameObject,bool> threePlayer = new Dictionary<GameObject, bool>();   //3人側プレイヤー(boolは死んだかどうか)

    private bool isPlayerAllDead = false;   //プレイヤーが全員死んでいるかどうか
    private bool isStart = false;           //ミニゲーム開始しているか
    private bool isFinish = false;          //ミニゲームが終了しているか

    /////////////////////////////////プレイヤー//////////////////////////////////////

    public void SetOnePlayer(GameObject player) { onePlayer = player; }
    public void SetThreePlayer(GameObject player) { threePlayer[player] = false; }
    public void playerDead(GameObject player) 
    { 
        //死んでいないのなら
        if(!threePlayer[player]) threePlayer[player] = true; 

    }

    /////////////////////////////////ミニゲーム情報//////////////////////////////////////

    public bool IsPlayerAllDead() {  return isPlayerAllDead; }
    public bool IsStart() {  return isStart; }
    public bool IsFinish() { return isFinish; }

    //ゲーム終了時に呼ばれる
    public void MiniGameFinish()
    {

    }

    //ゲーム開始時に呼ばれる
    public void MiniGameStart()
    {

    }
}
