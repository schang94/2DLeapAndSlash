using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public TMP_Text scoreTxt;
    public Canvas gameOverCanvas;
    public Canvas gameOption;
    public Slider sliderSound;
    private AudioSource soundBGM;
    private AudioSource soundPlayer;
    private void OnEnable()
    {
        GameManager.OnScoreAction += ScoreUpdate;
        GameManager.OnDieAction += Die;
        
        
    }

    private void Start()
    {
        sliderSound.onValueChanged.AddListener(soundVolume);
        soundBGM = GetComponent<AudioSource>();
        soundPlayer = GameObject.FindWithTag("Player").transform.GetComponent<AudioSource>();
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
        Time.timeScale = 1f;
    }

    public void ReStartBtn()
    {
        GameManager.Instance.ReStart();
        gameOverCanvas.gameObject.SetActive(false);
    }
    
    public void OpenOption()
    {
        gameOption.gameObject.SetActive(true);
        Time.timeScale = 0f;
    }
    public void BackBtn()
    {
        gameOption.gameObject.SetActive(false);
        Time.timeScale = 1f;
    }

    public void soundVolume(float volume)
    {
        soundBGM.volume = volume;
        soundPlayer.volume = volume;
    }

}
