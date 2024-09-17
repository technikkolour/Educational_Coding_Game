using UnityEngine;

public class Blockage : MonoBehaviour
{
    public int BlockageID;
    private GameManager GameManager;

    // Start is called before the first frame update
    void Start()
    {
        GameManager = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (BlockageID)
        {
            case int i when i >= 0 && i <= 1:
                // Remove the first two blockages if the correct journal entries are present;
                if ((GameManager.IsEntryFound(3) && BlockageID == 0) || (GameManager.IsEntryFound(4) && BlockageID == 1)) 
                    ClearBlockage();
                break;
            case int i when i >= 2 && i <= 8:
                PuzzleSpawner PuzzleSpawner = gameObject.GetComponent<PuzzleSpawner>();
                if (!PuzzleSpawner.IsActive())
                    ClearBlockage();
                break;
            case 9:
                if (GameManager.HasItem(0))
                    ClearBlockage();
                break;
        }

        if (GameManager.GetClearedBlockages().Count < 10)
            if (GameManager.IsCleared(BlockageID)) ClearBlockage();

    }

    // This function removes the blockage child game object;
    public void ClearBlockage()
    {
        if (gameObject.name.Contains("Blockage") || gameObject.name.Contains("Gate"))
        {
            GameManager.ClearBlockage(BlockageID);
            Destroy(gameObject);
        }
        else
        {
            GameObject Blockage = GameObject.Find(gameObject.name + "_Blockage");
            GameManager.ClearBlockage(BlockageID);
            Destroy(Blockage);
        }
    }
}
