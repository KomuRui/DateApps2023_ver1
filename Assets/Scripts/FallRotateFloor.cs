using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FallRotateFloor : MonoBehaviour
{

    [SerializeField] private int playerNum = 1;
    [SerializeField] private string buttonName = "Abutton";
    [SerializeField] private int sige = 1;
    List<Tweener> tweener = new List<Tweener>();

    public Vector3 rotationAxis = Vector3.right; 
    private float rotateTime = 1.0f;
    private float waitTime = 1.0f;
    private float flashingTime = 0.5f;
    private float warningTime = 2.0f;
    private bool isRotate = false;

    // Start is called before the first frame update
    void Start()
    {
        //ボタンの名前を設定しておく
        buttonName += playerNum;
    }

    // Update is called once per frame
    void Update()
    {
        
        //回転しているならこの先処理しない
        if (isRotate) return;

        //任意のボタンが押されたら
        if(Input.GetButtonDown(buttonName))
        {
           
            //回転しているに変更
            isRotate = true;

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

                    //回転
                    var sequence = DOTween.Sequence();
                    sequence.Append(this.transform.DORotate(rotationAxis * (sige * 80) + (Vector3.back * -90), rotateTime))
                            .Append(this.transform.DORotate((Vector3.back * -90), rotateTime).SetDelay(waitTime))
                            .AppendCallback(() =>
                            {
                                isRotate = false;
                            });
                }
            );
           
        }

    }
}
