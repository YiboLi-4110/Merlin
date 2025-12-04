using UnityEngine;



public class Entity : MonoBehaviour
{
    // 角色属性
    public EntityProperty Status;
    // 角色具体属性
    public CharacterStatusConfig ConcreteInformation;
    // 角色附带的图片
    // public Image EntityBackground;


    protected void Readconfig()
    {
        // transform.gameObject.AddComponent<Image>();

        // 将具体的属性加载到实体当中
        if (ConcreteInformation == null) Debug.Log("something is none"); 
        Status.Initialization(ConcreteInformation);

    }

    // Start is called before the first frame update

    // Update is called once per frame

    /*
    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "FireAttack":
                {
                    if (Status.CurrentSituation.Item1 == EntityProperty.situation.Burn)
                    {
                        Status.CurrentBlood -= 20f * (1-Status.CurrentMagicDfense[1]);
                        Status.CurrentSituation.Item2 = 5.0f;
                    }
                    else
                        Status.CurrentBlood -= 10f * (1-Status.CurrentMagicDfense[1]);

                    if (Status.CurrentSituation.Item1==EntityProperty.situation.Nothing 
                    ||  Status.CurrentSituation.Item2<=0
                    ||  Status.CurrentSituation.Item1==EntityProperty.situation.Burn)
                        Status.CurrentSituation.Item1 = EntityProperty.situation.Burn;
                }
                break;
            case "WaterAttack":
                {
                    Status.CurrentBlood -= 10f * (1-Status.CurrentMagicDfense[2]);
                    Status.CurrentSpeed *= 0.5f;
                    if (Status.CurrentSituation.Item1==EntityProperty.situation.Nothing 
                    || Status.CurrentSituation.Item1==EntityProperty.situation.Frozen
                    || Status.CurrentSituation.Item2<=0)
                        Status.CurrentSituation.Item1 = EntityProperty.situation.Frozen;
                }
                break;
            default:
                break;
        }
    }
    */

}
