using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateMonster : MonoBehaviour
{
    const float _X = 5;
    const float _Y = 10.5f;

    [SerializeField] private GameObject[] _monster;
    [SerializeField] private GameObject _boss;
    [SerializeField] private int _monsterCountA = 2;
    [SerializeField] private int _monsterCountB = 2;
    [SerializeField] private int _monsterCountALL;

    private void Awake()
    {
        _monsterCountALL = 0;
    }

    private void Start()
    {
        _monsterCountA = GameManager.Instance.mobACount;
        _monsterCountB = GameManager.Instance.mobBCount;
        _monsterCountALL = _monsterCountA + _monsterCountB;
        GameManager.Instance.isSpawned = true;
        StartCoroutine(spawnPattern());
    }

    private void Update()
    {
        _monsterCountA = GameManager.Instance.mobACount;
        _monsterCountB = GameManager.Instance.mobBCount;
        _monsterCountALL = _monsterCountA + _monsterCountB;
        Debug.Log(_monsterCountALL);

        if(_monsterCountALL<= 0)
            _monsterCountALL = 0;

        if (_monsterCountALL == 0 && GameManager.Instance.isDead == true)
        {
           
            _boss.SetActive(true); 
           
            GameManager.Instance.isDead = false;
        }
        
        if(GameManager.Instance.isDead == false && _monsterCountALL >= 0 )
        {
            GameManager.Instance.isSpawned = true;
        }

        Debug.Log(GameManager.Instance.isSpawned);
    }

    IEnumerator spawnPattern()
    {
       while(true) 
        {

            yield return new WaitForSeconds(1f);
            if (GameManager.Instance.isSpawned == true && GameManager.Instance.isDead == true && _monsterCountALL > 0 )
            {
                StartCoroutine(SpawnRndPosition(0, _monsterCountA, GameManager.Instance.mobASpawnTime)); // 나중에 csv.Count 가져올 예정
                StartCoroutine(SpawnRndPosition(1, _monsterCountB, GameManager.Instance.mobBSpawnTime));
                GameManager.Instance.isSpawned = false;
            }
        }

           
      
    }

    IEnumerator SpawnRndPosition(int monsterValue,int monsterCount ,float spawnTime)
    {
        for (int i = 0; i < monsterCount; i++)
        {
            float rndX = Random.Range(-_X, _X);
            Vector2 rndPosition = new Vector2(rndX, _Y);
            Instantiate(_monster[monsterValue], rndPosition, Quaternion.identity);
            yield return new WaitForSeconds(spawnTime);
        }
    }
}
