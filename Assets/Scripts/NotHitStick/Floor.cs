using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Floor : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI[] timeTextMeshPro; //�������ԃe�L�X�g

    //���ݐ�������
    private float time = 30.0f;

    //�X�s�[�h�{��
    private float speedRatio = 1.0f;

    //�������Ă���v���C���[��ۊ�
    private List<GameObject> hitPlayer = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < timeTextMeshPro.Length; i++)
            timeTextMeshPro[i].text = "30";
    }

    // Update is called once per frame
    void Update()
    {
        //���Ԍv�Z�E�\��
        time -= Time.deltaTime * (speedRatio * hitPlayer.Count);
        for (int i = 0; i < timeTextMeshPro.Length; i++)
            timeTextMeshPro[i].text = ((int)time).ToString();

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
