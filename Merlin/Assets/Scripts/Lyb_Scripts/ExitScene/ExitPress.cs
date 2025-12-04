using UnityEngine;
using UnityEngine.SceneManagement; // 引入场景管理命名空间

public class ExitPress : MonoBehaviour
{
    void Update()
    {
        // 检测是否有任意按键被按下
        if (Input.anyKeyDown)
        {
            // 加载名为 "StartScene" 的场景
            TheGlobalManager.TGM.TrytoLoad();
            SceneManager.LoadScene("WarScene");
            Debug.Log("Loading StartScene...");
        }
    }
}
