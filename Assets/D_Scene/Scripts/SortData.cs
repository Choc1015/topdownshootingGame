using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static GameManager;

public class SortData : MonoBehaviour
{
    public static SortData instance;
    

    const string rankFILEPATH = "Assets/DataBase/RankingData.csv";


    public List<RankData> rankDatas = CSVReaderRank.ReadRankDataFromCSV(rankFILEPATH);


    public void Awake()
    {
        rankDatas = Rank();
         instance = this;
    }

    

    private void Update()
    {
        rankDatas = Rank();
    }
    List<RankData> Rank()
    {
        List<RankData> result = rankDatas.OrderByDescending(data => data.EndPoint).ThenByDescending(data => data.EndStage).ThenBy(data => data.EndTime).ToList();
        return result;
    }


  


   
    
    
}
