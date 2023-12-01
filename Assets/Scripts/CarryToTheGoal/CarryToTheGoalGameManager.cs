using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;


public class CarryToTheGoalGameManager : MiniGameManager
{

    //プレイヤーライフ
    [SerializeField] private List<TextMeshProUGUI> lifeText;
    public Dictionary<byte, TextMeshProUGUI> playerLifeText = new Dictionary<byte, TextMeshProUGUI>();
    public Dictionary<byte, int> playerLife = new Dictionary<byte, int>();
    public LookOnTexture cannon;
    public bool isGoal = false;
    public int kill = 0;

    // Start is called before the first frame update
    public override void SceneStart()
    {
        cannon.playerNum = onePlayerObj.GetComponent<PlayerNum>().playerNum;

        int i = 0;
        foreach (byte num in threePlayer.Keys)
        {
            playerLifeText[num] = lifeText[i];
            playerLife[num] = 2;
            i++;
        }
    }

    public override void MiniGameUpdate()
    {

        foreach (byte num in threePlayer.Keys)
         playerLifeText[num].text = playerLife[num].ToString();
    }

    public override void MiniGameFinish()
    {
        bool isWinOnePlayer = false;

        //1人側が勝ったのなら
        if (!isGoal || isPlayerAllDead)
        {
            ScoreManager.AddScore(onePlayerObj.GetComponent<PlayerNum>().playerNum, 1);
            isWinOnePlayer = true;
        }
        else
            ScoreManager.AddScore(onePlayerObj.GetComponent<PlayerNum>().playerNum, 4);

        //順位を確認
        byte nowRank = (isWinOnePlayer ? (byte)2 : (byte)1);
        byte sameRank = 0;
        Dictionary<byte, int> life = new Dictionary<byte, int>();

        //生き残っている人に順位をつける
        foreach (var player in threePlayer)
        {
            //生きているのなら
            if (!player.Value)
            {
                life[player.Key] = playerLife[player.Key];
            }
        }

        //3人側の得点をソートで並び変える
        var sortedDictionary = life.OrderByDescending(pair => pair.Value);
        float beforeValue = -1;
        foreach (var item in sortedDictionary)
        {
            //前回の値と違うのならば
            if (beforeValue != item.Value)
            {
                nowRank += sameRank;
                sameRank = 1;
            }
            else
                sameRank++;

            beforeValue = item.Value;
            ScoreManager.AddScore(item.Key, nowRank);
        }

        var sortedDictionary2 = lifeTime.OrderByDescending(pair => pair.Value);
        foreach (var item in sortedDictionary2)
        {
            //生きているのならこの先処理しない
            if (!threePlayer[item.Key]) continue;

            //前回の値と違うのならば
            if (beforeValue != item.Value)
            {
                nowRank += sameRank;
                sameRank = 1;
            }
            else
                sameRank++;

            beforeValue = item.Value;
            ScoreManager.AddScore(item.Key, nowRank);
        }
    }

    public void Damege(byte player)
    {
        //すでに死んでいるのならこの先処理しない
        if (playerLife[player] <= 0) return;

        //ライフを減らす
        playerLife[player]--;
        playerLife[player] = Mathf.Max(playerLife[player], 0);
        if (playerLife[player] <= 0)
        {
            GameManager.nowMiniGameManager.PlayerDead(player);
            GameManager.nowMiniGameManager.PlayerFinish(player);

            for (int i = 0; i < threePlayerObj.Count; i++)
            {
                if (threePlayerObj[i] == null) continue;
                if (player == threePlayerObj[i].GetComponent<PlayerNum>().playerNum) threePlayerObj[i].GetComponent<CarryToTheGoalPlayer>().Dead();
            }
        }
    }
}
