using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using static UnityEngine.GraphicsBuffer;

public class BulletMove : MonoBehaviour
{
    [SerializeField] int bulletSpeed = 1;
    [SerializeField] GameObject Player;
    [SerializeField] GameObject SnipingMonster;
    [SerializeField] stateBullet state;

    float maxNum = 360;
    float currentNum = 0; 

    private int mobBBulletSpeed;
    private int bosBulletSpeed;
    private int bossBulletCycle;
    private int bossBulletHeight;

    enum stateBullet
    {
        직선,
        유도,
        보스
    }

    private void Awake()
    {
        InitialBullet();

    }
    
    

    

    private void Update()
    {
        InitialBullet();
        WhatStateBullet();
    }

    private void OnDisable()
    {
        
        currentNum = 0;
    }
    private void InitialBullet()
    {
        mobBBulletSpeed = GameManager.Instance.mobBBulletSpeed;
        bosBulletSpeed = GameManager.Instance.bossBulletSpeed;
        bossBulletCycle = GameManager.Instance.bossBulletCycle;
        bossBulletHeight = GameManager.Instance.bossBulletHeight;
    }
    private void WhatStateBullet()
    {
        switch (state)
        {
            case stateBullet.직선:
                StraightBullet();
                break;
            case stateBullet.유도:
                TargetToPlayerBullet();
                break;
            case stateBullet.보스:
                BossBullet();
                break;
        }
    }

    private void BossBullet()
    {
        currentNum += bosBulletSpeed * bossBulletCycle * Time.deltaTime;
        float turnNum = currentNum / maxNum;

        Vector2 cosVec = new Vector2(bossBulletHeight * Mathf.Cos(Mathf.PI * 2 * turnNum), -1);
        transform.Translate(cosVec * Time.deltaTime * bosBulletSpeed);
    }

    private void TargetToPlayerBullet()
    {
        Quaternion moveToTrargetRotation = Quaternion.LookRotation(
            Player.transform.position - transform.position,
            transform.TransformDirection(Vector3.back));

        transform.rotation = new Quaternion(0, 0, moveToTrargetRotation.z, moveToTrargetRotation.w);
        transform.position = Vector3.MoveTowards(transform.position, Player.transform.position, mobBBulletSpeed * Time.deltaTime);
    }

    private void StraightBullet()
    {
        transform.Translate(Vector2.up * bulletSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        FalseActiveToTag(collision,"Wall");
        switch (state)
        {
            case stateBullet.직선:
                FalseActiveToTag(collision, "Monster");
                break;
            case stateBullet.유도:
                FalseActiveToTag(collision, "noSniping");
                FalseActiveToTag(collision, "Player");
                break;
            case stateBullet.보스:
                FalseActiveToTag(collision, "Player");
                break;
        }
    }

    private void FalseActiveToTag(Collider2D collision, string Tag)
    {
        if (collision.transform.CompareTag(Tag))
        {
            gameObject.SetActive(false);
        }
    }

    public void SetPosition(Vector3 position)
    {
        transform.position = position;
    } 

    public void SetTarget(GameObject target)
    {
        if (state != stateBullet.유도)
            return;
        Player = target;
    }
   

}
