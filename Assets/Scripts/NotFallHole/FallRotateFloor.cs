using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FallRotateFloor : MonoBehaviour
{

    [SerializeField] public int playerNum = 1;
    [SerializeField] private string buttonName = "Abutton";
    [SerializeField] private int sige = 1;
    [SerializeField] private NotFallHoleGameManager mana;

    List<Tweener> tweener = new List<Tweener>();

    public Vector3 rotationAxis = Vector3.right;
    private Quaternion initialRotation;
    private float rotateSpeed = 100.0f;
    private float flashingTime = 0.5f;
    private float warningTime = 2.0f;
    private bool isPush = false;
    public bool isRotate = false;

    //回転に使う符号
    public int rotateSign;

    // Start is called before the first frame update
    void Start()
    {
        rotateSign = 1;

        //初期回転を設定
        initialRotation = transform.rotation;

    }

    // Update is called once per frame
    void Update()
    {
        //開始していないか終わっているのなら
        if (!GameManager.nowMiniGameManager.IsStart() || GameManager.nowMiniGameManager.IsFinish()) return;

        //任意のボタンが押されたら
        if (Input.GetButtonDown(buttonName) && !isPush && mana.AddCount())
        {

            if (Input.GetButton("RBbutton" + playerNum))
                rotateSign = -1;
            else
                rotateSign = 1;

            rotateSpeed = Mathf.Abs(rotateSpeed) * Mathf.Sign(rotateSign);

            // 親オブジェクトの回転に追従するローカル回転軸を計算
            Vector3 worldRotationAxis = this.transform.parent.TransformDirection(rotationAxis);

            //押されたに変更
            isPush = true;

            //メッシュレンダラーを取得
            MeshRenderer r = this.transform.GetChild(0).GetComponent<MeshRenderer>();
            for (int i = 0; i < r.materials.Length - 1; i++)
                tweener.Add(r.materials[i].DOColor(Color.red, flashingTime).SetLoops(-1, LoopType.Yoyo));

            //指定時間後に中の処理を呼ぶ
            DOVirtual.DelayedCall(
                warningTime,
                () => {

                    //フラッシュを止める
                    for (int i = 0; i < tweener.Count; i++)
                    {
                        tweener[i].Restart();
                        tweener[i].Pause();
                    }

                    //回転開始
                    isRotate = true;

                    //コルーチン
                    StartCoroutine(WaitRotate(1.0f));
                }
            );
           
        }

        //回転しているのなら
        if(isRotate)
        {
            // 親オブジェクトの回転に追従するローカル回転軸を計算
            Vector3 worldRotationAxis = this.transform.parent.TransformDirection(rotationAxis);

            // 回転する
            transform.RotateAround(this.transform.position, worldRotationAxis, sige * rotateSpeed * Time.deltaTime);
     
        }


    }

    public void SetPlayerNum(byte num) { buttonName += num; }

    IEnumerator WaitRotate(float delay)
    {
        yield return new WaitForSeconds(delay);

        //回転終了
        isRotate = false;

        //コルーチン
        StartCoroutine(ReverseRotate(2.0f));
    }

    IEnumerator ReverseRotate(float delay)
    {
        yield return new WaitForSeconds(delay);

        //逆回転になるように
        rotateSpeed *= -1;

        //回転開始
        isRotate = true;

        //コルーチン
        StartCoroutine(FinishRotate(1.0f));
        StartCoroutine(AddOpenFloor(0.5f));
    }

    IEnumerator FinishRotate(float delay)
    {
        yield return new WaitForSeconds(delay);

        //逆回転になるように
        rotateSpeed *= -1;

        //最初の状態に戻す
        transform.rotation = transform.parent.rotation;

        //回転終了
        isRotate = false;
        isPush = false;
    }

    IEnumerator AddOpenFloor(float delay)
    {
        yield return new WaitForSeconds(delay);

        mana.MinusCount();
    }
}
