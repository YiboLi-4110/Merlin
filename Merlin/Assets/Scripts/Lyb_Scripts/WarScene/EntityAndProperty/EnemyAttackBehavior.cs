using UnityEngine;

public class EnemyAttackBehavior : MonoBehaviour
{
    public GameManagerBehavior Controller = null;
    public float AttackSpeed = 4f;
    //记录存活时间
    private float LifeCount;

    // Start is called before the first frame update
    void Start()
    {
        LifeCount = Time.time;
        Controller = GameManagerBehavior.gm;

        if (gameObject.CompareTag("AerAttack"))
            AttackSpeed *= 1.8f;
    }

    // Update is called once per frame
    void Update()
    {
        if (Controller.IsMoving)
            transform.position += (AttackSpeed * Time.smoothDeltaTime) * transform.up;
        if (Time.time - LifeCount >= 2.5f)
            Destroy(transform.gameObject);
    }

    // collision
    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Merlin":
                {
                    Destroy(transform.gameObject);
                }
                break;
            default:
                break;
        }
    }
}

