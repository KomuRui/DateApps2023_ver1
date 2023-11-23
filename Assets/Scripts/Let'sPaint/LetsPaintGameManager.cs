using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Linq;

public class LetsPaintGameManager : MiniGameManager
{
    [SerializeField] private PaintTarget target;
    [SerializeField] private TextMeshProUGUI[] playerPercentText; //�p�[�Z���g

    private int[] playerPercent;


    // Update is called once per frame
    void Update()
    {
        //�e�v���C���[�̃p�[�Z���g�v�Z
        playerPercentCalc();
    }

    //�e�v���C���[�̃p�[�Z���g�v�Z
    private void playerPercentCalc()
    {
        playerPercent = target.GetPercent(target);
        for (int i = 0; i < playerPercentText.Length; i++)
            playerPercentText[i].SetText(playerPercent[i].ToString() + "%");
    }

    //�Q�[���I�����ɌĂ΂��
    public override void MiniGameFinish()
    {
        int threePlayer = playerPercent[0] + playerPercent[1] + playerPercent[2];
        int onePlayer = playerPercent[3];

        bool isWinOnePLayer = false;

        //1�l�����������̂Ȃ�
        if (threePlayer <= onePlayer)
        {
            ScoreManager.AddScore(onePlayerParent.GetComponent<PlayerNum>().playerNum, 1);
            isWinOnePLayer = true;
        }
        else
            ScoreManager.AddScore(onePlayerParent.GetComponent<PlayerNum>().playerNum, 4);

        //3�l���̓��_���\�[�g�ŕ��ѕς���
        var dict = new Dictionary<int, int>();
        for (int i = 0; i < playerPercent.Length; i++)
            dict.Add(i, playerPercent[i]);

        IOrderedEnumerable<KeyValuePair<int, int>> sorted = dict.OrderByDescending(pair => pair.Key);

        //���ʂ��m�F
        byte nowRank = (isWinOnePLayer ? (byte)1 : (byte)0);
        byte sameRank = 1;
        float beforeValue = -1;
        for (int i = 0; i < dict.Count; i++)
        {
            if (beforeValue != dict[i])
            {
                nowRank += sameRank;
                sameRank = 1;
            }
            else 
                sameRank++;

            beforeValue = dict[i];
            ScoreManager.AddScore(onePlayerParent.GetComponent<PlayerNum>().playerNum, nowRank);
        }
    }
}
