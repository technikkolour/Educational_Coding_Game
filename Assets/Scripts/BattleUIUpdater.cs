using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleUIUpdater : MonoBehaviour
{
    public GameObject Target;
    private float Dimension;

    private float CurrentValue;
    private float InitialValue;
    private float MaxDimension;

    // Start is called before the first frame update
    void Start()
    {
        InitialValue = GetValue();
        MaxDimension = gameObject.transform.localScale.x;
    }

    // Update is called once per frame
    void Update()
    {
        CurrentValue = GetValue();
        Dimension = (MaxDimension * CurrentValue) / InitialValue;

        gameObject.transform.localScale = new Vector3(Dimension, gameObject.transform.localScale.y, gameObject.transform.localScale.z);
    }

    // Get the corresponding value in the current state of the game;
    private float GetValue()
    {
        float Value = 0;

        // Depending on the Target GameObject, select the necessary value;
        if (Target.GetComponent<Robot>() != null && gameObject.name.Contains("Health")) Value = Target.GetComponent<Robot>().Health;
        else if (Target.GetComponent<Robot>() != null && gameObject.name.Contains("Strength")) Value = Target.GetComponent<Robot>().Strength;
        else if (Target.GetComponent<Enemy>() != null) Value = Target.GetComponent<Enemy>().Health;

        return Value;
    }
}
