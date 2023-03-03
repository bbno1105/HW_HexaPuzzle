using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticData : MonoBehaviour
{
    void Awake()
    {
        SetStageData();
    }

    [SerializeField] StageSheet stageSheet;
    public static Dictionary<int, StageSheetData> StageData = new Dictionary<int, StageSheetData>();

    public void SetStageData()
    {
        for (int i = 0; i < stageSheet.dataArray.Length; i++)
        {
            StageData.Add(stageSheet.dataArray[i].ID, stageSheet.dataArray[i]);
        }
    }
}
