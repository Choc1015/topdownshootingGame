using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class RankData
{
    public int EndStage;
    public int EndPoint;
    public float EndTime;
    public string EndName;
}

public class GameManager : MonoBehaviour
{
    const string FILEPATH = "Assets/DataBase/Data.csv";
    const string rankFILEPATH = "Assets/DataBase/RankingData.csv";
    public static GameManager Instance;
    private List<Data> levelData = CSVReader.ReadPeopleFromCSV(FILEPATH);

    [SerializeField] List<RankData> Rank;

    public int STAGE = 1;

    public int mobAHp;
    public int mobACount;
    public int mobASpeed;
    public float mobASpawnTime;

    public int mobBHp;
    public int mobBCount;
    public int mobBSpeed;
    public int mobBBulletSpeed;
    public float mobBSpawnTime;
    public float mobBShootingSpeed;


    public int bossHp;
    public int bossMoveSpeed;
    public int bossBulletCycle;
    public int bossBulletHeight;
    public float bossShootingSpeed;
    public int bossBulletSpeed;

    public bool isDead;
    public bool isSpawned;
    public bool isShield;

    public int pointAll;
    public float time;
    public string nameRank;
    private void Awake()
    {
        Initialized();
        pointAll = 0;
        time = 0.0f;
        isShield = false;
        isDead = true;
        Instance = this;

    }
    private void Update()
    {
        time += Time.deltaTime;
    }

    List<RankData> InputLevel()
    {
        List<RankData> data = new List<RankData>();

        RankData cash = new RankData
        {
            EndStage = STAGE,
            EndPoint = pointAll,
            EndTime = time,
            EndName = nameRank
        };
        data.Add(cash);
        return data;
    }

    string ConvertListToCSV(List<RankData> people)
    {
        StringBuilder csv = new StringBuilder();
        csv.AppendLine("EndName,EndPoint,EndStage,EndTime");
        foreach (var person in people)
        {
            csv.AppendLine($"{person.EndName},{person.EndPoint},{person.EndStage},{person.EndTime}");
        }
        return csv.ToString();
    }

    public void saveRankData()
    {
        List<RankData> existingData = LoadRankData();  // 기존 데이터를 불러옴
        existingData.AddRange(InputLevel());           // 새로운 데이터를 추가함
        string csv =  ConvertListToCSV(existingData);
        SaveCSVWithBOM(rankFILEPATH, csv);
        
    }

    void SaveCSVWithBOM(string filePath, string csvData)
    {
        byte[] bom = new byte[] { 0xEF, 0xBB, 0xBF };
        byte[] csvBytes = Encoding.UTF8.GetBytes(csvData);
        byte[] csvWithBom = new byte[bom.Length + csvBytes.Length];
        System.Buffer.BlockCopy(bom, 0, csvWithBom, 0, bom.Length);
        System.Buffer.BlockCopy(csvBytes, 0, csvWithBom, bom.Length, csvBytes.Length);
        File.WriteAllBytes(filePath, csvWithBom);
    }
    List<RankData> LoadRankData()
    {
        List<RankData> data = new List<RankData>();

        if (File.Exists(rankFILEPATH))
        {
            string[] lines = File.ReadAllLines(rankFILEPATH);
            for (int i = 1; i < lines.Length; i++)  // 첫 번째 줄은 헤더이므로 건너뜀
            {
                string[] values = lines[i].Split(',');
                RankData rankData = new RankData
                {
                    EndName = values[0],
                    EndPoint = int.Parse(values[1]),
                    EndStage = int.Parse(values[2]),
                    EndTime = float.Parse(values[3])
                };
                data.Add(rankData);
            }
        }

        return data;
    }
    public void Initialized()
    {
        mobAHp = levelData[STAGE].mobA_Hp;
        mobACount = levelData[STAGE].mobA_Count;
        mobASpeed = levelData[STAGE].mobA_Speed;
        mobASpawnTime = levelData[STAGE].mobA_SpawnTime;

        mobBHp = levelData[STAGE].mobB_Hp;
        mobBCount = levelData[STAGE].mobB_Count;
        mobBSpeed = levelData[STAGE].mobB_Speed;
        mobBBulletSpeed = levelData[STAGE].mobB_BulletSpeed;
        mobBSpawnTime = levelData[STAGE].mobB_SpawnTime;
        mobBShootingSpeed = levelData[STAGE].mobB_ShootingSpeed;

        bossHp = levelData[STAGE].boss_Hp;
        bossMoveSpeed = levelData[STAGE].boss_MoveSpeed;
        bossBulletCycle = levelData[STAGE].boss_BulletCycle;
        bossBulletHeight = levelData[STAGE].boss_BulletHeight;
        bossShootingSpeed = levelData[STAGE].boss_ShootingSpeed;
        bossBulletSpeed = levelData[STAGE].boss_BulletSpeed;
    }

    public class CSVReaderRank
    {
        public static List<RankData> ReadRankDataFromCSV(string filePath)
        {
            var rankData = new List<RankData>();
            var lines = File.ReadAllLines(filePath);
            for (int i = 1; i < lines.Length; i++)
            {
                var fields = lines[i].Split(",");
                if (fields.Length == 4)
                {
                    var readData = new RankData
                    {
                        EndName = fields[0],
                        EndPoint = int.Parse(fields[1]),
                        EndStage = int.Parse(fields[2]),
                        EndTime = float.Parse(fields[3]),
                    };
                    rankData.Add(readData);
                }
            }
            return rankData;
        }
    }
}
