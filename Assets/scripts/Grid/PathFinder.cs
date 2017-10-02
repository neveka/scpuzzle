using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace SCPuzzle
{
	public class PathFinder
	{
		List<Vector3> _points = new List<Vector3>();

		public MovingPath FindPath(IGridObject goThis, Vector3 endPoint, float speed)
		{
			if (Find (goThis, endPoint, ref speed)) 
			{
				return new MovingPath (_points.ToArray(), speed);
			}
			return null;
		}

		bool Find(IGridObject goThis, Vector3 endPoint, ref float speed)
		{
			_points.Clear();
			IGrid grid = goThis.Grid;
			_points.Insert (0, endPoint);
			while(endPoint != goThis.GridPos)
			{
				PassableObject endObject = grid.GetFromCell<PassableObject>(endPoint+Vector3.down);
				endPoint = endObject.PrevPos+Vector3.up;
				if(_points[0].y != endPoint.y)
				{
					Vector3 midPoint = _points[0].y<endPoint.y?_points[0]:endPoint;
					midPoint.y = _points[0].y<endPoint.y?endPoint.y:_points[0].y;
					_points.Insert(0, midPoint);
				}
				_points.Insert(0, endPoint);
			}
			
			return true;
		}

		public void GetReachableCells(PassableObject cell, int waveLength, bool byAir, Func<PassableObject, bool> filterCell)
		{
			if(waveLength == 0)
				return;
			IGrid grid = cell.GridObject.Grid;
			Dictionary<Vector3, PassableObject> cells = new Dictionary<Vector3, PassableObject>(new PosComparer());
			Dictionary<Vector3, PassableObject> justVisitedCells = new Dictionary<Vector3, PassableObject>(new PosComparer());
			cells.Add(cell.GridObject.WorldPos, cell);
			if(byAir)
			{
				cell.ShootWaveIdx = 0;
			}
			else
			{
				cell.WalkWaveIdx = 0;
			}
			if(filterCell(cell))
				return;
			for(int i=0; i<waveLength; i++)
			{
				if(GetReachableCells(grid, i+1, byAir, cells, justVisitedCells, filterCell))
					break;
			}
		}
		
		
		public bool GetReachableCells(IGrid grid, int waveIdx, bool byAir, Dictionary<Vector3, PassableObject> cells, Dictionary<Vector3, PassableObject> justVisitedCells, Func<PassableObject, bool> filterCell)//optimize it!
		{
			bool reached = false;
			List<Vector3> nears = grid.Utils.GetNearCells();
			Dictionary<Vector3, PassableObject> newCells = new Dictionary<Vector3, PassableObject>(new PosComparer());
			foreach(KeyValuePair<Vector3, PassableObject> pair in cells)
			{
				if(pair.Value.WalkWaveIdx < waveIdx-1)
					continue;
				Vector3 gridPos = pair.Key;
				nears.ForEach(n=>{
					Vector3 pos = n+gridPos;
					if(cells.ContainsKey(pos) || newCells.ContainsKey(pos) || justVisitedCells.ContainsKey(pos))
						return;
					PassableObject cellForwardUnder = grid.GetFromCell<PassableObject>(pos);
					if(cellForwardUnder==null)
					{
						justVisitedCells.Add(pos, null);
						return;
					}
					if(byAir)
					{
						newCells.Add(pos, cellForwardUnder);
						cellForwardUnder.ShootWaveIdx = waveIdx;
					}
					if(!byAir)
					{
						if(!cellForwardUnder.isObstacle && !cellForwardUnder.isWorkPlace)
						{
							newCells.Add(pos, cellForwardUnder);
						}
						else
						{
							justVisitedCells.Add(pos, cellForwardUnder);
						}
						cellForwardUnder.PrevPos = gridPos;
						cellForwardUnder.WalkWaveIdx = waveIdx;
					}
					if(filterCell(cellForwardUnder))
						reached = true;
				});
			}
			foreach(KeyValuePair<Vector3, PassableObject> pair in newCells)
			{
				cells.Add(pair.Key, pair.Value);
			}
			//cells.AddRange(newCells);
			return reached;
		}
	}
}