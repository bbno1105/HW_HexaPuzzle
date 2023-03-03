using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchController : SingletonBehaviour<MatchController>
{
    public List<int> CheckStraightMatch(Tile _tile)
    {
        for (int i = 0; i < _tile.TileAround.Length / 2; i++)
        {
            List<int> matchTileID = new List<int>();
            matchTileID.Add(_tile.ID);
            int matchCount = 1;

            matchCount += CheckStraight(_tile, i, ref matchTileID);
            matchCount += CheckStraight(_tile, i + 3, ref matchTileID);

            if (3 <= matchCount) return matchTileID;
        }
        return new List<int>();
    }

    int CheckStraight(Tile _tile, int _aroundIndex, ref List<int> _matchTileID)
    {
        List<Tile> TileList = TileController.Instance.TileList;

        int matchCount = 0;
        if (_tile.TileAround[_aroundIndex] != -1)
        {
            if (_tile.NowBlock && TileList[_tile.TileAround[_aroundIndex]].NowBlock)
            {
                if (_tile.NowBlock.BlockType == TileList[_tile.TileAround[_aroundIndex]].NowBlock.BlockType)
                {
                    matchCount++;
                    _matchTileID.Add(_tile.TileAround[_aroundIndex]);
                    matchCount += CheckStraight(TileList[_tile.TileAround[_aroundIndex]], _aroundIndex, ref _matchTileID);
                }
            }
        }
        return matchCount;
    }
}
