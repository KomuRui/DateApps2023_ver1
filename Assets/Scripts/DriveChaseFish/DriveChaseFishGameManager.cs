using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class DriveChaseFishGameManager : MiniGameManager
{
    public Dictionary<int, int> fiscScore = new Dictionary<int, int>();
    public FishManager fishManager;

    //�V�[���J�n
    public override void SceneStart() {

       
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
    }

    //���_���Z
    public void FishScorePlus(int playerNum,int fishSum) { fiscScore[playerNum] += fishSum; }
}
