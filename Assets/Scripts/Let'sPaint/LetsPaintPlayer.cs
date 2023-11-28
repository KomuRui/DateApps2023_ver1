using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LetsPaintPlayer : MonoBehaviour
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
    [SerializeField] private float gravitySpeed = 0.05f;      // 重力速度
    [SerializeField] private float flashingTime;              //点滅時間
    [SerializeField] private bool isHorizontalInput = true;   // 横の入力許可するか
    [SerializeField] private bool isVerticalInput = true;     // 縦の入力許可するか
    [SerializeField] private bool isAnimIdle = true;
    [SerializeField] private bool isAnimWalk = true;
    [SerializeField] private bool isAnimJump = true;
    [SerializeField] private bool isAnimAttack = true;
    [SerializeField] private bool isAnimDamage = true;
    [SerializeField] private int playerNum;                   // プレイヤー番号
    [SerializeField] private Vector3 localGravity;
    private Rigidbody rBody;
    private Transform mainCameraTransform; // メインカメラのTransform

    private Vector3 initializPos;   //初期位置
    private bool isRespawn = false; //現在リスポーン中かどうか
    private Tweener tweener;        //Dotween用
    [SerializeField]  private ChildCol col;


    // Start is called before the first frame update
    void Start()
    {
        //初期位置設定
        initializPos = transform.position;

        // メインカメラを取得
        mainCameraTransform = Camera.main.transform;

        //リジットボディ取得
        rBody = this.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        //開始していないか終了しているのならこの先処理しない
        if (!GameManager.nowMiniGameManager.IsStart() || GameManager.nowMiniGameManager.IsFinish()) return;

        Move();
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
           // ChangeStateTo(SlimeAnimationState.Idle);
            return;
        }

        //歩き状態に変更
       // ChangeStateTo(SlimeAnimationState.Walk);

        // カメラの向きを基準にプレイヤーを移動
        Vector3 forwardDirection = mainCameraTransform.forward;
        Vector3 rightDirection = mainCameraTransform.right;
        forwardDirection.y = 0f; // Y軸成分を0にすることで水平方向に制限

        // 移動方向を計算
        Vector3 moveDirection = (forwardDirection.normalized * verticalInput + rightDirection.normalized * horizontalInput).normalized;

        // 移動
        rBody.AddForce(moveDirection * moveSpeed * Time.deltaTime);

        Quaternion newRotation = Quaternion.LookRotation(moveDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, rotationSpeed * Time.deltaTime);
    }

    //リスポーンスタート
    IEnumerator StartRespawn(float delay)
    {
        yield return new WaitForSeconds(delay);

        //初期位置決定
        transform.position = initializPos;

        var children = this.GetComponentsInChildren<SkinnedMeshRenderer>(true);
        for (int i = 0; i < children.Length; i++)
        {
            Color r = children[i].material.color;
            r.a = 0.6f;
            children[i].material.color = r;
        }

        var children2 = this.GetComponentsInChildren<MeshRenderer>(true);
        for (int i = 0; i < children2.Length; i++)
        {
            Color r = children2[i].material.color;
            r.a = 0.6f;
            children2[i].material.color = r;
        }

        //レンダリングモード変える
        Material material = this.GetComponent<MeshRenderer>().material;
        material.SetOverrideTag("RenderType", "Transparent");
        material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
        material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        material.SetInt("_ZWrite", 0);
        material.DisableKeyword("_ALPHATEST_ON");
        material.DisableKeyword("_ALPHABLEND_ON");
        material.EnableKeyword("_ALPHAPREMULTIPLY_ON");
        material.renderQueue = 3000;
      
        //色変える
        Color r2 = material.color;
        r2.a = 0.6f;
        material.color = r2;

        //力オフ
        rBody.velocity = Vector3.zero;

        //無効か
        this.gameObject.layer = 7;
        col.isMuteki = true;

        //コルーチン
        StartCoroutine(ReStart(1.0f));
    }

    //スタート
    IEnumerator ReStart(float delay)
    {
        yield return new WaitForSeconds(delay);

        //リスポート
        isRespawn = false;

        if (col.hitObj == null)
            ReturnAlpha();
    }

    public void ReturnAlpha()
    {
        var children = this.GetComponentsInChildren<SkinnedMeshRenderer>(true);
        for (int i = 0; i < children.Length; i++)
        {
            Color r = children[i].material.color;
            r.a = 1.0f;
            children[i].material.color = r;
        }

        var children2 = this.GetComponentsInChildren<MeshRenderer>(true);
        for (int i = 0; i < children2.Length; i++)
        {
            Color r = children2[i].material.color;
            r.a = 1.0f;
            children2[i].material.color = r;
        }

        //レンダリングモード変える
        Material material = this.GetComponent<MeshRenderer>().material;
        material.SetOverrideTag("RenderType", "");
        material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
        material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
        material.SetInt("_ZWrite", 1);
        material.DisableKeyword("_ALPHATEST_ON");
        material.DisableKeyword("_ALPHABLEND_ON");
        material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        material.renderQueue = 2000;

        //元に戻す
        Color r2 = material.color;
        r2.a = 1.0f;
        material.color = r2;

        col.isMuteki = false;
        this.gameObject.layer = 6;
        col.hitObj = null;
    }

    //何かと当たった時に呼ばれる関数
    void OnTriggerEnter(Collider collision)
    {
        if (collision.transform.tag == "Sea" && !isRespawn)
        {
            isRespawn = true;
            StartCoroutine(StartRespawn(2.0f));
        }
    }

}
