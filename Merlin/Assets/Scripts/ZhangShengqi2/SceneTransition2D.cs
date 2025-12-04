using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition2D : MonoBehaviour
{
    public float transitionTime = 5f; // 转场时间，单位：秒
    private string targetSceneName = "WarScene"; // 目标场景的名称

    void Start()
    {
        // 调用 Invoke 方法，在指定时间后调用 LoadTargetScene 方法
        Invoke("LoadTargetScene", transitionTime);
    }

    void LoadTargetScene()
    {
        // 加载目标场景
        SceneManager.LoadScene(targetSceneName);
    }
}