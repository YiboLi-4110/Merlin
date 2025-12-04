using UnityEngine;


public class EntityProperty : MonoBehaviour
{
    // 属性初始值
    public float Blood;  // 原始血量
    private float Speed;  // 原始移动速度
    private float FrontDefense;  // 原始前护甲
    private float BehindDefense;  // 原始后护甲
    private float[] MagicDefense = new float[5];  // 原始法抗

    // 公有变量
    // 背景图片
    // public Image EntityBackground;
    public float CurrentBlood;
    //public float CurrentAttack;
    public float CurrentSpeed;
    public float CurrentFrontDefense;
    public float CurrentBehindDefense;
    public float[] CurrentMagicDfense = new float[5];  // 法抗:火、水、风、土
    public enum situation
    {
        Nothing,  // 无
        Toxic,  // 中毒
        Burn,  // 灼烧
        Wet,  // 潮湿
        Frozen,  // 冰冻
        Easyhurt,  // 易伤
        Magicshield,  // 法抗护盾
        Physicshield,  // 物理护盾
        Fast,  // 迅捷
        Death  // 死亡
    }
    public (situation , float) CurrentSituation;
    public float SituationContinuousTime = 2.0f;
    // public float HelpTime;
    public BarManager ShowBlood;
    //public BarManager ShowMovingBlood;
    //public BarManager ShowAttack;
    public BarManager ShowSpeed;
    public BarManager ShowFrontDefense;
    public BarManager ShowBehindDefense;
    public ValueManager[] ShowMagicDefense = new ValueManager[5];
    public SituationManager ShowSituation;


    // 初始化以及修改属性的函数
    public void Initialization(CharacterStatusConfig ConcreteInformation) 
    {
        CurrentBlood = Blood = ConcreteInformation.Blood;
        Debug.Log("initial blood is " + CurrentBlood);
        CurrentSpeed = Speed = ConcreteInformation.Speed;
        CurrentFrontDefense = FrontDefense = ConcreteInformation.FrontDefense;
        CurrentBehindDefense = BehindDefense = ConcreteInformation.BehindDefense;
        for (int i=0; i<=4; i++) CurrentMagicDfense[i] = MagicDefense[i] = ConcreteInformation.MagicDefense[i];
        CurrentSituation = (situation.Nothing , SituationContinuousTime);

    }

    public void UpdateBlood() 
    {
        ShowBlood.UpdateBar(CurrentBlood);
        //ShowMovingBlood.UpdateBar(CurrentBlood);
        
        if (CurrentBlood <= 0)
            CurrentSituation.Item1 = situation.Death;
    }
    public void UpdateSpeed() 
    {
        ShowSpeed.UpdateBar(CurrentSpeed);
    }
    public void UpdateFrontDefense() 
    {
        ShowFrontDefense.UpdateBar(CurrentFrontDefense);
    }
    public void UpdateBehindDefense() 
    {
        ShowBehindDefense.UpdateBar(CurrentBehindDefense);
    }
    public void UpdateMagicDefense() 
    {
        for (int i=1; i<=4; i++)
            ShowMagicDefense[i].UpdateValue(CurrentMagicDfense[i]);
    }
    public void UpdateSituation() 
    {
        //Sprite BackGroundSprite = EntityBackground.GetComponent<Sprite>();

        int SituationIndex = (int)CurrentSituation.Item1;
        CurrentSituation.Item2 -= Time.smoothDeltaTime;
        // HelpTime = CurrentSituation.Item2;
        ShowSituation.UpdateSituation(SituationIndex , CurrentSituation.Item2);
        if (CurrentSituation.Item2 <= 0)
        {
            CurrentSpeed = Speed;
            CurrentSituation.Item2 = SituationContinuousTime;
            CurrentSituation.Item1 = situation.Nothing;
            //BackGroundSprite = null;
        }

        // 更新背景图片
        /*
        if (CurrentSituation.Item1!=EntityProperty.situation.Nothing && EntityBackground.sprite==null)
        {
            switch(CurrentSituation.Item1)
            {
                case EntityProperty.situation.Frozen:
                    {
                        EntityBackground.sprite = Resources.Load<Sprite>("Textures/FrozenIce");
                    }
                    break;
                default:
                    break;
            }
        }
        */
    }
    public void UpdataAll()
    {
        UpdateBlood();
        UpdateSpeed();
        UpdateFrontDefense();
        UpdateBehindDefense();
        UpdateMagicDefense();
        UpdateSituation();
    }
}
