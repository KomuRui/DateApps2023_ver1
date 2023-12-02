using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class StageSelectManager
{

    private static Dictionary<GameObject, bool> stage = new Dictionary<GameObject, bool>(); //GameObject : �e�X�e�[�W�̉摜 bool : �X�e�[�W���V�΂ꂽ���ǂ���
    private static int nowRound = 1;                                   �@     //���݂̃��E���h
    private const int MAX_ROUND = 4;                                   �@     //�ő僉�E���h��
    public static List<string> notPlayminiGameSceneName = new List<string>(); //�v���C���Ă��Ȃ��~�j�Q�[���̖��O
    public static List<string> playMiniGameSceneName = new List<string>();    //���łɃv���C���Ă���~�j�Q�[���̖��O
    public static string nowMiniGameName;
    public static bool isMainModeFinish;

    //������
    public static void Initializ()
    {
        nowMiniGameName = "";
        isMainModeFinish = false;
        nowRound = 1;
        notPlayminiGameSceneName.Add("AvoidFish");
        notPlayminiGameSceneName.Add("CarryToTheGoal");
        notPlayminiGameSceneName.Add("ConsecutiveChases");
        notPlayminiGameSceneName.Add("FlagUp");
        notPlayminiGameSceneName.Add("Let'sPaint");
        notPlayminiGameSceneName.Add("NotFallHole");
        notPlayminiGameSceneName.Add("NotHitStick");
        notPlayminiGameSceneName.Add("Surfboard");
    }

    //�~�j�Q�[���V�[���ɕύX
    public static void ChangeMiniGameScene()
    {
        nowMiniGameName = notPlayminiGameSceneName[Random.Range(0, notPlayminiGameSceneName.Count)];
        notPlayminiGameSceneName.Remove(nowMiniGameName);
        SceneManager.LoadScene(nowMiniGameName);
    }

    //���݂̃��E���h�����擾
    public static int GetNowRound() { return nowRound; }

    //���̃��E���h��
    public static void NextRound()
    {
        //�����ő僉�E���h���ȏ�ɂȂ��Ă���̂Ȃ�΂��̐揈�����Ȃ�
        if (nowRound >= MAX_ROUND)
        {
            isMainModeFinish = true;
            return;
        }

        nowRound++; 
    }

}
