using UnityEngine;

public class PuzzleSpawner : MonoBehaviour
{
    public string PuzzleType;
    public int PuzzleID;
    public int ItemID;
    public int Attempts;

    private PuzzleManager PuzzleManager;
    private GameManager GameManager;
    private bool IsActive = true;

    // Start is called before the first frame update
    void Start()
    {
        PuzzleManager = FindObjectOfType<PuzzleManager>();
        GameManager = FindObjectOfType<GameManager>();

        Attempts = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.IsPuzzleCompleted(PuzzleID)) IsActive = false;
    }

    public void Spawn()
    {
        if (IsActive) PuzzleManager.SpawnPuzzle(PuzzleType, PuzzleID);
    }
}
