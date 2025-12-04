using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class MovingBloodManager : MonoBehaviour
{
    public CinemachineVirtualCamera CVC;
    public GameObject target; // 要跟随的目标对象
    public Vector3 offset = new Vector3(0, 1.6f, 0); // 血条相对于目标的偏移量
    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main; // 获取主相机
        CVC = FindObjectOfType<CinemachineVirtualCamera>();  // 获取唯一的virtual camera
        transform.position = target.transform.position + offset;
    }

    void Update()
    {
        transform.position = target.transform.position + offset;
        // 将目标的世界坐标转换为屏幕坐标，并应用偏移量
        Vector3 screenPosition = mainCamera.WorldToScreenPoint(target.transform.position + offset);
        // 更新血条的位置
        transform.position = screenPosition;
        
        // 让血条始终朝向相机
        transform.LookAt(transform.position + mainCamera.transform.forward);

        // 让血条随着相机比例变化而变化
        float scale = CVC.m_Lens.OrthographicSize;
        scale = (float)(88.0f-6.0f*scale)/49.0f;
        transform.localScale = new Vector3 (scale,scale,1);
    }

    // 设置血量的方法，可以被其他脚本调用
    /*
    public void SetHealth(float currentHealth, float maxHealth)
    {
        Slider slider = GetComponent<Slider>();
        if (slider != null)
        {
            slider.value = currentHealth / maxHealth; // 更新血条的填充量
        }
    }
    */
}