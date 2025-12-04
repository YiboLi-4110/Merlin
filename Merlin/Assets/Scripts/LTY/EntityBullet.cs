using Unity.VisualScripting;
using UnityEngine;

public class EntityBullet : BulletBase
{
    // Start is called before the first frame update

    //public SpellEntity spellentity = null;

    public int logical_num = 0;

    private float born_time = 0; 
    public float alive_distance_time = 1.0f;

    public float Speed = 0.05f;

    public bool copy = false;

    //public int collietime = 1;

    public Vector3 original_position;
    public Vector3 original_up;

    public float skpara = 0.5f;

    public LogicalOperators[] operators = null; 

    public SpellEntity bullet_main_entity = null;

    public float intervaltime;    

    public static EntityBullet Create(Vector3 MerlinPosition, Vector3 Bullet_up, bool Iscopy)
    {
        //Bullet_up += (Vector3)(UnityEngine.Random.insideUnitCircle * 0.3f);
        GameObject ABullet = new GameObject("ABullet");
        ABullet.tag = "Bullet";
        EntityBullet bullet_instance = ABullet.AddComponent<EntityBullet>();

        Debug.Log("Iscopy == " + Iscopy);
        bullet_instance.copy = Iscopy;
        bullet_instance.logical_num = TestBulletManager.TBM.setoperators(ref bullet_instance.operators, ref bullet_instance.bullet_main_entity);
        Bullet_up.z = 0f;
        bullet_instance.original_up = Bullet_up;

        bullet_instance.original_position = MerlinPosition;
        SpriteRenderer thisSpriteRenderer = ABullet.AddComponent<SpriteRenderer>();
        thisSpriteRenderer.sprite = Instantiate(Resources.Load<Sprite>(bullet_instance.bullet_main_entity.Sprite_Location));
        thisSpriteRenderer.sortingLayerName = "flowers";
        thisSpriteRenderer.sortingOrder = 1;

        CircleCollider2D Bullet_collider = ABullet.AddComponent<CircleCollider2D>();
        Bullet_collider.isTrigger = true;


        if(bullet_instance.operators == null)
        {
            Debug.Log("FUUUCK2");
        }

        //Debug.Log("entityid: " + bullet_instance.GetInstanceID() + "  count: " + TBM.count);
        return bullet_instance;
    }

    void Start()
    {
        transform.position = original_position;
        //transform.up = original_up;
        Speed = bullet_main_entity.basespeed;

        bullet_main_entity.Emit(this);
        intervaltime = bullet_main_entity.intervaltime;
        Debug.Log("this time internal = " + intervaltime);
        if(operators == null)
        {
            Debug.Log("Empty pointer");
        }
        else
        {
            Debug.Log("Accessful Start EntityBullrt");
            for(int i = 0; i < logical_num; i++)
            {
                if(operators[i] == null)
                {
                    Debug.Log("operators[i] is null! " + i);

                }
                else 
                {
                    operators[i].Emit(this);
                    Debug.Log("internal = "+intervaltime);
                }
            }
            
        }
        original_up += (Vector3)(UnityEngine.Random.insideUnitCircle * skpara);
        //transform.up = original_up;

        //bullet_main_entity.Emit(this);

        if(copy == false && (Time.time - TestBulletManager.last_bullet_time) < intervaltime)
        {
            Debug.Log("interval time " + bullet_main_entity.intervaltime);
            Destroy_bullet();
            return;
        }

        //bullet_main_entity.Emit(this);

        TestBulletManager.last_bullet_time = Time.time;
        born_time = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        //Speed = bullet_main_entity.GetSpeed();
        transform.position += original_up.normalized * Speed * Time.smoothDeltaTime * 100;
        bullet_main_entity.Process(this);
        for(int i = 0; i < logical_num; i++)
        {
            if(operators[i] == null)
            {
                Debug.Log("operators[i] is null!" + i);

            }
            else
            {
                //Debug.Log("update");
                operators[i].Process(this);
            }
        }
        
        if(Time.time - born_time >= alive_distance_time)
        {
            Destroy_bullet();
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("hhhhhhhhh");
        
        for(int i = 0; i < logical_num; i++)
        {
            if(operators[i] == null)
                continue;
            operators[i].Hit(collision, this);
        }
        
        bullet_main_entity.Hit(collision, this);
    }

}
