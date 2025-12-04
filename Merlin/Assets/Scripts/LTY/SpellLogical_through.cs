using UnityEngine;

public class SpellLogical_through : SpellLogical, LogicalOperators
{
    public override void Emit(BulletBase bb)
    {
        bb.collidetime++;
    }
    /*public override void Hit(Collider2D Colliding_bullet, BulletBase bb)
    {
        Enemy enemy = Colliding_bullet.gameObject.GetComponent<Enemy>();
        if(enemy != null)
        {
            if(bb is EntityBullet eb)
            {
                if(eb.bullet_main_entity is SpellEntity_AerbulletPlus || eb.copy == true)
                {
                    Debug.Log("cant use this");
                    return;
                }
                if(eb.copy == false)
                {
                    Debug.Log("Double Bullet!!!!!");
                    TestBulletManager.TBM.CreateANewBullet2(eb.original_up, Colliding_bullet.gameObject.transform.position + eb.original_up.normalized*2.0f); 
                }  
                else
                {
                    Debug.Log("Double false????????");
                } 
            }
        }
    }*/
}
