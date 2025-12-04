using UnityEngine;

public class SpellEntity_Ignisbullet : SpellEntity, LogicalOperators
{
    public SpellEntity_Ignisbullet()
    {
        Sprite_Location = "Textures/Bullets/Ignisbullet";
        entity_types = Entity_Type.Bullet;
        basespeed = 0.09f;
        Blood_dec_ = 10.0f;
        Defence_dec_ = 0;

    }

    public override float Blood_dec
    {
        get => Blood_dec_;
        set => Blood_dec_ = value;
    }

    public override float Defence_dec
    {
        get => Defence_dec_;
        set => Defence_dec_ = value;
    }
    //public byte Element_type = 1<<1; //Aqua


    public override void Hit(Collider2D Colliding_bullet, BulletBase bb)
    {
        Debug.Log("Fuck!");
        Enemy enemy = Colliding_bullet.gameObject.GetComponent<Enemy>();
        if(enemy != null)
        {
            enemy.Status.CurrentBlood -= Blood_dec * (1-enemy.Status.CurrentMagicDfense[1]);
            if (enemy.Status.CurrentSituation.Item1==EntityProperty.situation.Nothing || enemy.Status.CurrentSituation.Item2<=0)
                enemy.Status.CurrentSituation.Item1 = EntityProperty.situation.Burn;
            bb.Destroy_bullet();
        }
    }


}
