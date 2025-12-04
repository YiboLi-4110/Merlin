using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellEntity_Thunderbullet : SpellEntity, LogicalOperators
{
    public SpellEntity_Thunderbullet()
    {
        Sprite_Location = "Textures/Bullets/Thunderbullet";
        entity_types = Entity_Type.Bullet;
        basespeed = 0.2f;
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
            enemy.Status.CurrentBlood -= Blood_dec;
            bb.Destroy_bullet();
        }
    }

    //public byte Element_type = 1<<2; //Aer
}
