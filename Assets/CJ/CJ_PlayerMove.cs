using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class CJ_PlayerMove : MonoBehaviour
{


    public Animator anim;
    public static CJ_PlayerMove instance;



    private void Awake()
    {
        CJ_PlayerMove.instance = this;
    }

    internal void OnAttack()
    {
        //SetState(State.Idle);
        state = State.Move;
        anim.SetTrigger("Idle");
    }

    internal void OnWalk()
    {
    }

    internal void OnRun()
    {
    }

    internal void OnIdle()
    {
    }

    internal void OnJump()
    {
        state = State.Move;
        anim.SetTrigger("Idle");
    }

    internal void OnHit()
    {
        state = State.Move;
        anim.SetTrigger("Idle");
    }

    public int pm
    {
        get
        {
            return pm;
        }
        set
        {
            pm = value;
        }
    }


    public float speed = 5;
    public float jumpPower = 10;
    Rigidbody rd;
    public float jumpCount = 0;

    public int skillattack = 50;
    public int skillmp = 50;
    public int tskillmp = 1;

    CJ_PlayerHP playerhp;
    CJ_PlayerMP playermp;

    public CapsuleCollider skillcollder;

    public AudioSource skillSource;
    public AudioSource attackSource;
    public AudioSource jumpSource;
    public AudioSource potionSource;
    public AudioSource keySource;
    public AudioSource spinSource;


    // Start is called before the first frame update
    void Start()
    {
        playermp = GetComponent<CJ_PlayerMP>();
        playerhp = GetComponent<CJ_PlayerHP>();
        rd = gameObject.GetComponent<Rigidbody>();

        skillcollder.enabled = false;
        state = State.Move;

        skillSource.enabled = false;
        attackSource.enabled = false;
        jumpSource.enabled = false;
        potionSource.enabled = false;
        keySource.enabled = false;
        spinSource.enabled = false;


        onekey.SetActive(false);
        towkey.SetActive(false);
    }



    public float gravity = -25;
    float h, v;
    // Update is called once per frame
    void Update()
    {
        Physics.gravity = Vector3.up * gravity;

        switch (state)
        {
            case State.Move:
                UpdateMove();
                UpdateJump();
                break;
            case State.Jump:
                UpdateJump();
                UpdateMove();
                break;
            case State.Die:
                DoDie();
                break;
        }
    }

    private void UpdateJump()
    {
        if (jumpCount < 2 && Input.GetButtonDown("Jump"))
        {
            Vector3 dir = new Vector3(-v, 0, h);
            dir.Normalize();
            rd.velocity = Vector3.zero;
            rd.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
            jumpCount++;
            if (state != State.Jump)
            {

                jumpSource.enabled = true;
                Invoke("JumpSource", 0.3f);
                state = State.Jump;
            }
            jumpSource.enabled = true;
            anim.SetTrigger("Jump");
            Invoke("JumpSource", 0.3f);
        }

        if (Input.GetKeyDown(KeyCode.C) && jumpCount == 2 && playermp.MP >= skillmp)
        {
            playermp.MP -= skillmp;
            skill = true;
            gravity = -200;

            Invoke("shake", 0.3f);
            skillcollder.enabled = true;
            Invoke("skillone", 0.5f);

            skillSource.enabled = true;
            Invoke("sourceskill", 1.5f);
        }

    }
    void sourceskill()
    {
        skillSource.enabled = false;
    }
    void skillone()
    {
        skillcollder.enabled = false;
    }
    void shake()
    {
        CJ_CameraMove.instance.DoShake(0.7f);
    }

    public float attackTime = 1;
    float currentTime;
    bool move;

    private void UpdateMove()
    {
        if (state != State.Die)
        {
            h = Input.GetAxis("Horizontal");
            v = Input.GetAxis("Vertical");
            Vector3 dir = new Vector3(-v, 0, h);
            dir.Normalize();

            transform.LookAt(transform.position + dir);

            if (Input.GetKey(KeyCode.LeftShift))
            {
                rd.MovePosition(transform.position + dir * (speed * 2) * Time.deltaTime);
            }
            else
            {
                rd.MovePosition(transform.position + dir * speed * Time.deltaTime);
            }

            bool isKeyRelease = h == 0 && v == 0;
            if (isKeyRelease)
            {
                anim.SetTrigger("Idle");
            }
            else if (Input.GetKey(KeyCode.LeftShift))
            {
                anim.SetTrigger("Run");
            }
            else
            {
                anim.SetTrigger("Walk");
            }

            if (Input.GetKeyDown(KeyCode.X))
            {
                attack = true;
                state = State.Attack;
                anim.SetTrigger("Attack");
                attackSource.enabled = true;
                Invoke("attacksource", 0.4f);
            }

            // z를 누르면  1초에한번씩 마나가 5 깍이고 일반공격력 대미지을 돌면서 준다

            //if (Input.GetKey(KeyCode.Z) && playermp.MP >= 0)
            if (Input.GetKey(KeyCode.Z))
            {
                if (playermp.MP > 0)
                {
                    currentTime += Time.deltaTime;

                    attack = true;
                    transform.Rotate(transform.up * 1000 * Time.deltaTime);
                    if (attack == true)
                    {

                        if (currentTime > 1)
                        {
                            spinSource.enabled = true;
                            AudioSource[] arr = transform.GetComponents<AudioSource>();
                            arr[arr.Length - 1].Play();
                            playermp.MP -= tskillmp;
                            currentTime = 0;
                        }

                    }
                    if (Input.GetKeyUp(KeyCode.Z))
                    {
                        AudioSource[] arr = transform.GetComponents<AudioSource>();
                        arr[arr.Length - 1].Stop();
                        //spinSource.enabled = false;
                    }
                }

                else if (playermp.MP <= 0) {
                    return;
                }
            }

        }
    }

    void JumpSource() {
        jumpSource.enabled = false;
    }

    void PotionSource() {
        potionSource.enabled = false;
    }

    void attacksource()
    {
        attackSource.enabled = false;
    }

    public void DoDie()
    {
        if (state != State.Die)
        {
            CJ_GameManager.instance.GameOverUI.SetActive(true);
            state = State.Die;
            anim.SetTrigger("Die");
            Time.timeScale = 0.2f;
            Camera.main.fieldOfView = zoomIn;
        }
    }

    public enum State
    {
        Move,
        Attack,
        Jump,
        Skill,
        Die,
        Hit
    }
    public State state;

    bool attack;
    bool skill;
    public float zoomIn = 35;
    public float zoomOut = 60;

/*    private void OnTriggerEnter(Collider other)
    {
        if (skillcollder == true)
        {
            if (skill == true)
            {
                if (other.gameObject.CompareTag("Enemy"))
                {
                    Enemy enemy = other.gameObject.GetComponent<Enemy>();
                    enemy.HP -= skillattack;
                }
            }

        }

    }*/

    private void OnTriggerEnter(Collider other)
    {
        if (skillcollder == true && other.gameObject.CompareTag("Enemy"))
        {
            Enemy enemy = other.gameObject.GetComponent<Enemy>();
            if (skill == true)
            {
                if (enemy != null)
                {
                    enemy.HP -= skillattack;
                }
                else
                {
                    Boss boss = other.gameObject.GetComponent<Boss>();
                    if (boss != null)
                    {
                        boss.HP -= skillattack;
                    }
                }
            }
        }

    }

    void ZoomOut()
    {
        Camera.main.fieldOfView = zoomOut;
    }




    enum Items
    {
        hp,
        mp,
        yellowKey,
        greenKey,

    }

    public bool yellowKey, greenKey;
    public Image oneKey, towKey;
    public GameObject onekey, towkey, oonekey, ttowkey;

    private void OnCollisionEnter(Collision other)
    {
        if (attack == true && other.gameObject.CompareTag("Enemy"))
        {
            Enemy enemy = other.gameObject.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.SetDamaged(CJ_wp.instance.damage);
            }
            else
            {
                Boss boss = other.gameObject.GetComponent<Boss>();
                if (boss != null)
                {
                    boss.SetDamaged(CJ_wp.instance.damage);
                }
            }
            Camera.main.fieldOfView = zoomIn;
            Invoke("ZoomOut", 1.5f);

        }

        jumpCount = 0;
        gravity = -25f;
        skill = false;
        attack = false;

        if (other.gameObject.CompareTag("Item_HP"))
        {
            AddItem(Items.hp);
            potionSource.enabled = true;
            Invoke("PotionSource", 0.5f);
            Destroy(other.gameObject);
        }

        else if (other.gameObject.CompareTag("Item_MP"))
        {
            AddItem(Items.mp);
            potionSource.enabled = true;
            Invoke("PotionSource", 0.5f);
            Destroy(other.gameObject);
        }

        else if (other.gameObject.CompareTag("Item_YellowKey"))
        {
            AddItem(Items.yellowKey);
            keySource.enabled = true;
            Destroy(other.gameObject);
        }

        else if (other.gameObject.CompareTag("Item_GreenKey"))
        {
            AddItem(Items.greenKey);
            keySource.enabled = true;
            Destroy(other.gameObject);
        }
    }

    

    void AddItem(Items item)
    {
        if (item == Items.hp)
        {
            playerhp.HP += 50;
        }

        if (item == Items.mp)
        {
            playermp.MP += 50;
        }

        if (item == Items.yellowKey)
        {
            yellowKey = true;
            onekey.SetActive(true);
            oonekey.SetActive(false);
        }
        if (item == Items.greenKey)
        {
            greenKey = true;
            towkey.SetActive(true);
            ttowkey.SetActive(false);
        }

    }


}





