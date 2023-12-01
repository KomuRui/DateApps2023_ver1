using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SurfboardManager : MiniGameManager
{
    [SerializeField] List<SurfboardPlayer> threePlayerList;

    //Start�Ɠ���
    public override void SceneStart()
    {
    }

    //�Q�[���J�n���ɌĂ΂��
    public override void MiniGameStart()
    {
    }

    //Update�Ɠ���
    public override void MiniGameUpdate()
    {
        ///////////a�ł̂�////////////////

        bool flag = true;
        foreach (var item in threePlayerList)
        {
            if (!item.isDead)
            {
                flag = false;
            }
        }

        if (flag)
        {
            GameManager.nowMiniGameManager.SetMiniGameFinish();
        }
        //////////////////////////////////
       
        
    }

    //�~�j�Q�[�����I�������Ă΂��
    public override void MiniGameFinish()
    {
        //�P�l�������������ǂ���
        bool isWinOnePLayer = false;

        //�v���C���[�����ׂĎ���ł���̂Ȃ�
        if (isPlayerAllDead)
        {
            ScoreManager.AddScore(onePlayerObj.GetComponent<PlayerNum>().playerNum, 1);
            isWinOnePLayer = true;
        }
        else
            ScoreManager.AddScore(onePlayerObj.GetComponent<PlayerNum>().playerNum, 4);

        //���ʂ��m�F
        byte nowRank = (isWinOnePLayer ? (byte)2 : (byte)1);
        byte sameRank = 0;

        //�����c���Ă���l�ɏ��ʂ�����
        foreach (var player in threePlayer)
        {
            //�����Ă���̂Ȃ�
            if (!player.Value)
            {
                ScoreManager.AddScore(player.Key, nowRank);
                sameRank++;
            }
        }

        //3�l���̓��_���\�[�g�ŕ��ѕς���
        var sortedDictionary = lifeTime.OrderByDescending(pair => pair.Value);
        float beforeValue = -1;
        foreach (var item in sortedDictionary)
        {
            //�����Ă���̂Ȃ炱�̐揈�����Ȃ�
            if (!threePlayer[item.Key]) continue;

            //�O��̒l�ƈႤ�̂Ȃ��
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
}
