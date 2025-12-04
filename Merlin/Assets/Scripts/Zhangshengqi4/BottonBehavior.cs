using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadSceneWithAddListener : MonoBehaviour
{
    // 引用按钮组件
    public Button loadSceneButton;
    // 要加载的场景名称
    private string sceneName = "WarScene";

    void Start()
    {
        // 检查按钮引用是否为空
        if (loadSceneButton != null)
        {
            // 为按钮的点击事件添加监听
            loadSceneButton.onClick.AddListener(LoadScene);
        }
        else
        {
            Debug.LogError("按钮引用未设置！");
        }
    }

    // 按钮点击时调用的方法
    void LoadScene()
    {
        // 检查场景名称是否为空
        if (!string.IsNullOrEmpty(sceneName))
        {
            // 加载指定名称的场景
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.LogError("场景名称不能为空！");
        }
    }
}