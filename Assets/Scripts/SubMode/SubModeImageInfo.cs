using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SubModeImageInfo : MonoBehaviour
{

    [SerializeField] private SubModeImageInfo leftImage;    //¶‘¤‚Ì‰æ‘œ
    [SerializeField] private SubModeImageInfo rightImage;   //‰E‘¤‚Ì‰æ‘œ
    [SerializeField] private SubModeImageInfo upImage;      //ã‘¤‚Ì‰æ‘œ
    [SerializeField] private SubModeImageInfo downImage;    //‰º‘¤‚Ì‰æ‘œ
    [SerializeField] private List<Image> edgeImage;         //˜g‚Ì‰æ‘œ
    [SerializeField] private List<Image> playerNumberImage; //˜g‚Ì‰æ‘œ

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
