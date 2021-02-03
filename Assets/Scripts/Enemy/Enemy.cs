using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(CapsuleCollider))]
public class Enemy : MonoBehaviour
{
    public float health, damage, speed;
    public float rotateSpeed = 180;
    Rigidbody rb;
    Animator anim;
    public float hitRange;
    Transform target;
    public float value;
    public float hitSpeed;
    private float countdown = 0;
    bool dead = false;

    [ContextMenu("Apply Rigidbody And Capsule Collider")]
    void ApplyRBandCols()
    {
        rb = GetComponent<Rigidbody>();

        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;

        CapsuleCollider col = GetComponent<CapsuleCollider>();

        col.height = 1.6f;
        col.radius = 0.34f;
    }

    void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();

        //ApplyRBandCols();
    }

    void Start()
    {
        target = GameMaster4D.instance.player.transform;
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if(health <= 0)
        {
            dead = true;
            anim.SetTrigger("Die");
            GetComponent<Collider>().enabled = false;
            EnemySpawner.alive--;
            PlayerStats.money += value;
            Destroy(gameObject, 5);
        }
    }

    void FixedUpdate()
    {
        if (dead) return;

        Vector3 dir = target.position - transform.position;
        float dist = dir.magnitude;

        countdown -= Time.deltaTime;

        bool move = dist > hitRange;

        anim.SetBool("moving", move);

        dir.y = 0;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), rotateSpeed * Time.fixedDeltaTime);

        if(!move)
        {
            if(countdown <= 0)
            {
                PlayerStats.health -= damage;
                countdown = 1f / hitSpeed;
                anim.SetTrigger("Attack");
            }
        }
        else
        {
            rb.MovePosition(rb.position + transform.forward * speed * Time.fixedDeltaTime);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, hitRange);
    }
}
