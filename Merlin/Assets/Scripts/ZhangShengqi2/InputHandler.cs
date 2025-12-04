using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class IntegerInputHandler : MonoBehaviour
{
    // 引用输入字段
    public TMP_InputField inputField;
    // 引用按钮
    public Button submitButton;
    // 用于保存用户输入的整数
    public static int userInputInteger;
    //private string targetSceneName = "WarScene";

    void Start()
    {
        // 为按钮的点击事件添加监听
        submitButton.onClick.AddListener(OnSubmitButtonClicked);
    }

    void OnSubmitButtonClicked()
    {
        // 获取输入字段的文本
        string input = inputField.text;
        int globalseed = UnityEngine.Random.Range(0, 1999999999);       // 尝试将输入的字符串转换为整数
        if (int.TryParse(input, out userInputInteger))
        {
            TheGlobalManager.TGM.SetNewWorld(userInputInteger);
            Debug.Log("用户输入的整数是: " + userInputInteger);
            SceneManager.LoadScene("LeadScene");
            // 转换成功，打印用户输入的整数
            
        }
        else
        {
            TheGlobalManager.TGM.SetNewWorld(globalseed);
            SceneManager.LoadScene("LeadScene");
            // 转换失败，提示用户输入无效
            //Debug.Log("输入无效，请输入一个有效的整数。");
        }
    }

}