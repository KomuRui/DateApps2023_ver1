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
        //�{�^���̖��O��ݒ肵�Ă���
        buttonName += playerNum;
    }

    // Update is called once per frame
    void Update()
    {
        
        //��]���Ă���Ȃ炱�̐揈�����Ȃ�
        if (isRotate) return;

        //�C�ӂ̃{�^���������ꂽ��
        if(Input.GetButtonDown(buttonName))
        {
           
            //��]���Ă���ɕύX
            isRotate = true;

            //���b�V�������_���[���擾
            MeshRenderer r = this.transform.GetChild(0).GetComponent<MeshRenderer>();
            for (int i = 0; i < r.materials.Length - 1; i++)
                tweener.Add(r.materials[i].DOColor(Color.red, flashingTime).SetLoops(-1, LoopType.Yoyo));

            //�w�莞�Ԍ�ɒ��̏������Ă�
            DOVirtual.DelayedCall(
                warningTime,
                () => {

                    //�t���b�V�����~�߂�
                    for (int i = 0; i < tweener.Count; i++)
                    {
                        tweener[i].Restart();
                        tweener[i].Pause();
                    }

                    //��]
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
