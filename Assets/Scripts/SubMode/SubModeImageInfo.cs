using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SubModeImageInfo : MonoBehaviour
{

    [SerializeField] private SubModeImageInfo leftImage;    //�����̉摜
    [SerializeField] private SubModeImageInfo rightImage;   //�E���̉摜
    [SerializeField] private SubModeImageInfo upImage;      //�㑤�̉摜
    [SerializeField] private SubModeImageInfo downImage;    //�����̉摜
    [SerializeField] private List<Image> edgeImage;         //�g�̉摜
    [SerializeField] private List<Image> playerNumberImage; //�g�̉摜

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
