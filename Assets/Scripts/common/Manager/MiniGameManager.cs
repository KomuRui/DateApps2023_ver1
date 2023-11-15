using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniGameManager : MonoBehaviour
{
    private GameManager gameManager; //�Q�[���}�l�[�W���[

    private GameObject onePlayer;                                                           //1�l���v���C���[
    private Dictionary<GameObject,bool> threePlayer = new Dictionary<GameObject, bool>();   //3�l���v���C���[(bool�͎��񂾂��ǂ���)

    private bool isPlayerAllDead = false;   //�v���C���[���S������ł��邩�ǂ���
    private bool isStart = false;           //�~�j�Q�[���J�n���Ă��邩
    private bool isFinish = false;          //�~�j�Q�[�����I�����Ă��邩

    /////////////////////////////////�v���C���[//////////////////////////////////////

    public void SetOnePlayer(GameObject player) { onePlayer = player; }
    public void SetThreePlayer(GameObject player) { threePlayer[player] = false; }
    public void playerDead(GameObject player) 
    { 
        //����ł��Ȃ��̂Ȃ�
        if(!threePlayer[player]) threePlayer[player] = true; 

    }

    /////////////////////////////////�~�j�Q�[�����//////////////////////////////////////

    public bool IsPlayerAllDead() {  return isPlayerAllDead; }
    public bool IsStart() {  return isStart; }
    public bool IsFinish() { return isFinish; }

    //�Q�[���I�����ɌĂ΂��
    public void MiniGameFinish()
    {

    }

    //�Q�[���J�n���ɌĂ΂��
    public void MiniGameStart()
    {

    }
}
