using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpider : MonoBehaviour
{
    public int attack = 5;
    public int hp = 100;

    public Animator anim;
    GameObject target;
    NavMeshAgent agent;

    public float attackRadius = 2;
    public float lookRadius = 15;


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

            hp = value;
            if (hp <= 0)
            {
                hp = 0;
                SetState(State.Die, "Die");
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
        agent.stoppingDistance = attackRadius;
        state = State.Move;
    }

    void Update()
    {
        print(state);
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
        agent.isStopped = true;
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
            SetState(State.Attack, "Attack");
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
        // 1. target�� ���� �̵��ϰ�ʹ�.
        agent.isStopped = false;
        agent.SetDestination(target.transform.position);

        float distance = Vector3.Distance(transform.position, target.transform.position);
        // ���� target���� �Ÿ��� ���ݰ��� �Ÿ���� Attack���·� �����ϰ� �ʹ�.
        if (distance <= attackRadius)
        {
            SetState(State.Attack, "Attack");
        }
        // �׷����ʰ� ���� target�� �����ݰ��� ����� Idle���·� �����ϰ� �ʹ�.

    }


    /// <summary>
    /// ���� �ִϸ��̼��� �� �����ӿ�
    /// </summary>
    public void OnAnimationAttackFinished()
    {
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
        // target�� attackRadius�ȿ� ������
        float distance = Vector3.Distance(transform.position, target.transform.position);
        if (distance <= attackRadius)
        {
            // target(Player)���� �������� ������ʹ�.
            CJ_PlayerHP.instance.HP--;
        }
    }

    /// <summary>
    /// Damage�� Player�� Enemy�� ������ ���̴�.
    /// �÷��̾ Enemy�� Ÿ�������� ȣ���ؾ��Ѵ�.
    /// </summary>
    public void SetDamaged(int damage)
    {
        HP -= damage;
    }




}
