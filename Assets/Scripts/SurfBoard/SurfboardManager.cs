using System.Collections;
using System.Collections.Generic;
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
    }
}
