using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CarryBullet : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2.0f; �@�@�@�@�@�@�@�@//�ړ����x
    [SerializeField] private GameObject cannon;      �@�@�@�@�@�@�@�@//��C�I�u�W�F�N�g
    [SerializeField] private CarryToTheGoalGameManager manager;      //�}�l�W���[

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //�I�u�W�F�N�g���A�N�e�B�u�Ȃ�ړ�
        if (this.gameObject.activeSelf)
            transform.position += Vector3.down * moveSpeed * Time.deltaTime;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "CarryStage")
        {
            this.gameObject.SetActive(false);
            transform.position = cannon.gameObject.transform.position;
        }
        if(other.gameObject.tag == "Player")
        {
            this.gameObject.SetActive(false);
            transform.position = cannon.gameObject.transform.position;
            manager.Damege(other.gameObject);
        }
    }
}
