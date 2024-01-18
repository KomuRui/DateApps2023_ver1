using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ReachExtenderManager : MiniGameManager
{
    [SerializeField] VortexManager vortexManager;

    //Start�Ɠ���
    public override void SceneStart()
    {
    }

    //�Q�[���J�n���ɌĂ΂��
    public override void MiniGameStart()
    {
        vortexManager.StartVortexCoroutine();
    }

    //Update�Ɠ���
    public override void MiniGameUpdate()
    {
    }

    //�~�j�Q�[�����I�������Ă΂��
    public override void MiniGameFinish()
    {
        vortexManager.SetIsAppearanceVotex(false);
    }
}
