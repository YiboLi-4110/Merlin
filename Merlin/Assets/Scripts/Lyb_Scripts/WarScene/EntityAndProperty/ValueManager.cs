using UnityEngine;

using TMPro;

public class ValueManager : MonoBehaviour
{
    public TMP_Text TextValue;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateValue(float CurrentValue)
    {
        TextValue.text = CurrentValue.ToString("0.0");
    }
}
