using UnityEngine;
using System.Collections;

///
/// !!! Machine generated code !!!
/// !!! DO NOT CHANGE Tabs to Spaces !!!
/// 
[System.Serializable]
public class StageSheetData
{
  [SerializeField]
  int id;
  public int ID { get {return id; } set { this.id = value;} }
  
  [SerializeField]
  int[] maplist = new int[0];
  public int[] Maplist { get {return maplist; } set { this.maplist = value;} }
  
  [SerializeField]
  int[] createblocktile = new int[0];
  public int[] Createblocktile { get {return createblocktile; } set { this.createblocktile = value;} }
  
  [SerializeField]
  int[] blcoktype = new int[0];
  public int[] Blcoktype { get {return blcoktype; } set { this.blcoktype = value;} }
  
  [SerializeField]
  int[] blocklist = new int[0];
  public int[] Blocklist { get {return blocklist; } set { this.blocklist = value;} }
  
  [SerializeField]
  int[] missiontype = new int[0];
  public int[] Missiontype { get {return missiontype; } set { this.missiontype = value;} }
  
  [SerializeField]
  int[] clearcount = new int[0];
  public int[] Clearcount { get {return clearcount; } set { this.clearcount = value;} }
  
  [SerializeField]
  int movecount;
  public int Movecount { get {return movecount; } set { this.movecount = value;} }
  
}