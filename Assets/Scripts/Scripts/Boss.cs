using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Boss : MonoBehaviour
{
    public int attack = 5;
    public int hp = 100;

    public Animator anim;
    GameObject target;
    NavMeshAgent agent;

    public float attackRadius = 2;
    public float lookRadius = 15;
    public float battleRadius = 30;

    float distance;


    public enum State
    {
        Idle,
        Move,
        Attack,
        Die,
        GetHit
    }
    public State state;

    public int HP
    {
        get { return hp; }
        set
        {
            bool isDamage = (hp > value);
            GetComponent<Collider>().enabled = false;

            hp = value;
            if (hp <= 0)
            {
                hp = 0;
                SetState(State.Die, "Die");
                //agent.isStopped = true;
                Destroy(gameObject, 3);
            }
            else if (isDamage)
            {
                SetState(State.GetHit, "GetHit");
            }
        }
    }

    void Start()
    {
        target = GameObject.Find("Player");
        agent = GetComponent<NavMeshAgent>();
        //agent.stoppingDistance = attackRadius;
        state = State.Idle;
    }

    void Update()
    {

        switch (state)
        {
            case State.Idle:
                UpdateIdle();
                break;
            case State.Move:
                UpdateMove();
                break;
        }
    }

    private void UpdateIdle()
    {
        if (agent.isOnNavMesh)
        {
            agent.isStopped = true;
        }
        //�÷��̾ ���ʹ� lookRadius ���� ������ �̵����·� ���� 
        int layer = 1 << LayerMask.NameToLayer("Player");
        Collider[] cols = Physics.OverlapSphere(transform.position, lookRadius, layer);
        if (cols.Length > 0)
        {
            target = cols[0].gameObject;

            AutoMoveOrAtattck();
        }
        else
        {
            target = null;
        }
    }

    void SetState(State next, string trigger)
    {
        state = next;
        anim.CrossFade(trigger, 0.1f);
    }

    void AutoMoveOrAtattck()
    {
        if (CanAttack())
        {
            //GetComponent<AudioSource>().Play();
            SetState(State.Attack, "Attack2");
        }
        else
        {
            SetState(State.Move, "Move");
        }
    }

    bool CanAttack()
    {
        // target�� ������ ������ �� ������ ��ȯ�ϰ�ʹ�.
        if (target == null)
            return false;
        // �Ÿ��� �����ؼ� ���ݰ��ɿ��θ� ��ȯ�ϰ�ʹ�.
        float distance = Vector3.Distance(transform.position, target.transform.position);
        return distance < attackRadius;
    }

    private void UpdateMove()
    {
        if (agent.isOnNavMesh)
        {
            print("onnavmesh");
            agent.isStopped = true;
        }
        // 1. target�� ���� �̵��ϰ�ʹ�.
        agent.isStopped = false;
        agent.SetDestination(target.transform.position);

        float distance = Vector3.Distance(transform.position, target.transform.position);
        // ���� target���� �Ÿ��� ���ݰ��� �Ÿ���� Attack���·� �����ϰ� �ʹ�.
        if (distance <= attackRadius)
        {
            GetComponent<AudioSource>().Play();
            SetState(State.Attack, "Attack2");
        }
        // �׷����ʰ� ���� target�� �����ݰ��� ����� Idle���·� �����ϰ� �ʹ�.
        else if (distance > battleRadius)
        {
            target = null;
            SetState(State.Idle, "Idle");
        }
    }


    /// <summary>
    /// ���� �ִϸ��̼��� �� �����ӿ�
    /// </summary>
    public void OnAnimationAttackFinished()
    {
        if (target == null)
        {
            return;
        }

        // target�� attackRadius�� �������
        float distance = Vector3.Distance(transform.position, target.transform.position);
        if (distance > attackRadius)
        {
            // Move���·� �����ϰ�ʹ�.
            SetState(State.Move, "Move");
        }
    }

    /// <summary>
    /// Hit�� Enemy�� Player�� ������ ���̴�.
    /// ���� �ִϸ��̼� �� Ÿ�� �����ӿ�
    /// </summary>
    public void OnAnimationAttackHit()
    {
        if (target == null)
        {
            return;
        }

        // target�� attackRadius�ȿ� ������
        float distance = Vector3.Distance(transform.position, target.transform.position);
        if (distance <= attackRadius)
        {
            // target(Player)���� �������� ������ʹ�.
            CJ_PlayerHP.instance.HP -= attack;
            CJ_PlayerMove.instance.anim.SetTrigger("Hit");
        }
    }

    public void OnAnimationGetHitFinished()
    {
        GetComponent<Collider>().enabled = true;
        SetState(State.Idle, "Idle");
    }

    /// <summary>
    /// Damage�� Player�� Enemy�� ������ ���̴�.
    /// �÷��̾ Enemy�� Ÿ�������� ȣ���ؾ��Ѵ�.
    /// </summary>
    public void SetDamaged(int damage)
    {
        HP -= damage;

    }




    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, battleRadius);
    }

}
