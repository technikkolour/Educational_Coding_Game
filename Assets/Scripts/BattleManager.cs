using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    private GameManager GameManager;

    public List<GameObject> Bosses;

    // Start is called before the first frame update;
    void Start()
    {        
        GameManager = FindObjectOfType<GameManager>();

    }

    // Update is called once per frame;
    void Update()
    {
        bool BossActive = false;

        for (int i = 0; i < 3; i++)
        {
            if (GameManager.IsDefeated(i) || BossActive) Bosses[i].SetActive(false);
            else
            {
                BossActive = true;
                Bosses[i].SetActive(true);
            }
        }
    }
}
