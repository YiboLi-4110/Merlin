using UnityEngine;
using UnityEngine.UI;

public class ButtonPanelController : MonoBehaviour
{
    // 定义三个按钮
    public Button[] buttons;

    // 定义三个面板
    public GameObject[] panels;

    private void Start()
    {
        // 为每个按钮添加点击事件监听器
        //button1.onClick.AddListener(() => ShowPanel(panel1));
        for (int i=0;i<buttons.Length;i++)
        {
            int index = i;
            buttons[index].onClick.AddListener(() =>ShowPanel(index));
        }
        // 初始时隐藏所有面板
        HideAllPanels();
    }

    private void HideAllPanels()
    {
        for (int i=0;i<panels.Length;i++)
        {
            panels[i].SetActive(false);
        }
    }

    private void ShowPanel(int index)
    {
        // 隐藏所有面板
        HideAllPanels();
        // 显示指定的面板
        panels[index].SetActive(true);
    }
}