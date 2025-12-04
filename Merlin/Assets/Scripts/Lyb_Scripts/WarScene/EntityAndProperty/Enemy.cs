
using UnityEngine;


public class Enemy : Entity
{
    // 共同收到同一个GameObject的控制
    //public GameManagerBehavior Controller;
    
    // Enemy随机移动参数
    public float moveSpeed;          // 移动速度
    public Vector2 areaSize = new Vector2(8f, 8f); // 移动区域的大小
    //private bool IsMoving = true;  // 是否移动

    private Vector3 moveDirection;        // 当前移动方向
    private Vector3 initialPosition;      // 初始位置（区域中心）
    private float minX, maxX, minY, maxY; // 移动边界

    // Enemy攻击参数
    public PlayerController targetObject = null;   // 目标对象GameObjectB
    private GameObject AttackThing; // 发射的预制体
    public float AttackMinDistance = 8f;  // 检测范围
    public float AttackTime = 0f;  // 攻击时间

    // Enemy的属性卡片
    public GameObject PropertyCanvas;
    public BarManager ShowMovingBlood;

    // Entity的Awake函数被默认继续调用

    /*void Awake()
    {
        
    }*/

    void Start()
    {
        Readconfig();
        // 确定攻击目标Merlin
        targetObject = PlayerController.EntityMerlin;
        
        RandomlyInitializePosition();
        
        // Enemy的初始移动设置
        moveSpeed = Status.CurrentSpeed;
        initialPosition = transform.position;
        CalculateBounds();
        moveDirection = Random.insideUnitCircle.normalized; // 初始随机方向
        moveDirection.z = 0;

        // Enemy的属性卡片初始设置
        // 确保 UI Canvas 在游戏开始时是隐藏的
        if (PropertyCanvas != null)
        {
            PropertyCanvas.SetActive(false);
        }

        // Enemy初始攻击设置
        AttackTime = Time.time;
    }

    // Update is called once per frame
    private void Update()
    {

        //if (Input.GetKeyDown(KeyCode.Tab))
        //    IsMoving = !IsMoving;

        if (GameManagerBehavior.gm.IsMoving)
        {
            // 显式调用Entity的Update函数
            //base.Update(); 
            Status.UpdataAll();       
            // Enemy的随机有范围移动
            UpdatePosition();

            // Enemy对Merlin的近距离攻击
            AttackMerlin();

            // Enemy对鼠标点击的反应
            CheckClick();

            // Enemy's moving blood volume
            ShowMovingBlood.UpdateBar(Status.CurrentBlood);
        }

        if (Status.CurrentBlood <= 0)
        {
            targetObject.OneLessEnemy();
            Destroy(transform.gameObject);
        }
    }

    private void RandomlyInitializePosition()
    {
        float RandomX = Random.Range(-35,35);
        float HelpRandomY = Random.Range(-1,1);
        float SignRandomY;
        if (HelpRandomY >= 0) SignRandomY = 1;
        else SignRandomY = -1;
        float RandomY = (Random.Range(Mathf.Abs(RandomX)*0.5f,24.3f) - Mathf.Abs(RandomX)*0.5f) * SignRandomY;
        Vector3 RandomOriginalPosition = new Vector3(RandomX,RandomY,0);
        transform.position = RandomOriginalPosition;
    }

    void CalculateBounds()
    {
        minX = initialPosition.x - areaSize.x / 2f;
        maxX = initialPosition.x + areaSize.x / 2f;
        minY = initialPosition.y - areaSize.y / 2f;
        maxY = initialPosition.y + areaSize.y / 2f;
    }

    void CheckBoundaryCollision(Vector3 newPosition)
    {
        //bool hitBoundary = false;
        
        // 检查X轴是否碰到边界
        if (newPosition.x <= minX || newPosition.x >= maxX)
        {
            moveDirection.x *= -1; // 反转X方向
            //hitBoundary = true;
        }

        // 检查Y轴是否碰到边界
        if (newPosition.y <= minY || newPosition.y >= maxY)
        {
            moveDirection.y *= -1; // 反转Y方向
            //hitBoundary = true;
        }

        if (Mathf.Abs(newPosition.y) + 0.5f*Mathf.Abs(newPosition.x) >= 24.3f)
        {
            moveDirection.x *= -1;
            moveDirection.y *= -1;
            // 偏移一定距离，避免下一帧再次触发
            newPosition += moveDirection * 0.5f; 
        }
        // 可选：碰到边界时生成新随机方向
        /*
        if (hitBoundary)
        {
             moveDirection = Random.insideUnitCircle.normalized;
        }
        */
    }

    private void UpdatePosition()
    {
        // 计算速度
        moveSpeed = Status.CurrentSpeed;
        
        // 计算新位置
        Vector3 newPosition = transform.position + (moveSpeed * Time.smoothDeltaTime) * moveDirection;

        // 限制新位置在边界内
        newPosition.x = Mathf.Clamp(newPosition.x, minX, maxX);
        newPosition.y = Mathf.Clamp(newPosition.y, minY, maxY);
        newPosition.z = 0;


        // 检测是否碰到边界并反弹
        CheckBoundaryCollision(newPosition);

        // 更新位置
        transform.position = newPosition;
    }

    private void AttackMerlin()
    {
        if (targetObject != null)
        {
            // 计算与目标的距离
            float distance = Vector3.Distance(transform.position, targetObject.transform.position);

            // 如果目标在范围内则发射攻击
            if (distance <= AttackMinDistance && (Time.time - AttackTime) >= 1f)
            {
                if (gameObject.tag == "AerMonster")
                    AttackThing = Instantiate(Resources.Load("Prefabs/AerBall") as GameObject);
                else if (gameObject.tag == "WaterMonster")
                    AttackThing = Instantiate(Resources.Load("Prefabs/WaterBall") as GameObject);
                else if (gameObject.tag == "RockMonster")
                    AttackThing = Instantiate(Resources.Load("Prefabs/RockBall") as GameObject);
                else if (gameObject.tag == "FireMonster")
                    AttackThing = Instantiate(Resources.Load("Prefabs/FireBall") as GameObject);
                else if (gameObject.tag == "LittleMonster_0")
                    AttackThing = Instantiate(Resources.Load("Prefabs/RedBall") as GameObject);
                else if (gameObject.tag == "LittleMonster_1")
                    AttackThing = Instantiate(Resources.Load("Prefabs/GreenBall") as GameObject);
                else if (gameObject.tag == "Boss_0")
                    AttackThing = Instantiate(Resources.Load("Prefabs/BossBall") as GameObject);
                else if (gameObject.tag == "Boss_1")
                    AttackThing = Instantiate(Resources.Load("Prefabs/BossBallFire") as GameObject);
                AttackThing.transform.localPosition = transform.localPosition;
                AttackThing.transform.up = (targetObject.transform.position - AttackThing.transform.position).normalized;
                AttackTime = Time.time;
            }
        }
    }

    private void CheckClick()
    {
        // 检测鼠标点击
        if (Input.GetMouseButtonDown(1))
        {
            // 发射射线检测点击的对象
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

            float distance = Vector2.Distance(mousePosition, transform.position);

            if (hit.collider != null)
            {
                // 如果点击的是当前对象，显示 UI
                if (distance<=10f || hit.collider.gameObject == gameObject)
                {
                    if (PropertyCanvas != null)
                        PropertyCanvas.SetActive(true);
                }
                // 如果点击的是其他地方，隐藏 UI
                else
                {
                    if (PropertyCanvas != null)
                        PropertyCanvas.SetActive(false);
                }
            }
            else
            {
                if (PropertyCanvas != null)
                    PropertyCanvas.SetActive(false);
            }
        }
    }
}
