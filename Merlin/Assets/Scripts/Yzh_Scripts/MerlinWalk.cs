using UnityEngine;
using UnityEngine.SceneManagement; // 引入场景管理命名空间
using TMPro;

public class PlayerController : Entity
{   
    //所有敌人共同攻击同一个Merlin，同时记录剩余敌人数量
    public static PlayerController EntityMerlin = null;
    public TMP_Text RecordEnemy = null;
    private int RemainingEnemy;
    
    //public GameObject respawnPrefab; // 关联的Prefab（通常是自己）
    //public float respawnDelay = 3f;  // 复活延迟时间

    // 初始状态保存
    private Vector3 initialPosition;
    private Quaternion initialRotation;
    
    // 共同受到一个控制
    //public GameManagerBehavior Controller;
    
    // 移动参数
    public float normalSpeed = 5f;
    //private bool IsMoving = true;
    public GameObject AskNewMap;

    public float sprintSpeed = 10f;
    [Range(0.1f, 1f)] 
    public float sprintSmoothness = 0.5f;

    // 攻击参数
    public float attackRange = 1.2f;
    public float attackWidth = 0.8f;
    public LayerMask attackLayerMask;

    // 死亡参数
    public float deathAnimDuration = 1.5f;

    // 组件引用
    private Rigidbody2D rb;
    private Animator animator;
    private Collider2D col;
    
    // 状态变量
    private Vector2 movement;
    private Vector2 lastDirection;
    private float currentSpeed;
    private bool isDead;

    // 动画参数
    private const string MoveX = "MoveX";
    private const string MoveY = "MoveY";
    private const string Moving = "Moving";
    private const string LastMoveX = "LastMoveX";
    private const string LastMoveY = "LastMoveY";
    private const string Attacking = "Attacking"; // Bool类型
    private const string Death = "Death";
    private const string DeathDirX = "DeathDirX";
    private const string DeathDirY = "DeathDirY";

    void Awake()
    {
        EntityMerlin = this;
    }

    void Start()
    {
        Readconfig();
        // 给共有的Merlin赋值
        //EntityMerlin = this;
        
        // 保存初始状态
        initialPosition = transform.position;
        initialRotation = transform.rotation;

        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        col = GetComponent<Collider2D>();
        currentSpeed = normalSpeed;
        lastDirection = Vector2.down;
        InitializeAnimationParameters();

        AskNewMap.SetActive(false);


        if(TheGlobalManager.TGM.IsSafe() == false)
        {
            RemainingEnemy = GameManagerBehavior.gm.NumberOfEnemy * 2;
            Debug.Log("Heyheyhey some enemy");
        }
        else    
            RemainingEnemy = 0;

    }

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Tab))
        //    IsMoving = !IsMoving;
        
        if (GameManagerBehavior.gm.IsMoving)
        {
            // 设置速度
            normalSpeed = Status.CurrentSpeed;
            sprintSpeed = 1.5f * normalSpeed;

            //base.Update();
            Status.UpdataAll();
            
            //if (isDead) return;

            HandleMovementInput();
            //HandleAttackInput();
            UpdateAnimationParameters();
            PropertyChange();
            CheckAskScene();
            RecordRemainingEnemy();

            // 测试用死亡命令（正式版需删除）
            if (Status.CurrentBlood<=0) Die();
        }
    }

    void CheckAskScene()
    {
        if (Mathf.Abs(transform.position.y) + 0.5f*Mathf.Abs(transform.position.x) >= 23.0f)
            AskNewMap.SetActive(true);
        else
            AskNewMap.SetActive(false);
    }

    void HandleMovementInput()
    {
        float horizontal = 0f;
        if (Input.GetKey(KeyCode.A)) horizontal = -1f;
        if (Input.GetKey(KeyCode.D)) horizontal = 1f;
        float vertical = Input.GetKey(KeyCode.S) ? -1f : Input.GetKey(KeyCode.W) ? 1f : 0f;

        movement = new Vector2(horizontal, vertical);

        Vector3 currentPosition = transform.position;
        float currentPositionSum = Mathf.Abs(currentPosition.x) * 0.5f + Mathf.Abs(currentPosition.y);

        Vector3 predictedPosition = currentPosition + (Vector3)(movement.normalized * currentSpeed * Time.deltaTime);
        float predictedPositionSum = Mathf.Abs(predictedPosition.x) * 0.5f + Mathf.Abs(predictedPosition.y);

        if (currentPositionSum > 24.5f && predictedPositionSum >= currentPositionSum)
        {
            movement = Vector2.zero;
            rb.velocity = Vector2.zero;
            return;
        }
        
        bool isSprinting = (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) 
                        && movement != Vector2.zero;
        
        currentSpeed = Mathf.Lerp(currentSpeed, 
            isSprinting ? sprintSpeed : normalSpeed, 
            sprintSmoothness);

        if (movement != Vector2.zero)
        {
            lastDirection = GetPrimaryDirection(movement);
            UpdateDirectionParameters(lastDirection);
        }
    }

    void HandleAttackInput() //无用
    {
        // 持续按下空格键保持攻击状态
        bool isAttacking = Input.GetMouseButtonDown(0);
        animator.SetBool(Attacking, isAttacking);

        // 按下瞬间触发攻击检测
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("I'm atacking");
            DetectAttack(GetAttackDirection());
        }
    }

    Vector2 GetAttackDirection()
    {
        return movement != Vector2.zero ? 
            GetPrimaryDirection(movement) : 
            lastDirection;
    }

    void DetectAttack(Vector2 direction) // 完全无用
    {
        Vector2 origin = (Vector2)transform.position + Vector2.up * 0.5f;
        RaycastHit2D[] hits = Physics2D.BoxCastAll(
            origin,
            new Vector2(attackWidth, attackWidth),
            0f,
            direction,
            attackRange,
            attackLayerMask);
        bool temp = false;
        foreach (var hit in hits)
        {
            temp = true;
            if (hit.collider.CompareTag("Enemy"))
            {
                Debug.Log($"击中敌人：{hit.collider.name}");
                // 此处添加敌人受伤逻辑
            }
        }
        if (temp)
        {
            Debug.Log("ohhohohhohohoho");
        } 
    }

    // Death and Life
    /*
    public void DestroyAndRespawn()
    {
        // 通知管理器复活
        RespawnManager.Instance.RespawnObject(
            respawnPrefab, 
            initialPosition, 
            initialRotation, 
            respawnDelay
        );
        // Destroy(gameObject); // 销毁当前对象
    }
    */

    public void Die()
    {
        if (isDead) return;
        isDead = true;

        Vector2 deathDirection = GetPrimaryDirection(lastDirection);
        animator.SetFloat(DeathDirX, deathDirection.x);
        animator.SetFloat(DeathDirY, deathDirection.y);
        animator.SetTrigger(Death);

        rb.velocity = Vector2.zero;
        rb.simulated = false;
        col.enabled = false;
        enabled = false;

        string currentSceneName = SceneManager.GetActiveScene().name;

        // 重新加载当前场景
        SceneManager.LoadScene(currentSceneName);

        // 输出调试信息
        Debug.Log("Current scene reloaded and all objects reset to their initial state.");
        Debug.Log("blood is" + Status.CurrentBlood);
        SceneManager.LoadScene("ExitScene");

        Destroy(gameObject, deathAnimDuration);
    }

    void UpdateAnimationParameters()
    {
        bool isMoving = movement.magnitude > 0.1f;
        animator.SetBool(Moving, isMoving);

        if (isMoving)
        {
            Vector2 primaryDir = GetPrimaryDirection(movement);
            animator.SetFloat(MoveX, primaryDir.x);
            animator.SetFloat(MoveY, primaryDir.y);
        }
    }

    void FixedUpdate()
    {
        if (!GameManagerBehavior.gm.IsMoving || isDead)
        {
            rb.velocity = Vector2.zero;
            return;
        }
        rb.velocity = movement.normalized * currentSpeed;
    }

    Vector2 GetPrimaryDirection(Vector2 input)
    {
        if (Mathf.Abs(input.x) > Mathf.Abs(input.y))
        {
            return input.x > 0 ? Vector2.right : Vector2.left;
        }
        return input.y > 0 ? Vector2.up : Vector2.down;
    }

    void UpdateDirectionParameters(Vector2 direction)
    {
        lastDirection = direction;
        animator.SetFloat(LastMoveX, direction.x);
        animator.SetFloat(LastMoveY, direction.y);
    }

    void InitializeAnimationParameters()
    {
        animator.SetFloat(LastMoveX, 0);
        animator.SetFloat(LastMoveY, -1);
    }

    // 调试可视化
    void OnDrawGizmosSelected()
    {
        if (!Application.isPlaying) return;

        // 移动方向指示（绿色）
        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, lastDirection * 2);

        // 攻击范围指示（红色）
        if (animator.GetBool(Attacking))
        {
            Gizmos.color = Color.red;
            Vector2 origin = (Vector2)transform.position + Vector2.up * 0.5f;
            Vector2 size = new Vector2(attackWidth, attackWidth);
            Vector2 end = origin + lastDirection * attackRange;
            
            Gizmos.DrawWireCube(origin + lastDirection * attackRange/2, size);
            Gizmos.DrawLine(origin, end);
        }

        // 死亡方向指示（黄色）
        if (isDead)
        {
            Gizmos.color = Color.yellow;
            Vector2 deathDir = new Vector2(
                animator.GetFloat(DeathDirX),
                animator.GetFloat(DeathDirY)
            );
            Gizmos.DrawRay(transform.position, deathDir * 3);
        }
    }

    private void PropertyChange()
    {
        switch (Status.CurrentSituation.Item1)
        {
            case EntityProperty.situation.Burn: 
                {
                    Status.CurrentBlood -= 0.5f * Time.smoothDeltaTime;
                }
                break;
            case EntityProperty.situation.Toxic:
                {
                    Status.CurrentBlood -= (Status.Blood * 0.05f) * Time.smoothDeltaTime;
                }
                break;
            default:
                break;
        }
    }

    // Been Attacked
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Status.CurrentSituation.Item2 = Status.SituationContinuousTime;
        switch (collision.gameObject.tag)
        {
            case "FireAttack":
                {
                    Status.CurrentBlood -= 2.5f * (1-Status.CurrentMagicDfense[1]);
                    if (Status.CurrentSituation.Item1==EntityProperty.situation.Nothing || Status.CurrentSituation.Item2<=0)
                        Status.CurrentSituation.Item1 = EntityProperty.situation.Burn;
                }
                break;
            case "WaterAttack":
                {
                    Status.CurrentBlood -= 2.5f * (1-Status.CurrentMagicDfense[2]);
                    if (Status.CurrentSituation.Item1==EntityProperty.situation.Nothing || Status.CurrentSituation.Item2<=0)
                    {
                        Status.CurrentSituation.Item1 = EntityProperty.situation.Frozen;
                        Status.CurrentSpeed *= 0.5f;
                    }
                }
                break;
            case "AerAttack":
                {
                    Status.CurrentBlood -= 1.5f * (1-Status.CurrentMagicDfense[3]);
                }
                break;
            case "RockAttack":
                {
                    Status.CurrentBlood -= 3.5f * (1-Status.CurrentMagicDfense[4]);
                }
                break;
                        case "RedBall":
                {
                    Status.CurrentBlood -= 1.25f * (1-Status.CurrentMagicDfense[1]);
                }
                break;
            case "GreenBall":
                {
                    Status.CurrentBlood -= 2.0f * (1-Status.CurrentMagicDfense[4]);
                }
                break;
            case "BlueBall":
                {
                    Status.CurrentBlood -= 1.25f * (1-Status.CurrentMagicDfense[2]);
                    if (Status.CurrentSituation.Item1==EntityProperty.situation.Nothing || Status.CurrentSituation.Item2<=0)
                    {
                        Status.CurrentSituation.Item1 = EntityProperty.situation.Wet;
                        Status.CurrentSpeed *= 0.75f;
                    }
                }
                break; 
            case "BossBall":
                {
                    Status.CurrentBlood -= 2.5f;
                    if (Status.CurrentSituation.Item1==EntityProperty.situation.Nothing || Status.CurrentSituation.Item2<=0)
                    {
                        Status.CurrentSituation.Item1 = EntityProperty.situation.Toxic;
                    }
                } 
                break;
            case "BossBallFire":
                {
                    Status.CurrentBlood -= 3.0f;
                    if (Status.CurrentSituation.Item1==EntityProperty.situation.Nothing || Status.CurrentSituation.Item2<=0)
                    {
                        Status.CurrentSituation.Item1 = EntityProperty.situation.Toxic;
                    }
                }
                break;
            default:
                break;
        }
    }

    public void OneLessEnemy()
    {
        RemainingEnemy--;
        if(RemainingEnemy <= 0)
        {
            TheGlobalManager.TGM.SetSafe();
        }
    }

    private void RecordRemainingEnemy()
    {
        RecordEnemy.text = "Remaining Enemy:   " + RemainingEnemy.ToString(); 
    }
}