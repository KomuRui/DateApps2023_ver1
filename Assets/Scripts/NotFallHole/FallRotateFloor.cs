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
    private Quaternion initialRotation;
    private float rotateSpeed = 100.0f;
    private float flashingTime = 0.5f;
    private float warningTime = 2.0f;
    private bool isPush = false;
    public bool isRotate = false;

    // Start is called before the first frame update
    void Start()
    {
        //�{�^���̖��O��ݒ肵�Ă���
        buttonName += playerNum;

        //������]��ݒ�
        initialRotation = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        
        //�C�ӂ̃{�^���������ꂽ��
        if(Input.GetButtonDown(buttonName) && !isPush)
        {
            // �e�I�u�W�F�N�g�̉�]�ɒǏ]���郍�[�J����]�����v�Z
            Vector3 worldRotationAxis = this.transform.parent.TransformDirection(rotationAxis);

            //�����ꂽ�ɕύX
            isPush = true;

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

                    //��]�J�n
                    isRotate = true;

                    //�R���[�`��
                    StartCoroutine(WaitRotate(1.0f));
                }
            );
           
        }

        //��]���Ă���̂Ȃ�
        if(isRotate)
        {
            // �e�I�u�W�F�N�g�̉�]�ɒǏ]���郍�[�J����]�����v�Z
            Vector3 worldRotationAxis = this.transform.parent.TransformDirection(rotationAxis);

            // ��]����
            transform.RotateAround(this.transform.position, worldRotationAxis, -sige * rotateSpeed * Time.deltaTime);
        
        }


    }

    IEnumerator WaitRotate(float delay)
    {
        yield return new WaitForSeconds(delay);

        //��]�I��
        isRotate = false;

        //�R���[�`��
        StartCoroutine(ReverseRotate(1.0f));
    }

    IEnumerator ReverseRotate(float delay)
    {
        yield return new WaitForSeconds(delay);

        //�t��]�ɂȂ�悤��
        rotateSpeed *= -1;

        //��]�J�n
        isRotate = true;

        //�R���[�`��
        StartCoroutine(FinishRotate(1.0f));
    }

    IEnumerator FinishRotate(float delay)
    {
        yield return new WaitForSeconds(delay);

        //�t��]�ɂȂ�悤��
        rotateSpeed *= -1;

        //�ŏ��̏�Ԃɖ߂�
        transform.rotation = transform.parent.rotation;

        //��]�I��
        isRotate = false;
        isPush = false;
    }
}
