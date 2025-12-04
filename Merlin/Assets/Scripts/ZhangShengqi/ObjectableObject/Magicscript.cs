using UnityEngine;

// 添加CreateAssetMenu特性，以便在编辑器中方便创建该ScriptableObject实例
[CreateAssetMenu(fileName = "ScriptableObject", menuName = "ScriptableObjects/ScriptableObject", order = 0)]
public class Magicscript : ScriptableObject
{
    // 在这里定义你需要存储的数据字段，比如：
    public int[] ElementInfo = new int[4];
    public string NameInfo;
    public string FuncInfo;
    public Sprite sprite;
    public bool Isactivated;

    public bool IsBullet = false;
    public bool IsComponent = false;
    public string TypeName = "";
}