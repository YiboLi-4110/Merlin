using UnityEngine;
using UnityEngine.UI;

public class BarManager : MonoBehaviour
{
    public Image fillImage; // 填充条的Image组件
    public float MaxValue = 25f; // 最大值
    public float smoothTime = 0.3f; // 平滑过渡时间

    private float targetFillAmount; // 目标填充比例
    private float currentFillAmount; // 当前填充比例
    private float velocity = 0f; // 平滑过渡的速度变量

    void Start()
    {
        currentFillAmount = fillImage.fillAmount; // 初始化当前填充比例
    }

    void Update()
    {
        // 使用Mathf.SmoothDamp进行平滑过渡
        currentFillAmount = Mathf.SmoothDamp(currentFillAmount, targetFillAmount, ref velocity, smoothTime);

        // 更新填充条的填充量
        fillImage.fillAmount = currentFillAmount;
    }

    public void UpdateBar(float CurrentValue)
    {
        // 计算目标填充比例
        targetFillAmount = CurrentValue / MaxValue;
    }
}
