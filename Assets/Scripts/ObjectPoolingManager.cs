using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BulletType
{
    PLAYER,
    MONSTER,
    BOSS
}


public class ObjectPoolingManager : MonoBehaviour
{
    public static ObjectPoolingManager instance;
    [SerializeField] GameObject[] poolGameObject = default;
    public int[] poolCount;

    private List<GameObject> _playerBullet = new List<GameObject>();
    private List<GameObject> _monsterBullet = new List<GameObject>();
    private List<GameObject> _bossBullet = new List<GameObject>();

    private void Start()
    {
        instance = this;
        Inisialize();
    }
    
    private void Inisialize()
    {
        _playerBullet = new List<GameObject>();
        _monsterBullet = new List<GameObject>();
        _bossBullet = new List<GameObject>();
        MakeBullet((int)BulletType.PLAYER, _playerBullet);
        MakeBullet((int)BulletType.MONSTER, _monsterBullet);

        MakeBullet((int)BulletType.BOSS, _bossBullet);
    }

    private void MakeBullet(int BulletType, List<GameObject> BulletList)
    {
        
        for (int i = 0; i < poolCount[BulletType]; i++)
        {
            GameObject gameOb = Instantiate(poolGameObject[BulletType]);
            gameOb.transform.parent = transform;
            gameOb.SetActive(false);
            BulletList.Add(gameOb);
        }
    }

    public GameObject GetBullet(int _BulletType)
    {
        List<GameObject> BulletList = new List<GameObject>();

        if (_BulletType == (int)BulletType.PLAYER)
            BulletList = _playerBullet;
        else if (_BulletType == (int)BulletType.MONSTER)
            BulletList = _monsterBullet;
        else if (_BulletType == (int)BulletType.BOSS)
            BulletList = _bossBullet;

        
        if(GameManager.Instance.isSpawned != true && _BulletType == (int)BulletType.BOSS && GameManager.Instance.isDead != false)
            return null;
        foreach (GameObject obj in BulletList)
        {
            if(obj.activeSelf == false)
            {
                obj.SetActive(true);
                return obj;
            }
        }

        GameObject var = Instantiate(poolGameObject[_BulletType]);
        var.transform.parent = transform;
        var.SetActive(true);
        BulletList.Add(var);
        
        return var;
    }
}
