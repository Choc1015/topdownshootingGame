using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;


public class TrashMob : MonoBehaviour
{
    [SerializeField] mobValue mob;

    int monsterSpeedA;// csv
    int monsterSpeedB;// csv
    int bossSpeed;// csv
    float mobBShootingSpeed;
    float bossShootingSpeed;
    private int HP_STRAIGHT; // csv
    private int HP_FOLLOW; // csv
    private int HP_BOSS; // csv
    private int xAxis = -1;
    private GameObject target;

    int mobApoint = 10;
    int mobBpoint = 50;
    int bosspoint = 1000;

    private void Awake()
    {
        monsterSpeedA = GameManager.Instance.mobASpeed;
        monsterSpeedB = GameManager.Instance.mobBSpeed;
        bossSpeed = GameManager.Instance.bossMoveSpeed;
        mobBShootingSpeed = GameManager.Instance.mobBShootingSpeed;
        bossShootingSpeed = GameManager.Instance.bossShootingSpeed;
        HP_STRAIGHT = GameManager.Instance.mobAHp;
        HP_FOLLOW = GameManager.Instance.mobBHp;
        HP_BOSS = GameManager.Instance.bossHp;
    }
    enum mobValue
    {
        돌진형,
        저격형,
        보스
    }
    private void Start()
    {
        target = GameObject.Find("Player");

        switch (mob)
        {
            case mobValue.저격형:
                InvokeRepeating("shooting", 0, mobBShootingSpeed);
                break;
            case mobValue.보스:

                InvokeRepeating("shooting", 0, bossShootingSpeed);
                break;
        }
    }

    private void Update()
    {
        switch (mob)
        {
            
            case mobValue.돌진형:
                Movement(monsterSpeedA);
                break;
            case mobValue.저격형:
                Movement(monsterSpeedB);
                break;
            case mobValue.보스:
                BossMovement();
                break;

        }
    }

    void shooting()
    {
        switch (mob)
        {
            case mobValue.저격형:
                savePosition((int)BulletType.MONSTER);
                break;

            case mobValue.보스:
                savePosition((int)BulletType.BOSS);
                break;

        }

    }

    private void savePosition(int bulletType)
    {
        GameObject bullet = ObjectPoolingManager.instance.GetBullet(bulletType);
        if (bullet == null)
            return;
        BulletMove bulletMove = bullet.GetComponent<BulletMove>();
        bulletMove.SetPosition(transform.position);
        bulletMove.SetTarget(target);
    }

    private void Movement(int mobValueSpeed)
    {
        transform.Translate(Vector2.down * mobValueSpeed * Time.deltaTime);
    }
    private void BossMovement()
    {
        transform.Translate( new Vector2(xAxis,0) * bossSpeed * Time.deltaTime);
       
        if (transform.position.x >= 2.8)
            xAxis = -1;
        else if (transform.position.x <= -2.8)
            xAxis = 1;
       
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("여기임? 콜라이더");
        if (collision.CompareTag("BulletToMonster"))
        {
            switch (mob)
            {
                case mobValue.돌진형:
                    DamageMob(ref HP_STRAIGHT,ref GameManager.Instance.mobACount);
                    
                    break;
                case mobValue.저격형:
                    DamageMob(ref HP_FOLLOW,ref GameManager.Instance.mobBCount);
                    break;
                case mobValue.보스:
                    DamageMob(ref HP_BOSS);
                    break;
            }
        }

        if (collision.CompareTag("Wall"))
        {
            switch (mob)
            {
                case mobValue.돌진형:
                    wallCheck(ref GameManager.Instance.mobACount);
                    break;
                case mobValue.저격형:
                    wallCheck(ref GameManager.Instance.mobBCount);
                    break;
            }
        }



    }

    private void DamageMob(ref int MonsterHP)
    {
        Debug.Log(MonsterHP);
        MonsterHP--;
        if (MonsterHP == 0)
        {
            Debug.Log("뒤짐");
            GameManager.Instance.pointAll += bosspoint;
            GameManager.Instance.STAGE++;
            GameManager.Instance.Initialized();
            HP_BOSS = GameManager.Instance.bossHp;
            GameManager.Instance.isDead = true;
            gameObject.SetActive(false);
        }
    }
    private void DamageMob(ref int MonsterHP,ref int Count)
    {
        Debug.Log(MonsterHP);
        MonsterHP--;
        if (MonsterHP == 0 && mob != mobValue.보스)
        {
            switch (mob)
            {
                case mobValue.돌진형:
                    GameManager.Instance.pointAll += mobApoint;
                    break;
                case mobValue.저격형:
                    GameManager.Instance.pointAll += mobBpoint;
                    break;
            }
            Count--;
            Destroy(gameObject);
        }
        

    }

    void wallCheck(ref int Count)
    {
        Count--;
        Destroy(gameObject);
       
    }
}
