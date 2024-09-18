using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BossBattleTrigger : MonoBehaviour
{
    private GameManager GameManager;
    public int TrigegrIndex;

    // Start is called before the first frame update
    void Start()
    {
        GameManager = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        // If the enemy has been defeated, the corresponding Boss Battle can no longer be triggered;
        if (GameManager.IsDefeated(TrigegrIndex)) Destroy(gameObject);
    }
}
