using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;


public class CarryToTheGoalGameManager : MiniGameManager
{

    //�v���C���[���C�t
    [SerializeField] private List<TextMeshProUGUI> lifeText;
    //public Dictionary<byte, TextMeshProUGUI> playerLifeText = new Dictionary<byte, TextMeshProUGUI>();
    //public Dictionary<byte, int> playerLife = new Dictionary<byte, int>();
    public Dictionary<GameObject, TextMeshProUGUI> playerLifeText = new Dictionary<GameObject, TextMeshProUGUI>();
    public Dictionary<GameObject, int> playerLife = new Dictionary<GameObject, int>();
    public GameObject[] player;
    public bool isGoal = false;
    public int kill = 0;

    // Start is called before the first frame update
    public override void SceneStart()
    {
        //int i = 0;
        //foreach (byte num in threePlayer.Keys)
        //{
        //    Debug.Log(num);
        //    playerLifeText[num] = lifeText[i];
        //    playerLife[num] = 2;
        //    i++;
        //}

        for (int i = 0; i < player.Length; i++)
        {
            Debug.Log(i);
            playerLife[player[i]] = 2;
            playerLifeText[player[i]] = lifeText[i];
        }
    }

    public override void MiniGameUpdate()
    {
        for (int i = 0; i < player.Length; i++)
            playerLifeText[player[i]].text = playerLife[player[i]].ToString();

        //foreach (byte num in threePlayer.Keys)
        // playerLifeText[num].text = playerLife[num].ToString();
    }

    public override void MiniGameFinish()
    {
        //bool isWinOnePlayer = false;

        ////1�l�����������̂Ȃ�
        //if (!isGoal)
        //{
        //    ScoreManager.AddScore(onePlayerObj.GetComponent<PlayerNum>().playerNum, 1);
        //    isWinOnePlayer = true;
        //}
        //else
        //    ScoreManager.AddScore(onePlayerObj.GetComponent<PlayerNum>().playerNum, 4);


    }

    public void Damege(GameObject player)
    {
        if (playerLife[player] <= 0) return;

        //���C�t�����炷
        playerLife[player]--;
        playerLife[player] = Mathf.Max(playerLife[player], 0);
        if (playerLife[player] <= 0)
        {
            GameManager.nowMiniGameManager.PlayerDead(player);
            player.GetComponent<CarryToTheGoalPlayer>().Dead();
        }
        ////���łɎ���ł���̂Ȃ炱�̐揈�����Ȃ�
        //if (playerLife[player] <= 0) return;

        ////���C�t�����炷
        //playerLife[player]--;
        //playerLife[player] = Mathf.Max(playerLife[player], 0);
        //if (playerLife[player] <= 0)
        //{
        //    for(int i = 0; i < threePlayerObj.Count; i++)
        //        if(player == threePlayerObj[i].GetComponent<PlayerNum>().playerNum) threePlayerObj[i].GetComponent<CarryToTheGoalPlayer>().Dead();
        //}
    }
}
