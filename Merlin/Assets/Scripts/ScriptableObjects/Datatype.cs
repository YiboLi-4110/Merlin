using UnityEngine;

// 定义可脚本化对象类
[CreateAssetMenu(fileName = "NewData", menuName = "Datatype")]
public class StartData : ScriptableObject
{
    public int Ignisnum; // 要显示的内容
    public int Aquanum;
    public int Terranum;
    public int Aernum;
}