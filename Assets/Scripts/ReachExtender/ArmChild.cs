using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ArmChild : MonoBehaviour
{
    [SerializeField] private Arm arm;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void PlayerMove()
    {
        //���C���������������
        RaycastHit rayHit;

        //���C���΂�����
        Ray ray = new Ray(transform.position, transform.parent.gameObject.transform.position);

        if (UnityEngine.Physics.Raycast(ray, out rayHit, 9999))
        {
            Arm am = transform.parent.GetComponent<Arm>();
            GameObject go = am.GetHitPlayer();
            go.GetComponent<ReachExtenderThreePlayer>().SetMove(rayHit.normal * 100);
        }
    }
}
