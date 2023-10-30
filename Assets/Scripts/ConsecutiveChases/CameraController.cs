using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float CAMERA_DISTANCE = 3.0f;       //��ԑ����v���C���[����̃J�����̋���
    public List<GameObject> playerList;


    // Start is called before the first frame update
    void Start()
    {
        //��Ԑi��ł���v���C���[
        float priorityDistance = 9999.0f;

        //�v���C���[�̈�ԑ�����̈ʒu
        for (int i = 0; i < playerList.Count; i++)
        {
            if (playerList[i].transform.position.z <= priorityDistance)
            {
                priorityDistance = playerList[i].transform.position.z;
            }
        }

        //�J�����Ƃ̋�����ۂ�
        transform.position = new Vector3(transform.position.x, transform.position.y, priorityDistance + CAMERA_DISTANCE);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
