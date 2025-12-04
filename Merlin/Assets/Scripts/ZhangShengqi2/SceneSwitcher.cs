using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneSwitcher : MonoBehaviour
{
    // 用于存储要跳转的目标场景的名称
    public string targetSceneName = "TransferScene"; 
    // 关联在Unity编辑器中要操作的按钮组件
    public Button switchSceneButton; 

    void Start()
    {
        // 检查按钮是否在Unity编辑器中正确赋值
        if (switchSceneButton != null)
        {
            // 给按钮的点击事件添加一个监听，当按钮被点击时调用SwitchScene方法
            switchSceneButton.onClick.AddListener(SwitchScene);
        }
        else
        {
            // 如果按钮未赋值，在控制台输出错误信息
            Debug.LogError("SwitchSceneButton is not assigned!");
        }
    }

    void SwitchScene()
    {
        // 检查目标场景名称是否为空
        if (!string.IsNullOrEmpty(targetSceneName))
        {
            // 如果不为空，使用SceneManager加载目标场景
            SceneManager.LoadScene(targetSceneName);
        }
        else
        {
            // 如果目标场景名称为空，在控制台输出错误信息
            Debug.LogError("TargetSceneName is not set!");
        }
    }
}