using System;
using UnityEngine;

public class SpellEntity_AerbulletPlus : SpellEntity, LogicalOperators
{
    public SpellEntity_AerbulletPlus()
    {
        Sprite_Location = "Textures/Bullets/AerbulletPlus";
        entity_types = Entity_Type.Bullet;
        basespeed = 0.1f;
        Blood_dec_ = 7.0f;
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

    public override void Emit(BulletBase bb)
    {
        intervaltime = 1.2f;
        //basespeed = basespeed * 0.7f;
        
        if(bb is EntityBullet eb)
        {
            if(Time.time - TestBulletManager.last_bullet_time < eb.intervaltime)
                return ;
            eb.skpara = 5.0f;
            eb.alive_distance_time = 4.0f;
        }
    }

    public override void Process(BulletBase bb)
    {
        if(bb is EntityBullet eb)
        {
            eb.Speed -= eb.Speed * 0.003f;
        }
    }

    public override void Hit(Collider2D Colliding_bullet, BulletBase bb)
    {
        Debug.Log("Fuck!");
        Enemy enemy = Colliding_bullet.gameObject.GetComponent<Enemy>();
        if(enemy != null)
        {
            enemy.Status.CurrentBlood -= Blood_dec * (1-enemy.Status.CurrentMagicDfense[3]);
            //bb.Destroy_bullet();
        }
    }
}
