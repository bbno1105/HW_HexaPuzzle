using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : SingletonBehaviour<UIManager>
{
    [SerializeField] Text missionCountText;
    [SerializeField] Text playCountText;
    
    // Å¬¸®¾î
    [SerializeField] GameObject clearUI;
    [SerializeField] Text clearText;


    void Awake()
    {
        clearUI.SetActive(false);
    }

    public void SetMissionCount(int _count)
    {
        missionCountText.text = _count.ToString();
    }

    public void SetPlayCount(int _count)
    {
        playCountText.text = _count.ToString();
    }

    public void SetClearUI(string _text)
    {
        clearText.text = _text;
        clearUI.SetActive(true);
    }
}
