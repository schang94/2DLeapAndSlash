using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SelectManager : MonoBehaviour
{
    public static SelectManager s_Instance;
    public int character = 0;
    public List<Image> characterList = new List<Image>();
    void Awake()
    {
        if (s_Instance == null)
        {
            s_Instance = this;
        }
        else if (s_Instance != this)
        {
            Destroy(gameObject);
        }
    }


    public void OnSelect(int choice)
    {
        for (int i = 0; i< characterList.Count; i++)
            characterList[i].color = (i == choice) ? Color.red : Color.white;
        character = choice;
    }
    public void OnNextScene()
    {
        SceneManager.LoadScene("BattleScene");
    }


}
