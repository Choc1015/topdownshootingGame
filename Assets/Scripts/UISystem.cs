using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UISystem : MonoBehaviour
{
    public Text stageText;
    public Text poingText;
    public Text timeText;

    public InputField inFied = null;

    // Update is called once per frame
    void Update()
    {
        stageText.text = (GameManager.Instance.STAGE + 1).ToString() + " STAGE";
        poingText.text = GameManager.Instance.pointAll.ToString();
        timeText.text = GameManager.Instance.time.ToString("F1");
    }

    public void cancleRestart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }

    public void rankRestart()
    {
        if (string.IsNullOrEmpty(inFied.text))
            return;

        GameManager.Instance.nameRank = inFied.text;
        GameManager.Instance.name = inFied.text;

        GameManager.Instance.saveRankData();

        Time.timeScale = 1;
        SceneManager.LoadScene(1);
    }
}
