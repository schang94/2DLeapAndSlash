using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SelectManager : MonoBehaviour
{
    public static SelectManager s_Instance;
    public int character = 0;
    public List<Image> characterList = new List<Image>();
    public TMP_Text stateTxt;
    public List<PlayerData> dataList = new List<PlayerData>();
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
        stateTxt.text = $"Health : {dataList[0].maxHp} / Damage : {dataList[0].damage} / AttackSpeed : {dataList[0].attackSpeed}";
    }


    public void OnSelect(int choice)
    {
        for (int i = 0; i< characterList.Count; i++)
            characterList[i].color = (i == choice) ? Color.red : Color.white;
        character = choice;

        stateTxt.text = $"Health : {dataList[choice].maxHp} / Damage : {dataList[choice].damage} / AttackSpeed : {dataList[choice].attackSpeed}";
    }
    public void OnNextScene()
    {
        SceneManager.LoadScene("BattleScene");
    }


}
