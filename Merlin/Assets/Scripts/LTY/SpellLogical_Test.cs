using UnityEngine;

public class SpellLogical_Test : SpellLogical, LogicalOperators
{
    public ParticleSystem Emit_Particle = null;

    private float last_time = 0.5f;

    public SpellLogical_Test()
    {
        Emit_Particle = Object.Instantiate(Resources.Load<ParticleSystem>("Prefabs/Particle/Exploding"));
        if(Emit_Particle == null)
        {
            Debug.Log("Particle is null");
        }
        else
        {
            Debug.Log("Particle is done");
        }

    }

    public override void Emit(BulletBase Hanging_bullet_base)
    {
        if(Hanging_bullet_base != null)
        {
            if(Hanging_bullet_base is EntityBullet Hanging_bullet)
            {
                Emit_Particle.transform.position = Hanging_bullet.gameObject.transform.position;
                Debug.Log("Play particle");
                Emit_Particle.Play();
            }
        }
        
    }

    public override void Process(BulletBase Hanging_bullet_base)
    {
        if(Hanging_bullet_base != null)
        {
            if(Hanging_bullet_base is EntityBullet Hanging_bullet)
            {
                SpriteRenderer sr = Hanging_bullet.GetComponent<SpriteRenderer>();
                if(sr == null)
                {
                    Debug.Log("SR is null!");
                    return;
                }
                Color Flashing_Color1 = sr.color;
            
                if(Time.realtimeSinceStartup - last_time > 0.1f)
                {
                    if(Flashing_Color1.a != 1.0f)
                    {
                        Flashing_Color1.a = 1.0f;
                    }
                    else
                    {
                        Flashing_Color1.a = 0.1f;
                    }
                    sr.color = Flashing_Color1;
                    Debug.Log("Color update sr" + sr.GetInstanceID());
                    Debug.Log("Color update object" + Hanging_bullet.GetInstanceID());
                    last_time = Time.realtimeSinceStartup;
                }
            }

        }
        
    }

    public override void Hit(Collider2D Colliding_bullet, BulletBase Hanging_bullet_base)
    {
        if(Hanging_bullet_base != null)
        {
            if(Hanging_bullet_base is EntityBullet Hanging_bullet)
            {
                Emit_Particle.transform.position = Hanging_bullet.transform.position;
                Emit_Particle.Play();
            }
        }
    }

    public void Spell_Destroy()
    {
        last_time = 0;
    }

}
