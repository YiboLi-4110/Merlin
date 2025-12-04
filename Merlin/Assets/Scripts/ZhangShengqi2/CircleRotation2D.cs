using UnityEngine;

public class CircleRotation2D : MonoBehaviour
{
    private float rotationSpeed = 250f;

    void Update()
    {
        // 绕 Z 轴旋转，因为在 2D 中，物体通常在 XY 平面，旋转轴为 Z 轴
        transform.Rotate(0, 0, -rotationSpeed * Time.deltaTime);
    }
}