using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestroyParticle : MonoBehaviour
{
    ParticleSystem particle;
    // Start is called before the first frame update
    void Start()
    {
        particle = this.GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (particle.isStopped) //�p�[�e�B�N�����I������������
        {
            Destroy(this.gameObject);//�p�[�e�B�N���p�Q�[���I�u�W�F�N�g���폜
        }
    }
}
