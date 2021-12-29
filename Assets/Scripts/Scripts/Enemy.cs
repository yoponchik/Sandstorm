using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
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

    public float blinkIntensity;
    public float blinkDuration;
    float blinkTimer;
    SkinnedMeshRenderer skinnedMeshRenderer;
    public GameObject bodyToHighlight;

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
                agent.isStopped = true;
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
        agent.stoppingDistance = attackRadius;
        state = State.Idle;
    }

    void Update()
    {
        //hitHighlight();

        //print(state);
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
        //플레이어가 에너미 lookRadius 내에 들어오면 이동상태로 전이 
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
        // target이 없으면 공격할 수 없음을 반환하고싶다.
        if (target == null)
            return false;
        // 거리를 측정해서 공격가능여부를 반환하고싶다.
        float distance = Vector3.Distance(transform.position, target.transform.position);
        return distance < attackRadius;
    }

    private void UpdateMove()
    {
        // 1. target을 향해 이동하고싶다.
        agent.isStopped = false;
        agent.SetDestination(target.transform.position);

        float distance = Vector3.Distance(transform.position, target.transform.position);
        // 만약 target과의 거리가 공격가능 거리라면 Attack상태로 전이하고 싶다.
        if (distance <= attackRadius)
        {

            SetState(State.Attack, "Attack");
        }
        // 그렇지않고 만약 target이 전투반경을 벗어나면 Idle상태로 전이하고 싶다.
        else if (distance > battleRadius)
        {
            target = null;
            SetState(State.Idle, "Idle");
        }
    }


    /// <summary>
    /// 공격 애니메이션의 끝 프레임에
    /// </summary>
    public void OnAnimationAttackFinished()
    {
        if (target == null)
        {
            return;
        }

        // target이 attackRadius를 벗어났으면
        float distance = Vector3.Distance(transform.position, target.transform.position);
        if (distance > attackRadius)
        {
            // Move상태로 전이하고싶다.

            SetState(State.Move, "Move");
        }
    }

    /// <summary>
    /// Hit는 Enemy가 Player를 때리는 일이다.
    /// 공격 애니메이션 중 타격 프레임에
    /// </summary>
    public void OnAnimationAttackHit()
    {
        if (target == null)
        {
            return;
        }

        // target이 attackRadius안에 있으면
        float distance = Vector3.Distance(transform.position, target.transform.position);
        if (distance <= attackRadius)
        {
            // target(Player)에게 데미지를 입히고싶다.
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
    /// Damage는 Player가 Enemy를 때리는 일이다.
    /// 플레이어가 Enemy를 타격했을때 호출해야한다.
    /// </summary>
    public void SetDamaged(int damage)
    {
        HP -= damage;

    }


    void FaceTarget()
    {
        GameObject target = GameObject.Find("Player");
        Vector3 dir = (target.transform.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(dir.x, 0, dir.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

/*    void hitHighlight()
    {
        blinkTimer -= Time.deltaTime;
        float lerp = Mathf.Clamp01(blinkTimer / blinkDuration);
        float intensity = (lerp * blinkIntensity) + 1.0f;
        skinnedMeshRenderer.material.color = Color.white * intensity;

    }*/


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
