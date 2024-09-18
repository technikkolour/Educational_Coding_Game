using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    private GameManager GameManager;
    private BattleUIUpdater UIUpdater;
    private bool BossActive;

    public List<GameObject> Bosses;        

    // Start is called before the first frame update;
    void Start()
    {        
        GameManager = FindObjectOfType<GameManager>();
        UIUpdater = FindObjectOfType<BattleUIUpdater>();

        BossActive = false;
    }

    // Update is called once per frame;
    void Update()
    {
        for (int i = 0; i < 3; i++)
        {
            if (!BossActive)
            {
                if (GameManager.IsDefeated(i) && Bosses[i] != null) Destroy(Bosses[i]);
                else
                {
                    BossActive = true;
                    Bosses[i].SetActive(true);

                    UIUpdater.Target = Bosses[i];
                    UIUpdater.SetInitialValue(Bosses[i].GetComponent<Enemy>().Health);
                }
            }

        }
    }
}
