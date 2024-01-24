using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class MainModePlayerImage : MonoBehaviour
{
    [SerializeField] private List<Image> playerImage;

    // Start is called before the first frame update
    void Start()
    {
        PlayerManager.Initializ();
        for(int i = 0; i < playerImage.Count; i++)
        {
            playerImage[i].sprite = Resources.Load<Sprite>(PlayerManager.GetPlayerVisualImage((byte)(i + 1)));
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
