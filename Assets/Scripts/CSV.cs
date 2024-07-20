using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

[SerializeField]
public class Data
{
    public int STAGE;

    public int mobA_Hp;
    public int mobA_Count;
    public int mobA_Speed;
    public float mobA_SpawnTime;

    public int mobB_Hp;
    public int mobB_Count;
    public int mobB_Speed;
    public int mobB_BulletSpeed;
    public float mobB_SpawnTime;
    public float mobB_ShootingSpeed;
    

    public int boss_Hp;
    public int boss_MoveSpeed;
    public int boss_BulletCycle;
    public int boss_BulletHeight;
    public float boss_ShootingSpeed;
    public int boss_BulletSpeed;

}
public class CSV : MonoBehaviour
{

    [SerializeField] List<Data> Level;

    const string FILEPATH = "Assets/DataBase/Data.csv";

    private void Awake()
    {
        Level = GenerateLevel(100);
        string csvData = ConvertListToCSV(Level);
        SaveCSVWithBOM(FILEPATH, csvData);
        Debug.Log("CSV 파일 생성 위치: " + FILEPATH);

    }

    List<Data> GenerateLevel(int count)
    {
        List<Data> level = new List<Data>();
        for(int i = 0; i < count; i++)
        {
            Data data = new Data
            {
                STAGE = i + 1,

                mobA_Hp = Random.Range(2, 5),
                mobA_Count = Random.Range(1, 11),
                mobA_SpawnTime = Random.Range(1, 4),
                mobA_Speed = Random.Range(5, 10),

                mobB_Hp = Random.Range(2, 4),
                mobB_Count = Random.Range(1, 7),
                mobB_SpawnTime = Random.Range(2, 5),
                mobB_Speed = Random.Range(1, 4),
                mobB_BulletSpeed = Random.Range(2, 6),
                mobB_ShootingSpeed = Random.Range(1, 5),

                boss_Hp = Random.Range(10, 21),
                boss_MoveSpeed = Random.Range(1, 18),
                boss_ShootingSpeed = Random.Range(0.05f, 0.1f),
                boss_BulletCycle = Random.Range(30, 100),
                boss_BulletHeight = Random.Range(1, 6),
                boss_BulletSpeed = Random.Range(1, 6)
            };
            level.Add(data);
        }
        return level;
    }

    string ConvertListToCSV(List<Data> level)
    {
        StringBuilder csv = new StringBuilder();
        csv.AppendLine("STAGE,mobA_Hp,mobA_Count,mobA_SpawnTime,mobA_Speed,mobB_Hp,mobB_Count,mobB_SpawnTime,mobB_Speed,mobB_ShootingSpeed,mobB_BulletSpeed,boss_Hp,boss_MoveSpeed,boss_ShootingSpeed,boss_BulletCycle,boss_BulletHeight,boss_BulletSpeed");
        foreach (var data in level)
        {
            csv.AppendLine($"{data.STAGE},{data.mobA_Hp},{data.mobA_Count},{data.mobA_SpawnTime},{data.mobA_Speed},{data.mobB_Hp},{data.mobB_Count},{data.mobB_SpawnTime},{data.mobB_Speed},{data.mobB_ShootingSpeed},{data.mobB_BulletSpeed},{data.boss_Hp},{data.boss_MoveSpeed},{data.boss_ShootingSpeed},{data.boss_BulletCycle},{data.boss_BulletHeight},{data.boss_BulletSpeed}");
        }
        return csv.ToString();
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


}
public class CSVReader
{
    public static List<Data> ReadPeopleFromCSV(string filePath)
    {
        var level = new List<Data>();
        var lines = File.ReadAllLines(filePath);

        for (int i = 1; i < lines.Length; i++) // 첫 번째 줄은 헤더이므로 건너
        {
            var fields = lines[i].Split(',');
            if (fields.Length == 17)
            {
                var readData = new Data
                {
                    STAGE = int.Parse(fields[0]),
                    mobA_Hp = int.Parse(fields[1]),
                    mobA_Count = int.Parse(fields[2]),
                    mobA_SpawnTime = float.Parse(fields[3]),
                    mobA_Speed = int.Parse(fields[4]),
                    mobB_Hp = int.Parse(fields[5]),
                    mobB_Count = int.Parse(fields[6]),
                    mobB_SpawnTime = float.Parse(fields[7]),
                    mobB_Speed = int.Parse(fields[8]),
                    mobB_ShootingSpeed = float.Parse(fields[9]),
                    mobB_BulletSpeed = int.Parse(fields[10]),
                    boss_Hp = int.Parse(fields[11]),
                    boss_MoveSpeed = int.Parse(fields[12]),
                    boss_ShootingSpeed = float.Parse(fields[13]),
                    boss_BulletCycle = int.Parse(fields[14]),
                    boss_BulletHeight = int.Parse(fields[15]),
                    boss_BulletSpeed = int.Parse(fields[16]),
                };
                level.Add(readData);
            }
        }

        return level;
    }
}
