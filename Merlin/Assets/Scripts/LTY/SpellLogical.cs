using UnityEngine;

public interface LogicalOperators
    {
        public void Emit(BulletBase bb);
        public void Process(BulletBase bb);
        public void Hit(Collider2D Colliding_bullet, BulletBase bb);
        public void Blast();
    }

public abstract class SpellLogical : SpellComponentBase, LogicalOperators
{   
    protected SpellLogical()
    {
        spelltype = SpellComponentType.Logical;
    }

    
    
    public virtual void Emit(BulletBase bb){}
    public virtual void Process(BulletBase bb){}
    public virtual void Hit(Collider2D Colliding_bullet, BulletBase bb){}
    public virtual void Blast(){}
}

