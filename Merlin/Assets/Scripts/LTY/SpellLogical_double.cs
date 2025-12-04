using UnityEngine;

public class SpellLogical_double : SpellLogical, LogicalOperators
{
    
    public override void Emit(BulletBase bb)
    {
        
        Debug.Log("Logical accessful");
        if(bb is EntityBullet eb)
        {
            //eb.intervaltime = eb.bullet_main_entity.intervaltime;
            if(Time.time - TestBulletManager.last_bullet_time < eb.intervaltime)
                return ;
            if(eb.copy == false)
            {
                Debug.Log("Double Bullet!!!!!");
                TestBulletManager.TBM.CreateANewBullet2(eb.original_up+(Vector3)UnityEngine.Random.insideUnitCircle, PlayerController.EntityMerlin.gameObject.transform.position); 
            }  
            else
            {
                Debug.Log("Double false????????");
            } 
        }
    }

    public override void Process(BulletBase bb)
    {
        if(bb is EntityBullet eb)
        {
            eb.Speed -= eb.Speed * 0.005f;
        }
    }
}
