using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VortexManager : MonoBehaviour
{
    [SerializeField] private float instanceTime = 5f;
    [SerializeField] private List<Vortex> vortexList;
    [SerializeField] private GameObject stage;
    [SerializeField] private float radius = 6f;
    [SerializeField] private float VertexRadius = 4f;
    private bool isAppearanceVotex = true;

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator VortexInstance()
    {
        yield return new WaitForSeconds(instanceTime);

        Vector3 prePos = Vector3.positiveInfinity;

        //�Q�̈ʒu�����߂�
        for (int i = 0; i < vortexList.Count; i++)
        {
            //�Q���A�N�e�B�u��
            vortexList[i].gameObject.SetActive(true);

            //�Q�̈ʒu��ύX
            Vector3 tmp = VertexPositionChange();
            vortexList[i].transform.position = new Vector3(tmp.x, vortexList[i].transform.position.y, tmp.z);

            //�Q���m���Ԃ���Ȃ��悤��
            float distance = 9999;

            //���ڂȂ�
            if (prePos != Vector3.positiveInfinity)
            {
                //�Q���m�̋������v�Z
                distance = Vector3.Distance(prePos, vortexList[i].transform.position);
            }

            if (distance < VertexRadius && prePos != Vector3.positiveInfinity)
            {
                i--;
                continue;
            }
            prePos = vortexList[i].transform.position;
        }
        if (isAppearanceVotex) 
            //�������R���[�`�����Ă�
            StartCoroutine(VortexInstance());
    }

    //�Q�̈ʒu
    public Vector3 VertexPositionChange()
    {
        //�����_���Ƀx�N�g�����쐬
        return stage.transform.position + (RandDirection() * Random.Range(0f, radius));
    }

    public Vector3 RandDirection()
    {
        return new Vector3(Random.Range(-100, 100), 0, Random.Range(-100, 100)).normalized;
    }

    //�Q�̃R���[�`���J�n
    public void StartVortexCoroutine()
    {
        //�R���[�`�����s
        StartCoroutine(VortexInstance());
    }

    public void SetIsAppearanceVotex(bool a)
    {
        isAppearanceVotex = a;
    }
}
