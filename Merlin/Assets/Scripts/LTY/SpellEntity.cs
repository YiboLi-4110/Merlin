using UnityEngine;
public abstract class SpellEntity : SpellComponentBase, LogicalOperators
{   
    public enum Entity_Type{Meta, Bullet, Range}

    public Entity_Type entity_types = Entity_Type.Meta;
    public string Sprite_Location = null;
    
    public float basespeed = 0.1f;
    public float Blood_dec_ = 1.0f;
    public float Defence_dec_ = 0;

    public float sustaintime = 1.5f;

    public float intervaltime = 0.3f;
    
    protected SpellEntity()
    {
        spelltype = SpellComponentType.Entity;
    }

    public abstract float Blood_dec
    {
        get;
        set;
    }
    public abstract float Defence_dec
    {
        get;
        set;
    }

    public virtual void Emit(BulletBase bb)
    {
        if(bb is EntityBullet eb)
        {
            Vector2 or = new Vector2(1,0);
            Vector2 thisvec = new Vector2(eb.original_up.x, eb.original_up.y);
            eb.transform.Rotate(0, 0, Vector2.SignedAngle(or, thisvec));
            Debug.Log("Rotate angle" + Vector2.SignedAngle(or, thisvec) + "  is " + thisvec.x + " " + thisvec.y);
        }

    }
    public virtual void Process(BulletBase bb){}
    public virtual void Hit(Collider2D Colliding_bullet, BulletBase bb){}
    public virtual void Blast(){}

    public float GetSpeed()
    {
        return basespeed;
    }
}
