using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class DriveChaseFishGameManager : MiniGameManager
{
    public Dictionary<int, int> fiscScore = new Dictionary<int, int>();
    public FishManager fishManager;

    //シーン開始
    public override void SceneStart() {

       
    }

    //ミニゲームスタート
    public override void MiniGameStart()
    {
        //各プレイヤーの得点初期化
        for (int i = 0; i < PlayerManager.PLAYER_MAX; i++)
            fiscScore[i + 1] = 0;
    }

    //更新
    public override void MiniGameUpdate()
    {
    }

    //ゲーム終了時に呼ばれる
    public override void MiniGameFinish()
    {
    }

    //得点加算
    public void FishScorePlus(int playerNum,int fishSum) { fiscScore[playerNum] += fishSum; }
}
