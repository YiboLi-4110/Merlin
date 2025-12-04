using UnityEngine;

[CreateAssetMenu(fileName = "NewStatsConfig", menuName = "Character/Stats Config")]
public class CharacterStatusConfig : ScriptableObject 
{
    public float Blood;  // 原始血量
    public float Speed;  // 原始移动速度
    public float FrontDefense;  // 原始前护甲
    public float BehindDefense;  // 原始后护甲
    public float[] MagicDefense = new float[5];  // 初始法抗
    
}
