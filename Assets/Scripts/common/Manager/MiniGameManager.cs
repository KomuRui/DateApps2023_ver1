using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MiniGameManager : MonoBehaviour
{

    ////////////////////////////////////プレイヤー情報////////////////////////////////////////////

    [SerializeField] protected GameObject onePlayerParent;                                   //1人側プレイヤーの親オブジェクト(Player1,PLayer2....みたいなやつ)
    [SerializeField] protected Vector3 onePlayerPos;                                         //1人側プレイヤーの初期位置
    [SerializeField] protected Vector3 onePlayerScale;                                       //1人側プレイヤーの拡大率
    [SerializeField] protected Vector3 onePlayerRotate;                                      //1人側プレイヤーの角度
    [SerializeField] protected List<GameObject> threePlayerParent = new List<GameObject>();  //3人側プレイヤーの親オブジェクト(Player1,PLayer2....みたいなやつ)
    [SerializeField] protected List<Vector3> threePlayerPos = new List<Vector3>();           //3人側プレイヤーの初期位置
    [SerializeField] protected List<Vector3> threePlayerScale = new List<Vector3>();         //3人側プレイヤーの拡大率
    [SerializeField] protected List<Vector3> threePlayerRotate = new List<Vector3>();        //3人側プレイヤーの角度

    [SerializeField] protected Image onePlayerImage;                                     //1人側プレイヤーの画像
    [SerializeField] protected List<Image> threePlayerImage = new List<Image>();         //3人側プレイヤーの画像

    [SerializeField] protected List<Vector3> rankAnnouncementPos = new List<Vector3>();     //ランク発表時のプレイヤー初期位置
    [SerializeField] protected List<Vector3> rankAnnouncementScale = new List<Vector3>();   //ランク発表時のプレイヤー拡大率
    [SerializeField] protected List<Vector3> rankAnnouncementRotate = new List<Vector3>();  //ランク発表時のプレイヤー角度

    protected bool isPlayerAllDead;                                                      //プレイヤーが全員死んでいるかどうか
    protected byte onePlayer;                                                            //1人側プレイヤー
    protected Dictionary<byte, bool> threePlayer = new Dictionary<byte, bool>();         //3人側プレイヤー(boolは死んだかどうか)

    ////////////////////////////////////カメラ////////////////////////////////////////////

    [SerializeField] protected Vector3 rankCameraPos;
    [SerializeField] protected Vector3 rankCameraRotate;

    ////////////////////////////////////必要UI////////////////////////////////////////////

    public GameObject endText;      //終了テキスト
    public GameObject rankText;     //順位テキスト
    public GameObject normalCanvas; //固有のキャンバス(各ミニゲームに表示してるUI,結果発表の時に消したいキャンバス)

    ////////////////////////////////////ミニゲーム情報////////////////////////////////////////////

    public Dictionary<byte, byte> nowMiniGameRank = new Dictionary<byte, byte>(); //現在のミニゲームのランク表(key : プレイヤー番号)
    protected bool isStart;           //ミニゲーム開始しているか
    protected bool isFinish;          //ミニゲームが終了しているか

    void Start()
    {
        /////////////////////////////////α版だけ

        PlayerManager.Initializ();
        ScoreManager.Initializ();

        /////初期化
        SceneStart();
        GameManager.nowMiniGameManager = this;
        isPlayerAllDead = false;
        isStart = false;
        isFinish = false;

        //各プレイヤー番号設定
        onePlayer = PlayerManager.GetOnePlayer();

        List<byte> threeP = PlayerManager.GetThreePlayer();
        foreach(byte num in threeP)
            threePlayer[num] = false;

        //プレイヤーと画像生成
        GameObject obj = (GameObject)Resources.Load(PlayerManager.GetPlayerVisual(onePlayer));
        obj = Instantiate(obj, onePlayerPos, Quaternion.identity);
        obj.transform.position = onePlayerPos;
        obj.transform.localScale = onePlayerScale;
        obj.transform.localEulerAngles = onePlayerRotate;
        obj.transform.parent = onePlayerParent.transform;
        obj.transform.parent.GetComponent<PlayerNum>().playerNum = onePlayer;

        if (onePlayerImage != null)
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
            obj.transform.parent.GetComponent<PlayerNum>().playerNum = num;

            if (threePlayerImage[i] != null)
                threePlayerImage[i].sprite = Resources.Load<Sprite>(PlayerManager.GetPlayerVisualImage(num));
            i++;
        }

    }

    /////////////////////////////////プレイヤー//////////////////////////////////////

    public void SetOnePlayer(byte player) { onePlayer = player; }
    public void SetThreePlayer(byte player) { threePlayer[player] = false; }
    public void PlayerDead(byte player) 
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

    //プレイヤーすべて削除
    public void PlayerAllDelete()
    {
        Destroy(onePlayerParent.gameObject);
        for(int i = 0; i < threePlayerParent.Count; i++)
            Destroy(threePlayerParent[i].gameObject);
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

        //順位テキスト表示
        for (byte i = 0; i < nowMiniGameRank.Count; i++)
        {
            rankText.transform.GetChild(i).GetComponent<TextMeshProUGUI>().text = nowMiniGameRank.Values.ElementAt(i).ToString();
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
        normalCanvas.SetActive(false);
    }

    //シーン開始
    public virtual void SceneStart() { }

    //ゲーム終了時に呼ばれる
    public virtual void MiniGameFinish(){}

    //ゲーム開始時に呼ばれる
    public virtual void MiniGameStart(){}

    //プレイヤーが全員死んだときに呼ばれる関数
    public virtual void PlayerAllDead(){}
}
