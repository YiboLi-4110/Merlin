using System;
using UnityEngine;

public class TestBulletManager : MonoBehaviour
{
    public static TestBulletManager TBM = null;

    public GameObject Merlin_entity = null;

    Type[] operators_type = new Type[5];

    //LogicalOperators[] operators = null;

    SpellEntity main_bullet_entity = null;

    public int extension_num = 0;

    //private int global_bullet = 1; 
    public int count = 0;

    // 初始攻击时间
    public static float last_bullet_time = 0;

    public float AttackEnemyTime = 0f;

    public int setoperators(ref LogicalOperators[] operators_, ref SpellEntity main_bullet_entity_)
    {
        main_bullet_entity_ = main_bullet_entity;
        operators_ = new LogicalOperators[extension_num];
        if(operators_type == null)
        {
            Debug.Log("NULL Operator_type");
            return 0;
        }
        for(int i = 0; i < extension_num; i++)
        {
            if(operators_type[i] == null)
                continue;
            var type_instance = Activator.CreateInstance(operators_type[i]);
            if(type_instance is LogicalOperators type_Instance)
            {
                operators_[i] = type_Instance;
            }
            else
            {
                Debug.Log("Invalid interface type");
            }
        }

        if(operators_ == null)
        {
            Debug.Log("FUUUCK1");
        }
        return extension_num; 
    }


    public void sequence_set(GameObject []boxes, int num)
    {
        int index = 0;
        for(int i = 0; i < num; i++)
        {
            Boxselected box = boxes[i].GetComponent<Boxselected>();
            if(box.IsBulllet)
            {
                if(Activator.CreateInstance(box.SpellType) is SpellEntity Spe)
                    main_bullet_entity = Spe;
                //main_bullet_entity = 
               
                //extension_num++;
            }
            else if(box.IsComponent)
            {
                operators_type[index] = box.SpellType;
                index++;
                extension_num++;
            }
        }
    }

    void Awake()
    {
        TBM = this;
    }



    // Start is called before the first frame update
    void Start()
    {
        Merlin_entity = PlayerController.EntityMerlin.gameObject;
        //operators = new LogicalOperators[]{new SpellLogical_Test()};
        //operators_type = new Type[]{typeof(SpellLogical_Test)};
        //bullet_type = typeof(SpellEntity_Aquabullet);
        //extension_num = 0;
    }









    // Update is called once per frame
    void Update()
    {
        if(GameManagerBehavior.gm.IsMoving)
        {
            if(Input.GetMouseButtonDown(0))
            {
                Vector3 mousepos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                if(main_bullet_entity == null)
                {
                    Debug.Log("NO BULLET");
                }
                else
                {
                    Debug.Log("here not empty");
                    /*
                    if(global_bullet == 1)   
                        main_bullet_entity = new SpellEntity_Aquabullet();
                    else
                    {
                        main_bullet_entity = new SpellEntity_Aerbullet();
                    }*/
                    CreateANewBullet(mousepos);
                    
                }
            }

        }
        
        /*if(Input.GetKeyDown(KeyCode.T))
        {
            extension_num = 1;
        }
        if(Input.GetKeyDown(KeyCode.Y))
        {
            extension_num = 0;
        }
        if(Input.GetKeyDown(KeyCode.B))
        {
            global_bullet = 0;
        }
        if(Input.GetKeyDown(KeyCode.N))
        {
            global_bullet = 1;
        }*/
    }


    public void CreateANewBullet(Vector3 targetpos)
    {
        Debug.Log("OOOOOOOOOOOOOO");
        Vector3 pos = Merlin_entity.transform.position;
        pos.z = 0f;
        pos += new Vector3(0, 0.6f, 0);
                    
                    //mousepos = new Vector3(0,1,0);
        targetpos.z = 0;

                    //count++;

        EntityBullet eb = EntityBullet.Create(pos, targetpos-pos, false);
                    
        if(eb.operators == null)
        {
            Debug.Log("Already failed");
        }
        else Debug.Log("Good");        
    }

    public void CreateANewBullet2(Vector3 targetpos, Vector3 pos)
    {
        /*Vector3 pos = Merlin_entity.transform.position;
        Debug.Log("double bullet pos is " + pos.x + " " + pos.y);
        pos.z = 0f;
        pos += new Vector3(0, 0.6f, 0);*/
                    
                    //mousepos = new Vector3(0,1,0);
        targetpos.z = 0;

                    //count++;

        //targetpos += (Vector3)UnityEngine.Random.insideUnitCircle ;
        EntityBullet eb = EntityBullet.Create(pos, targetpos, true);
                    
        if(eb.operators == null)
        {
            Debug.Log("Already failed");
        }
        else Debug.Log("Good");        
    }
}
