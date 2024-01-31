using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameManager
{
    public static MiniGameManager nowMiniGameManager; //���݂̃~�j�Q�[���Ǘ�
    public static bool isTitleStart = false; //�^�C�g������n�߂Ă邩�ǂ���
    public static bool isSubMode = false;

    //������
    public static void Initializ()
    {
        isTitleStart = true;
        PlayerManager.Initializ();
        ScoreManager.Initializ();
        StageSelectManager.Initializ();
    }
}
