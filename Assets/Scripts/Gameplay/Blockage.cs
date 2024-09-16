using UnityEngine;

public class Blockage : MonoBehaviour
{
    public int BlockageID;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // This function removes the blockage child game object;
    public void ClearBlockage()
    {
        if (gameObject.name.Contains("Blockage"))
            Destroy(gameObject);
        else
        {
            GameObject Blockage = GameObject.Find(gameObject.name + "_Blockage");
            Destroy(Blockage);
        }
    }
}
