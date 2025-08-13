using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public TMP_Text scoreTxt;
    public Canvas gameOverCanvas;
    private void OnEnable()
    {
        GameManager.OnScoreAction += ScoreUpdate;
        GameManager.OnDieAction += Die;
    }

    private void OnDisable()
    {
        GameManager.OnScoreAction -= ScoreUpdate;
        GameManager.OnDieAction -= Die;
    }

    private void ScoreUpdate(float score)
    {
        // 숫자 표기 설정 (1,000,000)
        scoreTxt.text = ((int)score).ToString("N0", new CultureInfo("ko-KR"));
    }

    private void Die()
    {
        gameOverCanvas.gameObject.SetActive(true);
    }

    public void LobbyBtn()
    {
        SceneManager.LoadScene("SelectScene");
    }

    public void ReStartBtn()
    {
        GameManager.Instance.ReStart();
        gameOverCanvas.gameObject.SetActive(false);
    }
    
    public void OpenOption()
    {

    }

}
