using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class Floor : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI[] timeTextMeshPro; //�������ԃe�L�X�g
    [SerializeField] private float flashingTime;                //�_�Ŏ���

    //���ݐ�������
    private float time =10.0f;

    //�X�s�[�h�{��
    private float speedRatio = 1.0f;

    //�������Ă���v���C���[��ۊ�
    private List<GameObject> hitPlayer = new List<GameObject>();

    //Dotween�p
    private Tweener tweener;

    //�h�炵�Ă��邩
    private bool isShake = false;

    //�ԐF�ɕς��邩
    private bool isChangeRedColor = false;

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < timeTextMeshPro.Length; i++)
            timeTextMeshPro[i].text = "10";
    }

    // Update is called once per frame
    void Update()
    {
        //���Ԍv�Z�E�\��
        TimeCalcPrint();

        //�������Ԃ�������h�炷
        Shake();

        //�F�X�V
        ColorUpdate();
    }

    //���Ԍv�Z�E�\��
    private void TimeCalcPrint()
    {
        time -= Time.deltaTime * (speedRatio * hitPlayer.Count);
        time = Mathf.Max(time, 0);
        for (int i = 0; i < timeTextMeshPro.Length; i++)
            timeTextMeshPro[i].text = ((int)time).ToString();
    }

    //�h�炷
    private void Shake()
    {
        if (time != 0 || isShake) return;

        //�_�Ŏ~�߂�
        tweener.Restart();
        tweener.Pause();

        //�ԐF�ɂ��Ă���
        GetComponent<MeshRenderer>().material.color = Color.red;

        //�h�炷
        transform.DOShakePosition(1f, 1f, 5, 1, false, true);
        isShake = true;

        //���Ƃ�
        StartCoroutine(Drop(1.0f));
    }

    //�X�V
    private void ColorUpdate()
    {
        if (((int)time) >= 4 || isChangeRedColor) return;

        //������ԐF
        for (int i = 0; i < timeTextMeshPro.Length; i++)
            timeTextMeshPro[i].color = Color.red;

        //���b�V�������_���[���擾(�_��)
        MeshRenderer r = GetComponent<MeshRenderer>();
        tweener = r.material.DOColor(Color.red, flashingTime).SetLoops(-1, LoopType.Yoyo);

        isChangeRedColor = true;
    }

    //���Ƃ�
    IEnumerator Drop(float delay)
    {
        yield return new WaitForSeconds(delay);

        transform.DOMove(new Vector3(transform.position.x, transform.position.y - 2.5f, transform.position.z), 1f).SetEase(Ease.InOutQuart);

        //���ɖ߂�
        StartCoroutine(Undo(4.0f));
    }

    //���ɖ߂�
    IEnumerator Undo(float delay)
    {
        yield return new WaitForSeconds(delay);

        //���F�ɂ��Ă���
        GetComponent<MeshRenderer>().material.color = Color.white;
        transform.DOMove(new Vector3(transform.position.x, transform.position.y + 2.5f, transform.position.z), 1f).SetEase(Ease.InOutQuart);

        //������
        StartCoroutine(Initializ(1.0f));
    }


    //������
    IEnumerator Initializ(float delay)
    {
        yield return new WaitForSeconds(delay);

        time = 10.0f;
        isShake = false;
        isChangeRedColor = false;

        for (int i = 0; i < timeTextMeshPro.Length; i++)
            timeTextMeshPro[i].color = Color.white;
    }

    // �R���W���������������Ƃ��ɌĂяo����郁�\�b�h
    private void OnCollisionEnter(Collision collision)
    {
        //�v���C���[�ł͂Ȃ��̂Ȃ炱�̐揈�����Ȃ�
        if (!collision.gameObject.CompareTag("Player")) return;

        //���łɓ����v���C���[�����Ȃ����`�F�b�N
        for (int i = 0; i < hitPlayer.Count; i++)
            if (hitPlayer[i] == collision.gameObject) return;

        //�ǉ�
        hitPlayer.Add(collision.gameObject);
    }

    //�R���W���������ꂽ���ɌĂ΂��֐�
    private void OnCollisionExit(Collision collision)
    {
        //�v���C���[�ł͂Ȃ��̂Ȃ炱�̐揈�����Ȃ�
        if (!collision.gameObject.CompareTag("Player")) return;

        //�����v���C���[�����Ȃ����`�F�b�N
        for (int i = 0; i < hitPlayer.Count; i++)
        {
            if (hitPlayer[i] == collision.gameObject)
            {
                //�폜
                hitPlayer.Remove(collision.gameObject);
                return;
            }
        }
       
    }
}
