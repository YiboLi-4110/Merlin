using UnityEngine;
using TMPro;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "EnemyScript", menuName = "ScriptableObjects/EnemyScript", order = 1)]
public class EnemyScript : ScriptableObject
{
    
    public string EnemyName;

   
    public string EnemyExplanaion;

    // Image 变量
    public Sprite EnemyImagesprite;
}