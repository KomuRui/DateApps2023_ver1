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

    [SerializeField] private string miniGameName;  //ミニゲームシーンの名前

    ////////////////////////////////////プレイヤー情報////////////////////////////////////////////

    [SerializeField] protected GameObject onePlayerParent;                                   //1人側プレイヤーの親オブジェクト
    [SerializeField] protected Vector3 onePlayerPos;                                         //1人側プレイヤーの初期位置
    [SerializeField] protected Vector3 onePlayerScale;                                       //1人側プレイヤーの拡大率
    [SerializeField] protected Vector3 onePlayerRotate;                                      //1人側プレイヤーの角度
    [SerializeField] protected List<GameObject> threePlayerParent = new List<GameObject>();  //3人側プレイヤーの親オブジェクト
    [SerializeField] protected List<Vector3> threePlayerPos = new List<Vector3>();           //3人側プレイヤーの初期位置
    [SerializeField] protected List<Vector3> threePlayerScale = new List<Vector3>();         //3人側プレイヤーの拡大率
    [SerializeField] protected List<Vector3> threePlayerRotate = new List<Vector3>();        //3人側プレイヤーの角度

    [SerializeField] protected Image onePlayerImage;                                     //1人側プレイヤーの画像
    [SerializeField] protected List<Image> threePlayerImage = new List<Image>();         //3人側プレイヤーの画像

    [SerializeField] protected List<Vector3> rankAnnouncementPos = new List<Vector3>();     //ランク発表時のプレイヤー初期位置
    [SerializeField] protected List<Vector3> rankAnnouncementScale = new List<Vector3>();   //ランク発表時のプレイヤー拡大率
    [SerializeField] protected List<Vector3> rankAnnouncementRotate = new List<Vector3>();  //ランク発表時のプレイヤー角度

    protected GameObject onePlayerObj;                                      //1人側プレイヤーオブジェクト
    protected List<GameObject> threePlayerObj = new List<GameObject>();     //3人側プレイヤーオブジェクト
    protected bool isPlayerAllDead;                                                      //プレイヤーが全員死んでいるかどうか
    protected byte onePlayer;                                                            //1人側プレイヤー
    protected Dictionary<byte, bool> threePlayer = new Dictionary<byte, bool>();         //3人側プレイヤー(boolは死んだかどうか)

    ////////////////////////////////////カメラ////////////////////////////////////////////

    [SerializeField] protected Vector3 rankCameraPos;
    [SerializeField] protected Vector3 rankCameraRotate;

    ////////////////////////////////////必要UI////////////////////////////////////////////

    public GameObject endText;          //終了テキスト
    public GameObject rankText;         //順位テキスト
    public List<GameObject> killCanvas; //固有のキャンバス(各ミニゲームに表示してるUI,結果発表の時に消したいキャンバス)

    ////////////////////////////////////ミニゲーム情報////////////////////////////////////////////

    public Dictionary<byte, byte> nowMiniGameRank = new Dictionary<byte, byte>(); //現在のミニゲームのランク表(key : プレイヤー番号)
    protected bool isStart;             //ミニゲーム開始しているか
    protected bool isFinish;            //ミニゲームが終了しているか
    protected bool nowRankAnnouncement; //順位発表しているかどうか


    //アルファ版
    [SerializeField] protected List<GameObject> testPlayer = new List<GameObject>(); 
    [SerializeField] protected List<Image> testImage = new List<Image>(); 
    protected Dictionary<GameObject,Image> testImageTable = new Dictionary<GameObject, Image>();

    void Start()
    {
        /////////////////////////////////α版だけ
        PlayerManager.Initializ();
        ScoreManager.Initializ();

        /////初期化
        GameManager.nowMiniGameManager = this;
        nowRankAnnouncement = false;　
        isPlayerAllDead = false;
        isStart = false;
        isFinish = false;
        SceneStart();

        for(int i = 0; i < testPlayer.Count; i++)
        {
            testImageTable[testPlayer[i]] = testImage[i];
        }
        //各プレイヤー番号設定
        //onePlayer = PlayerManager.GetOnePlayer();

        //List<byte> threeP = PlayerManager.GetThreePlayer();
        //foreach(byte num in threeP)
        //    threePlayer[num] = false;

        ////プレイヤーと画像生成
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

    //更新
    void Update()
    {
        //順位発表しているかつBボタンが押されたのなら
        if (nowRankAnnouncement && Input.GetButtonDown("Bbutton1"))
        {
            StageSelectManager.NextRound();
            ScoreManager.ReCalcRank();
            SceneManager.LoadScene("MainMode");
        }

        //継承先の更新
        MiniGameUpdate();
    }

    /////////////////////////////////プレイヤー//////////////////////////////////////

    public void SetOnePlayer(byte player) { onePlayer = player; }
    public void SetThreePlayer(byte player) { threePlayer[player] = false; }
    public void PlayerFinish(byte player) 
    { 
        //死んでいないのなら
        if(!threePlayer[player]) threePlayer[player] = true; 

        //1人でも死んでいなかったらこの先処理しない
        foreach(var item in threePlayer.Values)
            if(!threePlayer[player]) return;

        //プレイヤー全員死んだに設定
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


    //プレイヤーすべて削除
    public void PlayerAllDelete()
    {
        //Destroy(onePlayerParent.gameObject);
        //for(int i = 0; i < threePlayerParent.Count; i++)
        //    Destroy(threePlayerParent[i].gameObject);
    }

    /////////////////////////////////ミニゲーム情報//////////////////////////////////////

    public bool IsPlayerAllDead() {  return isPlayerAllDead; }
    public bool IsStart() {  return isStart; }
    public bool IsFinish() { return isFinish; }

    //ミニゲーム開始にセット
    public void SetMiniGameStart() { isStart = true; MiniGameStart(); }

    //ミニゲーム終了にセット
    public void SetMiniGameFinish() 
    {
        //すでに終わっているならこの先処理しない
        if (isFinish) return;

        isFinish = true; 
        isStart = false;
        endText = Instantiate(endText, new Vector3(0, 0, 0), Quaternion.identity);
        MiniGameFinish();

        //4秒後に順位発表に移行
        Invoke("ChangeRankAnnouncement", 4.0f);
    }

    //順位発表に変更
    public void ChangeRankAnnouncement()
    {
        nowRankAnnouncement = true; 

        //順位テキスト表示
        for (byte i = 0; i < nowMiniGameRank.Count; i++)
        {
            rankText.transform.GetChild(i).GetComponent<TextMeshProUGUI>().text = nowMiniGameRank.Values.ElementAt(i).ToString() + "位 " 
            + ScoreManager.GetRankScore(nowMiniGameRank.Keys.ElementAt(i),nowMiniGameRank.Values.ElementAt(i)) + "P";
        }
        Instantiate(rankText, new Vector3(0, 0, 0), Quaternion.identity);

        //新たにプレイヤー召喚
        for (byte i = 0; i < nowMiniGameRank.Count; i++)
        {
            GameObject obj = (GameObject)Resources.Load(PlayerManager.GetPlayerVisual(nowMiniGameRank.Keys.ElementAt(i)));
            obj = Instantiate(obj, rankAnnouncementPos[i], Quaternion.identity);
            obj.transform.position = rankAnnouncementPos[i];
            obj.transform.localScale = rankAnnouncementScale[i];
            obj.transform.localEulerAngles = rankAnnouncementRotate[i];
        }

        //カメラのもろもろ変更
        Camera.main.transform.position = rankCameraPos;
        Camera.main.transform.localEulerAngles = rankCameraRotate;

        //プレイヤーとテキスト削除
        PlayerAllDelete();
        endText.SetActive(false);
        foreach(var obj in killCanvas)
            obj.SetActive(false);
    }

    //シーン開始
    public virtual void SceneStart() {}

    //更新
    public virtual void MiniGameUpdate() {}

    //ゲーム終了時に呼ばれる
    public virtual void MiniGameFinish(){}

    //ゲーム開始時に呼ばれる
    public virtual void MiniGameStart(){}

    //プレイヤーが全員死んだときに呼ばれる関数
    public virtual void PlayerAllDead(){}
}
