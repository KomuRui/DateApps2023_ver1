using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MiniGameManager : MonoBehaviour
{

    [SerializeField] private string miniGameName;  //�~�j�Q�[���V�[���̖��O

    ////////////////////////////////////�v���C���[���////////////////////////////////////////////

    [SerializeField] protected GameObject onePlayerParent;                                   //1�l���v���C���[�̐e�I�u�W�F�N�g
    [SerializeField] protected Vector3 onePlayerPos;                                         //1�l���v���C���[�̏����ʒu
    [SerializeField] protected Vector3 onePlayerScale;                                       //1�l���v���C���[�̊g�嗦
    [SerializeField] protected Vector3 onePlayerRotate;                                      //1�l���v���C���[�̊p�x
    [SerializeField] protected List<GameObject> threePlayerParent = new List<GameObject>();  //3�l���v���C���[�̐e�I�u�W�F�N�g
    [SerializeField] protected List<Vector3> threePlayerPos = new List<Vector3>();           //3�l���v���C���[�̏����ʒu
    [SerializeField] protected List<Vector3> threePlayerScale = new List<Vector3>();         //3�l���v���C���[�̊g�嗦
    [SerializeField] protected List<Vector3> threePlayerRotate = new List<Vector3>();        //3�l���v���C���[�̊p�x

    [SerializeField] protected Image onePlayerImage;                                     //1�l���v���C���[�̉摜
    [SerializeField] protected List<Image> threePlayerImage = new List<Image>();         //3�l���v���C���[�̉摜

    [SerializeField] protected List<Vector3> rankAnnouncementPos = new List<Vector3>();     //�����N���\���̃v���C���[�����ʒu
    [SerializeField] protected List<Vector3> rankAnnouncementScale = new List<Vector3>();   //�����N���\���̃v���C���[�g�嗦
    [SerializeField] protected List<Vector3> rankAnnouncementRotate = new List<Vector3>();  //�����N���\���̃v���C���[�p�x

    protected GameObject onePlayerObj;                                      //1�l���v���C���[�I�u�W�F�N�g
    protected List<GameObject> threePlayerObj = new List<GameObject>();     //3�l���v���C���[�I�u�W�F�N�g
    protected bool isPlayerAllDead;                                                      //�v���C���[���S������ł��邩�ǂ���
    protected byte onePlayer;                                                            //1�l���v���C���[
    protected Dictionary<byte, bool> threePlayer = new Dictionary<byte, bool>();         //3�l���v���C���[(bool�͎��񂾂��ǂ���)

    ////////////////////////////////////�J����////////////////////////////////////////////

    [SerializeField] protected Vector3 rankCameraPos;
    [SerializeField] protected Vector3 rankCameraRotate;

    ////////////////////////////////////�K�vUI////////////////////////////////////////////

    public GameObject endText;          //�I���e�L�X�g
    public GameObject rankText;         //���ʃe�L�X�g
    public List<GameObject> killCanvas; //�ŗL�̃L�����o�X(�e�~�j�Q�[���ɕ\�����Ă�UI,���ʔ��\�̎��ɏ��������L�����o�X)

    ////////////////////////////////////�~�j�Q�[�����////////////////////////////////////////////

    public Dictionary<byte, byte> nowMiniGameRank = new Dictionary<byte, byte>(); //���݂̃~�j�Q�[���̃����N�\(key : �v���C���[�ԍ�)
    protected bool isStart;             //�~�j�Q�[���J�n���Ă��邩
    protected bool isFinish;            //�~�j�Q�[�����I�����Ă��邩
    protected bool nowRankAnnouncement; //���ʔ��\���Ă��邩�ǂ���


    //�A���t�@��
    [SerializeField] protected List<GameObject> testPlayer = new List<GameObject>(); 
    [SerializeField] protected List<Image> testImage = new List<Image>(); 
    protected Dictionary<GameObject,Image> testImageTable = new Dictionary<GameObject, Image>();

    void Start()
    {
        /////////////////////////////////���ł���
        PlayerManager.Initializ();
        ScoreManager.Initializ();

        /////������
        GameManager.nowMiniGameManager = this;
        nowRankAnnouncement = false;�@
        isPlayerAllDead = false;
        isStart = false;
        isFinish = false;
        SceneStart();

        for(int i = 0; i < testPlayer.Count; i++)
        {
            testImageTable[testPlayer[i]] = testImage[i];
        }
        //�e�v���C���[�ԍ��ݒ�
        //onePlayer = PlayerManager.GetOnePlayer();

        //List<byte> threeP = PlayerManager.GetThreePlayer();
        //foreach(byte num in threeP)
        //    threePlayer[num] = false;

        ////�v���C���[�Ɖ摜����
        //onePlayerObj = (GameObject)Resources.Load(PlayerManager.GetPlayerVisual(onePlayer));
        //onePlayerObj = Instantiate(onePlayerObj, onePlayerPos, Quaternion.identity);
        //onePlayerObj.transform.position = onePlayerPos;
        //onePlayerObj.transform.localScale = onePlayerScale;
        //onePlayerObj.transform.localEulerAngles = onePlayerRotate;
        //onePlayerObj.transform.GetComponent<PlayerNum>().playerNum = onePlayer;

        //if (onePlayerParent != null)
        //    onePlayerObj.transform.parent = onePlayerParent.transform;
           

        //if (onePlayerImage != null)
        //     onePlayerImage.sprite = Resources.Load<Sprite>(PlayerManager.GetPlayerVisualImage(onePlayer));

        //int i = 0;
        //foreach (byte num in threePlayer.Keys)
        //{
        //    threePlayerObj[i] = (GameObject)Resources.Load(PlayerManager.GetPlayerVisual(num));
        //    threePlayerObj[i] = Instantiate(threePlayerObj[i], this.transform.position, Quaternion.identity);
        //    threePlayerObj[i].transform.position = threePlayerPos[i];
        //    threePlayerObj[i].transform.localScale = threePlayerScale[i];
        //    threePlayerObj[i].transform.localEulerAngles = threePlayerRotate[i];
        //    threePlayerObj[i].transform.GetComponent<PlayerNum>().playerNum = num;

        //    if (threePlayerParent[i] != null)
        //        threePlayerObj[i].transform.parent = threePlayerParent[i].transform;
                

        //    if (threePlayerImage[i] != null)
        //        threePlayerImage[i].sprite = Resources.Load<Sprite>(PlayerManager.GetPlayerVisualImage(num));
        //    i++;
        //}
    }

    //�X�V
    void Update()
    {
        //���ʔ��\���Ă��邩��B�{�^���������ꂽ�̂Ȃ�
        if (nowRankAnnouncement && Input.GetButtonDown("Bbutton1"))
        {
            StageSelectManager.NextRound();
            ScoreManager.ReCalcRank();
            SceneManager.LoadScene("MainMode");
        }

        //�p����̍X�V
        MiniGameUpdate();
    }

    /////////////////////////////////�v���C���[//////////////////////////////////////

    public void SetOnePlayer(byte player) { onePlayer = player; }
    public void SetThreePlayer(byte player) { threePlayer[player] = false; }
    public void PlayerFinish(byte player) 
    { 
        //����ł��Ȃ��̂Ȃ�
        if(!threePlayer[player]) threePlayer[player] = true; 

        //1�l�ł�����ł��Ȃ������炱�̐揈�����Ȃ�
        foreach(var item in threePlayer.Values)
            if(!threePlayer[player]) return;

        //�v���C���[�S�����񂾂ɐݒ�
        isPlayerAllDead = true;
        PlayerAllDead();
        SetMiniGameFinish();
    }

    public void PlayerDead(GameObject player)
    {
        Color c = testImageTable[player].color;
        c.r = 0.2f;
        c.g = 0.2f;
        c.b = 0.2f;
        testImageTable[player].color = c;
    }

    public void PlayerHeal(GameObject player)
    {
        Color c = testImageTable[player].color;
        c.r = 1.0f;
        c.g = 1.0f;
        c.b = 1.0f;
        testImageTable[player].color = c;
    }


    //�v���C���[���ׂč폜
    public void PlayerAllDelete()
    {
        //Destroy(onePlayerParent.gameObject);
        //for(int i = 0; i < threePlayerParent.Count; i++)
        //    Destroy(threePlayerParent[i].gameObject);
    }

    /////////////////////////////////�~�j�Q�[�����//////////////////////////////////////

    public bool IsPlayerAllDead() {  return isPlayerAllDead; }
    public bool IsStart() {  return isStart; }
    public bool IsFinish() { return isFinish; }

    //�~�j�Q�[���J�n�ɃZ�b�g
    public void SetMiniGameStart() { isStart = true; MiniGameStart(); }

    //�~�j�Q�[���I���ɃZ�b�g
    public void SetMiniGameFinish() 
    {
        //���łɏI����Ă���Ȃ炱�̐揈�����Ȃ�
        if (isFinish) return;

        isFinish = true; 
        isStart = false;
        endText = Instantiate(endText, new Vector3(0, 0, 0), Quaternion.identity);
        MiniGameFinish();

        //4�b��ɏ��ʔ��\�Ɉڍs
        Invoke("ChangeRankAnnouncement", 4.0f);
    }

    //���ʔ��\�ɕύX
    public void ChangeRankAnnouncement()
    {
        nowRankAnnouncement = true; 

        //���ʃe�L�X�g�\��
        for (byte i = 0; i < nowMiniGameRank.Count; i++)
        {
            rankText.transform.GetChild(i).GetComponent<TextMeshProUGUI>().text = nowMiniGameRank.Values.ElementAt(i).ToString() + "�� " 
            + ScoreManager.GetRankScore(nowMiniGameRank.Keys.ElementAt(i),nowMiniGameRank.Values.ElementAt(i)) + "P";
        }
        Instantiate(rankText, new Vector3(0, 0, 0), Quaternion.identity);

        //�V���Ƀv���C���[����
        for (byte i = 0; i < nowMiniGameRank.Count; i++)
        {
            GameObject obj = (GameObject)Resources.Load(PlayerManager.GetPlayerVisual(nowMiniGameRank.Keys.ElementAt(i)));
            obj = Instantiate(obj, rankAnnouncementPos[i], Quaternion.identity);
            obj.transform.position = rankAnnouncementPos[i];
            obj.transform.localScale = rankAnnouncementScale[i];
            obj.transform.localEulerAngles = rankAnnouncementRotate[i];
        }

        //�J�����̂������ύX
        Camera.main.transform.position = rankCameraPos;
        Camera.main.transform.localEulerAngles = rankCameraRotate;

        //�v���C���[�ƃe�L�X�g�폜
        PlayerAllDelete();
        endText.SetActive(false);
        foreach(var obj in killCanvas)
            obj.SetActive(false);
    }

    //�V�[���J�n
    public virtual void SceneStart() {}

    //�X�V
    public virtual void MiniGameUpdate() {}

    //�Q�[���I�����ɌĂ΂��
    public virtual void MiniGameFinish(){}

    //�Q�[���J�n���ɌĂ΂��
    public virtual void MiniGameStart(){}

    //�v���C���[���S�����񂾂Ƃ��ɌĂ΂��֐�
    public virtual void PlayerAllDead(){}
}
