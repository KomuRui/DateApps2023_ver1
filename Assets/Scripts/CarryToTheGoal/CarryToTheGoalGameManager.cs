using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarryToTheGoalGameManager : MonoBehaviour
{
    //�v���C���[���C�t
    public Dictionary<GameObject, int> playerLife = new Dictionary<GameObject, int>();
    public GameObject[] player;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < player.Length; i++)
            playerLife[player[i]] = 3;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Damege(GameObject player)
    {
        //���łɎ���ł���̂Ȃ炱�̐揈�����Ȃ�
        if (playerLife[player] <= 0) return;

        Debug.Log("dame-ji");
        Debug.Log(playerLife[player]);
        //���C�t�����炷
        playerLife[player]--;
        playerLife[player] = Mathf.Max(playerLife[player], 0);
        if (playerLife[player] <= 0) player.GetComponent<CarryToTheGoalPlayer>().Dead();
    }
}
