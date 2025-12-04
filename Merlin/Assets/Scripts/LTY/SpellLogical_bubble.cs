using UnityEngine;

public class SpellLogical_bubble : SpellLogical, LogicalOperators
{
   public override void Emit(BulletBase bb)
    {
        Debug.Log("Logical accessful");
        if(bb is EntityBullet eb)
        {
            //eb.skpara += 4.0f;
            eb.alive_distance_time = 1.2f;
            eb.intervaltime = 3.0f;
            eb.Speed = 0.05f;
            if(Time.time - TestBulletManager.last_bullet_time < eb.intervaltime)
                return ;
            if(eb.copy == false)
            {
                Debug.Log("Double Bullet!!!!!");
                TestBulletManager.TBM.CreateANewBullet2(eb.original_up+(Vector3)UnityEngine.Random.insideUnitCircle*2.0f, PlayerController.EntityMerlin.gameObject.transform.position);
                TestBulletManager.TBM.CreateANewBullet2(eb.original_up+(Vector3)UnityEngine.Random.insideUnitCircle*2.0f, PlayerController.EntityMerlin.gameObject.transform.position); 
                TestBulletManager.TBM.CreateANewBullet2(eb.original_up+(Vector3)UnityEngine.Random.insideUnitCircle*2.0f, PlayerController.EntityMerlin.gameObject.transform.position); 
                TestBulletManager.TBM.CreateANewBullet2(eb.original_up+(Vector3)UnityEngine.Random.insideUnitCircle*2.0f, PlayerController.EntityMerlin.gameObject.transform.position); 
                TestBulletManager.TBM.CreateANewBullet2(eb.original_up+(Vector3)UnityEngine.Random.insideUnitCircle*2.0f, PlayerController.EntityMerlin.gameObject.transform.position); 
                TestBulletManager.TBM.CreateANewBullet2(eb.original_up+(Vector3)UnityEngine.Random.insideUnitCircle*2.0f, PlayerController.EntityMerlin.gameObject.transform.position); 
                TestBulletManager.TBM.CreateANewBullet2(eb.original_up+(Vector3)UnityEngine.Random.insideUnitCircle*2.0f, PlayerController.EntityMerlin.gameObject.transform.position); 

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
            eb.Speed -= eb.Speed * 0.008f;
        }
    }
}
