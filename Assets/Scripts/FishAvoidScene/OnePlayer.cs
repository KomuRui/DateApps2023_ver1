using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.AI;

using UnityEngine.UI;
using static UnityEngine.ParticleSystem;

public class OnePlayer : MonoBehaviour
{
    //アニメーションに必要
    public enum SlimeAnimationState { Idle, Walk, Jump, Attack, Damage }

    public Face faces;
    public GameObject SmileBody;
    public SlimeAnimationState currentState;

    public Animator animator;
    public int damType;

    private Material faceMaterial;

    [SerializeField] private float moveSpeed = 5.0f;          // プレイヤーの移動速度
    [SerializeField] private float rotationSpeed = 180.0f;    // プレイヤーの回転速度
    [SerializeField] private bool isHorizontalInput = true;   // 横の入力許可するか
    [SerializeField] private bool isVerticalInput = true;     // 縦の入力許可するか
    [SerializeField] private bool isAnimIdle = true;
    [SerializeField] private bool isAnimWalk = true;
    [SerializeField] private bool isAnimJump = true;
    [SerializeField] private bool isAnimAttack = true;
    [SerializeField] private bool isAnimDamage = true;
    [SerializeField] private int playerNum;                   // プレイヤー番号

    private Transform mainCameraTransform; // メインカメラのTransform



    public GameObject penguinP;
    public GameObject sharkP;
    public GameObject fishesP;
    public GameObject dolphinP;
    public GameObject penguinImage;
    public GameObject sharkImage;
    public GameObject dolphinImage;
    public GameObject fishesImage;
    GameObject penguinPre;
    GameObject sharkPre;
    GameObject fishesPre;
    GameObject dolphinPre;

    public float pinguinCoolTime;
    public float dolphinCoolTime;
    public float sharkCoolTime;
    public float fishesCoolTime;
    public float AllCoolTime;
    float penguinLeftTime = 1;
    float sharkLeftTime = 1;
    float fishesLeftTime = 1;
    float dolphinLeftTime = 1;
    float a = 2;
    float b = 1;


    Quaternion penguinRotate = Quaternion.Euler(0, -90, 90);
    Quaternion sharkRotate = Quaternion.Euler(0, 270, 0);
    Quaternion fishesRotate = Quaternion.Euler(0, 180, 0);
    Quaternion dolphinRotate = Quaternion.Euler(250, 180, 0);
    bool isPenguin;
    bool isShark;
    bool isFishes;
    bool isDolphin;
    bool isStop;
    bool isCoroutineStart;
    public GameObject PenguinImage;
    GameObject SharkImage;
    GameObject FishesImage;
    GameObject DolphinImage;


    // Start is called before the first frame update
    void Start()
    {
        //マテリアル設定
        faceMaterial = SmileBody.GetComponent<Renderer>().materials[1];

        // メインカメラを取得
        mainCameraTransform = Camera.main.transform;

        //Instantiate(penguinP, this.transform.position, penguinRotate);
        //penguinPre = Instantiate(penguinImage, new Vector3(557, 441, 0), Quaternion.Euler(0, 0, 0));
        isPenguin = true;
        isShark = true;
        isFishes = true;
        isDolphin = true;
        isStop = false;
        isCoroutineStart = false;
        SharkImage = GameObject.Find("SharkCoolTimeImage");
        FishesImage = GameObject.Find("FishesCoolTimeImage");
        DolphinImage = GameObject.Find("DolphinCoolTimeImage");
    }

    //顔のテクスチャ設定
    void SetFace(Texture tex)
    {
        faceMaterial.SetTexture("_MainTex", tex);
    }

    // Update is called once per frame
    void Update()
    {

        if (GameManager.nowMiniGameManager.IsStart() && !GameManager.nowMiniGameManager.IsFinish())
            //動き
            Move();

        //状態更新
        StateUpdata();

        //クールタイム
        if (isPenguin == false)
        {
            penguinLeftTime -= Time.deltaTime / pinguinCoolTime;
            //penguinPre.GetComponent<Image>().fillAmount = penguinLeftTime;
            //GameObject UI_Text = penguinPre.transform.Find("name").gameObject;
            penguinPre.transform.Find("PenguinCoolTimeImage").gameObject.GetComponent<Image>().fillAmount = penguinLeftTime;
            Debug.Log("a");
            if (penguinLeftTime < 0)
            {
                Destroy(penguinPre.GetComponent<Image>());
            }
        }
        if (isShark == false)
        {
            sharkLeftTime -= Time.deltaTime / sharkCoolTime;
            sharkPre.transform.Find("SharkCoolTimeImage").gameObject.GetComponent<Image>().fillAmount = sharkLeftTime;
            if (sharkLeftTime < 0)
            {
                Destroy(sharkPre.GetComponent<Image>());
            }
        }
        if (isFishes == false)
        {
            fishesLeftTime -= Time.deltaTime / fishesCoolTime;
            fishesPre.transform.Find("FishesCoolTimeImage").gameObject.GetComponent<Image>().fillAmount = fishesLeftTime;
            if (fishesLeftTime < 0)
            {
                Destroy(fishesPre.GetComponent<Image>());
            }
        }
        if (isDolphin == false)
        {
            dolphinLeftTime -= Time.deltaTime / dolphinCoolTime;
            dolphinPre.transform.Find("DolphinCoolTimeImage").gameObject.GetComponent<Image>().fillAmount = dolphinLeftTime;
            if (dolphinLeftTime < 0)
            {
                Destroy(dolphinPre.GetComponent<Image>());
            }
        }



        if (isStop)
        {
            if (!isCoroutineStart) 
            {
                StartCoroutine(AllCoolCorou());
            }           
        }//ペンギン
        else if ((Input.GetKeyDown(KeyCode.A) || Input.GetButtonDown("Abutton" + playerNum) )&& isPenguin)
        {
            Instantiate(penguinP, new Vector3(this.transform.position.x, -0.53f, 10), penguinRotate);
            penguinPre = Instantiate(penguinImage, new Vector3(0, 0, 0), Quaternion.identity);
            penguinLeftTime = 1;

            StartCoroutine(PenguinCoolCorou());
            isStop = true;
            
        }//サメ
        else if ((Input.GetKeyDown(KeyCode.S) || Input.GetButtonDown("Bbutton" + playerNum))&& isShark)
        {
            Instantiate(sharkP, new Vector3(this.transform.position.x, -1, 10), sharkRotate);
            sharkPre = Instantiate(sharkImage, new Vector3(0, 0, 0), Quaternion.identity);
            sharkLeftTime = 1;

            StartCoroutine(SharkCoolCorou());
            isStop = true;
        }//魚群
        else if ((Input.GetKeyDown(KeyCode.F) || Input.GetButtonDown("Xbutton" + playerNum))&& isFishes)
        {
            Instantiate(fishesP, new Vector3(this.transform.position.x, -0.9f, 10), fishesRotate);
            fishesPre = Instantiate(fishesImage, new Vector3(0, 0, 0), Quaternion.identity);
            fishesLeftTime = 1;

            StartCoroutine(FishesCoolCorou());
            isStop = true;
        }//イルカ
        else if ((Input.GetKeyDown(KeyCode.D) || Input.GetButtonDown("Ybutton" + playerNum))&& isDolphin)
        {
            Instantiate(dolphinP, new Vector3(this.transform.position.x, -3, 10), dolphinRotate);
            dolphinPre = Instantiate(dolphinImage, new Vector3(0, 0, 0), Quaternion.identity);
            dolphinLeftTime = 1;

            StartCoroutine(DolphinCoolCorou());
            isStop = true;
        }
    }


    //移動
    private void Move()
    {
        // 入力を取得用
        float horizontalInput = 0;
        float verticalInput = 0;

        // 入力を取得
        if (isHorizontalInput) horizontalInput = Input.GetAxis("L_Stick_H" + playerNum);
        if (isVerticalInput) verticalInput = -Input.GetAxis("L_Stick_V" + playerNum);

        //入力がないのなら
        if (horizontalInput == 0 && verticalInput == 0)
        {
            //通常状態に変更
            ChangeStateTo(SlimeAnimationState.Idle);
            return;
        }

        //歩き状態に変更
        ChangeStateTo(SlimeAnimationState.Walk);

        // カメラの向きを基準にプレイヤーを移動
        Vector3 forwardDirection = mainCameraTransform.forward;
        Vector3 rightDirection = mainCameraTransform.right;
        forwardDirection.y = 0f; // Y軸成分を0にすることで水平方向に制限

        // 移動方向を計算
        Vector3 moveDirection = (forwardDirection.normalized * verticalInput + rightDirection.normalized * horizontalInput).normalized;

        // 移動
        transform.position += moveDirection * moveSpeed * Time.deltaTime;
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, -3.5f, 3.5f), transform.position.y, transform.position.z);

        Quaternion newRotation = Quaternion.LookRotation(moveDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, rotationSpeed * Time.deltaTime);
    }

    //状態更新
    private void StateUpdata()
    {
        switch (currentState)
        {
            case SlimeAnimationState.Idle:

                if (animator.GetCurrentAnimatorStateInfo(0).IsName("Idle") || !isAnimIdle) return;

                currentState = SlimeAnimationState.Idle;
                animator.SetFloat("Speed", 0);
                SetFace(faces.Idleface);
                break;

            case SlimeAnimationState.Walk:

                if (animator.GetCurrentAnimatorStateInfo(0).IsName("Walk") || !isAnimWalk) return;

                currentState = SlimeAnimationState.Walk;
                animator.SetFloat("Speed", 1.0f);
                SetFace(faces.WalkFace);
                break;

            case SlimeAnimationState.Jump:

                if (animator.GetCurrentAnimatorStateInfo(0).IsName("Jump") || !isAnimJump) return;

                SetFace(faces.jumpFace);
                animator.SetTrigger("Jump");
                break;

            case SlimeAnimationState.Attack:

                if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack") || !isAnimAttack) return;
                SetFace(faces.attackFace);
                animator.SetTrigger("Attack");
                break;

            case SlimeAnimationState.Damage:

                if (animator.GetCurrentAnimatorStateInfo(0).IsName("Damage0")
                 || animator.GetCurrentAnimatorStateInfo(0).IsName("Damage1")
                 || animator.GetCurrentAnimatorStateInfo(0).IsName("Damage2")
                 || !isAnimDamage) return;

                animator.SetTrigger("Damage");
                animator.SetInteger("DamageType", damType);
                SetFace(faces.damageFace);
                break;
        }
    }

    public void ChangeStateTo(SlimeAnimationState state)
    {
        if (this == null) return;
        if (state == this.currentState) return;

        this.currentState = state;
    }


    //コルーチン関数を定義
    private IEnumerator AllCoolCorou() //コルーチン関数の名前
    {
        isCoroutineStart = true;
        yield return new WaitForSeconds(AllCoolTime);
        isStop = false;
        isCoroutineStart = false;
    }

    //ぺんぎん
    private IEnumerator PenguinCoolCorou() //コルーチン関数の名前
    {
        isPenguin = false;
        yield return new WaitForSeconds(pinguinCoolTime);
        isPenguin = true;
    }

    //さめ
    private IEnumerator SharkCoolCorou() //コルーチン関数の名前
    {
        isShark = false;
        yield return new WaitForSeconds(sharkCoolTime);
        isShark = true;
    }

    //魚群
    private IEnumerator FishesCoolCorou() //コルーチン関数の名前
    {
        isFishes = false;
        yield return new WaitForSeconds(fishesCoolTime);
        isFishes = true;
    }

    //いるか
    private IEnumerator DolphinCoolCorou() //コルーチン関数の名前
    {
        isDolphin = false;
        yield return new WaitForSeconds(dolphinCoolTime);
        isDolphin = true;
    }



    private IEnumerator DolphinCoolCoroua()
    {
        
        yield return new WaitForSeconds(1.0f);
    }
}
