using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class DriveChaseFishGameManager : MiniGameManager
{
    public Dictionary<int, int> fiscScore = new Dictionary<int, int>();
    public FishManager fishManager;
    public List<Pool> pool = new List<Pool>();
    public List<MeshRenderer> poolImageOnePlayerMesh = new List<MeshRenderer>();
    public List<MeshRenderer> poolImageThreePlayerMesh = new List<MeshRenderer>();


    //�V�[���J�n
    public override void SceneStart() {

        //�v���C���[�̔ԍ��Ɗe�v�[���̔ԍ��������ɂȂ�悤�ɓK�p
        pool[0].playerNum = onePlayerObj.GetComponent<PlayerNum>().playerNum;
        for(int i = 0; i < threePlayerObj.Count; i++)
        {
            pool[i + 1].playerNum = threePlayerObj[i].GetComponent<PlayerNum>().playerNum;
        }

        //�e�v�[���̃v���C���[�摜��K�p
        for(int i = 0; i < poolImageOnePlayerMesh.Count; i++)
        {
            poolImageOnePlayerMesh[i].material.mainTexture = Resources.Load<Texture>(PlayerManager.GetPlayerVisualImage((byte)pool[0].playerNum));
            poolImageThreePlayerMesh[i].material.mainTexture = Resources.Load<Texture>(PlayerManager.GetPlayerVisualImage((byte)pool[i + 1].playerNum));
        }
    }

    //�~�j�Q�[���X�^�[�g
    public override void MiniGameStart()
    {
        //�e�v���C���[�̓��_������
        for (int i = 0; i < PlayerManager.PLAYER_MAX; i++)
            fiscScore[i + 1] = 0;
    }

    //�X�V
    public override void MiniGameUpdate()
    {
    }

    //�Q�[���I�����ɌĂ΂��
    public override void MiniGameFinish()
    {
        int threePlayer = fiscScore[pool[1].playerNum] + fiscScore[pool[2].playerNum] + fiscScore[pool[3].playerNum];
        int onePlayer = fiscScore[pool[0].playerNum];

        bool isWinOnePLayer = false;

        //1�l�����������̂Ȃ�
        if (threePlayer <= onePlayer)
        {
            ScoreManager.AddScore(onePlayerObj.GetComponent<PlayerNum>().playerNum, 1);
            isWinOnePLayer = true;
        }
        else
            ScoreManager.AddScore(onePlayerObj.GetComponent<PlayerNum>().playerNum, 4);

        //3�l���̓��_���\�[�g�ŕ��ѕς���
        var dict = new Dictionary<int, int>();
        for (int i = 1; i < pool.Count; i++)
            dict.Add(pool[i].playerNum, fiscScore[pool[i].playerNum]);

        var sortedDictionary = dict.OrderByDescending(pair => pair.Value);

        //���ʂ��m�F
        byte nowRank = (isWinOnePLayer ? (byte)1 : (byte)0);
        byte sameRank = 1;
        byte lookNum = 1;
        float beforeValue = -1;
        foreach (var item in sortedDictionary)
        {
            if (beforeValue != item.Value)
            {
                nowRank += sameRank;
                sameRank = 1;
            }
            else
                sameRank++;

            beforeValue = item.Value;
            ScoreManager.AddScore((byte)item.Key, nowRank);
            lookNum++;
        }
    }

    //���_���Z
    public void FishScorePlus(int playerNum,int fishSum) { fiscScore[playerNum] += fishSum; }
}
