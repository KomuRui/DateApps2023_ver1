using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class CarryToTheGoalGameManager : MiniGameManager
{

    //�v���C���[���C�t
    [SerializeField] private List<TextMeshProUGUI> lifeText;
    public Dictionary<GameObject, TextMeshProUGUI> playerLifeText = new Dictionary<GameObject, TextMeshProUGUI>();
    public Dictionary<GameObject, int> playerLife = new Dictionary<GameObject, int>();
    public GameObject[] player;

    // Start is called before the first frame update
    public override void SceneStart()
    {
        for (int i = 0; i < player.Length; i++)
        {
            playerLifeText[player[i]] = lifeText[i];
            playerLife[player[i]] = 2;
        }
    }

    public override void MiniGameUpdate()
    {
        for (int i = 0; i < player.Length; i++)
            playerLifeText[player[i]].text = playerLife[player[i]].ToString();
    }


    public void Damege(GameObject player)
    {
        //���łɎ���ł���̂Ȃ炱�̐揈�����Ȃ�
        if (playerLife[player] <= 0) return;

        //���C�t�����炷
        playerLife[player]--;
        playerLife[player] = Mathf.Max(playerLife[player], 0);
        if (playerLife[player] <= 0) player.GetComponent<CarryToTheGoalPlayer>().Dead();
    }
}
