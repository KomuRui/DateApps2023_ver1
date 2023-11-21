using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniGameManager : MonoBehaviour
{
    [SerializeField] protected GameObject onePlayerParent;                                   //1�l���v���C���[�̐e�I�u�W�F�N�g(Player1,PLayer2....�݂����Ȃ��)
    [SerializeField] protected Vector3 onePlayerPos;                                         //1�l���v���C���[�̏����ʒu
    [SerializeField] protected Vector3 onePlayerScale;                                       //1�l���v���C���[�̊g�嗦
    [SerializeField] protected Vector3 onePlayerRotate;                                      //1�l���v���C���[�̊p�x
    [SerializeField] protected List<GameObject> threePlayerParent = new List<GameObject>();  //3�l���v���C���[�̐e�I�u�W�F�N�g(Player1,PLayer2....�݂����Ȃ��)
    [SerializeField] protected List<Vector3> threePlayerPos = new List<Vector3>();           //3�l���v���C���[�̏����ʒu
    [SerializeField] protected List<Vector3> threePlayerScale = new List<Vector3>();         //3�l���v���C���[�̊g�嗦
    [SerializeField] protected List<Vector3> threePlayerRotate = new List<Vector3>();        //3�l���v���C���[�̊p�x

    [SerializeField] protected Image onePlayerImage;                                    //1�l���v���C���[�̉摜
    [SerializeField] protected List<Image> threePlayerImage = new List<Image>();        //3�l���v���C���[�̉摜

    protected byte onePlayer;                                                           //1�l���v���C���[
    protected Dictionary<byte,bool> threePlayer = new Dictionary<byte, bool>();         //3�l���v���C���[(bool�͎��񂾂��ǂ���)

    protected bool isPlayerAllDead;   //�v���C���[���S������ł��邩�ǂ���
    protected bool isStart;           //�~�j�Q�[���J�n���Ă��邩
    protected bool isFinish;          //�~�j�Q�[�����I�����Ă��邩

    void Start()
    {
        /////////////////////////////////���ł���

        PlayerManager.Initializ();

        /////������
        isPlayerAllDead = false;
        isStart = false;
        isFinish = false;

        //�e�v���C���[�ԍ��ݒ�
        onePlayer = PlayerManager.GetOnePlayer();

        List<byte> threeP = PlayerManager.GetThreePlayer();
        foreach(byte num in threeP)
            threePlayer[num] = false;

        //�v���C���[�Ɖ摜����
        GameObject obj = (GameObject)Resources.Load(PlayerManager.GetPlayerVisual(onePlayer));
        obj = Instantiate(obj, onePlayerPos, Quaternion.identity);
        obj.transform.position = onePlayerPos;
        obj.transform.localScale = onePlayerScale;
        obj.transform.localEulerAngles = onePlayerRotate;
        obj.transform.parent = onePlayerParent.transform;
        

        onePlayerImage.sprite = Resources.Load<Sprite>(PlayerManager.GetPlayerVisualImage(onePlayer));

        int i = 0;
        foreach (byte num in threePlayer.Keys)
        {
            obj = (GameObject)Resources.Load(PlayerManager.GetPlayerVisual(num));
            obj = Instantiate(obj, this.transform.position, Quaternion.identity);
            obj.transform.position = threePlayerPos[i];
            obj.transform.localScale = threePlayerScale[i];
            obj.transform.localEulerAngles = threePlayerRotate[i];
            obj.transform.parent = threePlayerParent[i].transform;


            threePlayerImage[i].sprite = Resources.Load<Sprite>(PlayerManager.GetPlayerVisualImage(num));
            i++;
        }

    }

    /////////////////////////////////�v���C���[//////////////////////////////////////

    public void SetOnePlayer(byte player) { onePlayer = player; }
    public void SetThreePlayer(byte player) { threePlayer[player] = false; }
    public void playerDead(byte player) 
    { 
        //����ł��Ȃ��̂Ȃ�
        if(!threePlayer[player]) threePlayer[player] = true; 

        //1�l�ł�����ł��Ȃ������炱�̐揈�����Ȃ�
        foreach(var item in threePlayer.Values)
            if(!threePlayer[player]) return;

        //�v���C���[�S�����񂾂ɐݒ�
        isPlayerAllDead = true;
        PlayerAllDead();
    }

    /////////////////////////////////�~�j�Q�[�����//////////////////////////////////////

    public bool IsPlayerAllDead() {  return isPlayerAllDead; }
    public bool IsStart() {  return isStart; }
    public bool IsFinish() { return isFinish; }

    //�~�j�Q�[���J�n�ɃZ�b�g
    public void SetMiniGameStart() { isStart = true; MiniGameStart(); }

    //�~�j�Q�[���I���ɃZ�b�g
    public void SetMiniGameFinish() { isFinish = true; isStart = false; MiniGameFinish(); }

    //�Q�[���I�����ɌĂ΂��
    public virtual void MiniGameFinish(){}

    //�Q�[���J�n���ɌĂ΂��
    public virtual void MiniGameStart(){}

    //�v���C���[���S�����񂾂Ƃ��ɌĂ΂��֐�
    public virtual void PlayerAllDead(){}
}
