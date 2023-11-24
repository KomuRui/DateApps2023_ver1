using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public static class StageSelectManager
{

    private static Dictionary<GameObject, bool> stage = new Dictionary<GameObject, bool>(); //GameObject : �e�X�e�[�W�̉摜 bool : �X�e�[�W���V�΂ꂽ���ǂ���
    private static int nowRound = 1; //���݂̃��E���h
    private const int MAX_ROUND = 4; //�ő僉�E���h��
    
    //������
    public static void Initializ()
    {
        nowRound = 1;
    }

    //���݂̃��E���h�����擾
    public static int GetNowRound() { return nowRound; }

    //���̃��E���h��
    public static void NextRound() 
    {
        //�����ő僉�E���h���ȏ�ɂȂ��Ă���̂Ȃ�΂��̐揈�����Ȃ�
        if (nowRound >= MAX_ROUND) return;

        nowRound++; 
    }

}
