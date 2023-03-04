using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticData : MonoBehaviour
{
    void Awake()
    {
        SetStageData();
    }

    [Tooltip("Sheet 폴더의 데이터파일을 연결시켜주세요.")]
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
