using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class PositionShow : MonoBehaviour
{
    public static PositionShow PS = null;
    public TextMeshProUGUI position_text = null;
    public UnityEngine.UI.Image position_panel = null;

    void Awake()
    {
        PS = this;
    }

    void Start()
    {
        if(position_text != null)
        {
            Tuple<int, int> pos = TheGlobalManager.TGM.GetCurrentPosition();
            position_text.text = "（" + pos.Item1+'，' + pos.Item2 + "）";
        }
        if(!TheGlobalManager.TGM.IsSafe())
        {
            position_panel.color = new Color(0.96f, 0.56f, 0.57f, 0.3f);  
        }
        else
        {
            position_panel.color = new Color(0.52f, 0.96f, 0.51f, 0.3f);
        }
    }

}
