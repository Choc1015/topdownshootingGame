using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour
{
    public GameObject buttonPrefab;
    public GameObject Content;
    public Text nameIndex;
    public Scrollbar scrollbar;
    public GameObject scoll;

    private List<GameObject> buttonObject = new List<GameObject>();
    private List<RankData> sortData;
    int cons = 0;

    private void Start()
    {
        sortData = SortData.instance.rankDatas;
        Debug.Log(sortData.Count);
        StartCoroutine(OptimizationButtonCoroutine());
    }

    private void Update()
    {
        OnScroll();
    }

    private void OnScroll()
    {
        if (!scoll.activeSelf)
        {
            scrollbar.value = 0;
        }
    }


    IEnumerator OptimizationButtonCoroutine()
    {
        while (true)
        {
            optimizationButtonCount();
            yield return new WaitForSeconds(0.3f); // Adjust the delay as needed
        }
    }
    public void Restart()
    {
        SceneManager.LoadScene(0);
    }

    void optimizationButtonCount()
    {
        if (sortData.Count == 0 || scrollbar.value > 0.01)
            return;

        for (int i = cons; i < 4 + cons && i < sortData.Count; i++)
        {
            if (i >= buttonObject.Count)
            {
                instantiatePrefab(i);
            }
            else
            {
                UpdateButton(i);
            }
        }
        cons += 4;
    }

    void instantiatePrefab(int i)
    {
        GameObject par = Instantiate(buttonPrefab);
        buttonObject.Add(par);
        par.transform.SetParent(Content.transform, false);

        UpdateButtonText(par, i);
    }

    void UpdateButton(int i)
    {
        UpdateButtonText(buttonObject[i], i);
    }

    void UpdateButtonText(GameObject button, int i)
    {
        button.GetComponentInChildren<Text>().text =
            $"이름: {sortData[i].EndName} \n점수: {sortData[i].EndPoint} \n스테이지: {sortData[i].EndStage + 1} \n시간: {sortData[i].EndTime}";
    }
}

