using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RandomDataBehavior : MonoBehaviour
{
    public StartData[] allData; // 存储所有可脚本化对象实例的数组
    public TextMeshProUGUI[] text = new TextMeshProUGUI[12];
    private List<StartData> selectedData = new List<StartData>(); // 存储随机选取的3个实例
    public static int[] savedData = {2,2,2,2}; // 公有的静态整数数组

    public Button[] buttons; // 三个按钮
    public Button changeDataButton; // 新添加的按钮
    private bool isChangeDataButtonClicked = false; // 标记按钮是否已经被点击过

    private void Start()
    {
        // 随机选取3个实例
        SelectRandomData();

        // 为按钮添加点击事件
        for (int i = 0; i < buttons.Length; i++)
        {
            int index = i; // 避免闭包问题
            buttons[i].onClick.AddListener(() => SaveDataToStaticArray(index));
        }

        // 为更换数据按钮添加点击事件
        changeDataButton.onClick.AddListener(ChangeDataIfNotClicked);
    }

    private void SelectRandomData()
    {
        // 确保有足够的实例可供选择
        if (allData.Length < 3)
        {
            Debug.LogError("没有足够的可脚本化对象实例可供选择。");
            return;
        }

        // 确保text数组长度足够
        if (text.Length < 12)
        {
            Debug.LogError("text数组长度不足，无法显示所有数据。");
            return;
        }

        selectedData.Clear(); // 清空已选列表
        List<StartData> availableData = new List<StartData>(allData); // 复制所有实例到一个列表中

        for (int i = 0; i < 3; i++)
        {
            int randomIndex = Random.Range(0, availableData.Count); // 随机选择一个索引
            StartData selected = availableData[randomIndex]; // 获取随机选择的实例
            selectedData.Add(selected); // 将选择的实例添加到已选列表中
            availableData.RemoveAt(randomIndex); // 从可用列表中移除已选实例，避免重复选择
        }

        // 显示选取的实例的数据
        for (int i = 0; i < 12; i += 4)
        {
            int index = i / 4;
            text[i].text = selectedData[index].Ignisnum.ToString();
            text[i + 1].text = selectedData[index].Aquanum.ToString();
            text[i + 2].text = selectedData[index].Terranum.ToString();
            text[i + 3].text = selectedData[index].Aernum.ToString();
        }
    }

    private void SaveDataToStaticArray(int buttonIndex)
    {
        if (buttonIndex < 0 || buttonIndex >= selectedData.Count)
        {
            Debug.LogError("按钮索引超出范围。");
            return;
        }

        StartData data = selectedData[buttonIndex];
        savedData[0] += data.Ignisnum;
        savedData[1] += data.Aquanum;
        savedData[2] += data.Terranum;
        savedData[3] += data.Aernum;

        Debug.Log("数据已保存到静态数组：" + string.Join(", ", savedData));
        SceneManager.LoadScene("TransferScene");
        Debug.Log("转换到加载界面");
    }

    private void ChangeDataIfNotClicked()
    {

        if (!isChangeDataButtonClicked)
        {
            SelectRandomData(); // 更换数据
            isChangeDataButtonClicked = true; // 标记按钮已被点击

            changeDataButton.interactable = false; // 禁用按钮
        }
    }

    /*void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            //SceneManager.LoadScene("InputScene");
        }
    }*/
}