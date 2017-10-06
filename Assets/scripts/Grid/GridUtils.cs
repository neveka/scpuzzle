using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace SCPuzzle
{
	public class GridUtils  
	{
		protected PathFinder _finder = new PathFinder();
		private List<Vector3> _nearCellsCache = new List<Vector3>(){Vector3.left, Vector3.right, Vector3.up, Vector3.down};
		private List<Vector3> _aroundCellsCache = new List<Vector3>(){Vector3.left, Vector3.right, Vector3.up, Vector3.down,
			Vector3.left+Vector3.up, Vector3.right+Vector3.up, Vector3.left+Vector3.down, Vector3.right+Vector3.down};	
		private float cellSize = 100;
		private int cellsNumber = 5;
		private Vector3 centerOffset = new Vector3(2,2,0);

		public bool IsInside(Vector3 pos)
		{
			return pos.x >= 0 && pos.x < cellsNumber && pos.y >= 0 && pos.y < cellsNumber;
		}

		public PathFinder PathFinder
		{
			get { return _finder; }
		}

		public List<Vector3> GetNearCells()
		{
			return _nearCellsCache;
		}

		public List<Vector3> GetAroundCells()
		{
			return _aroundCellsCache;
		}

		public Vector3 SnapToGrid(Vector3 pos)
		{
			return new Vector3(Mathf.Round(pos.x),Mathf.Round(pos.y),Mathf.Round(pos.z));
		}

		public Vector3 WorldPosToGridPos(Vector3 worldPos)//snapped
		{
			return SnapToGrid(worldPos/cellSize+centerOffset);
		}

		public Vector3 GridPosToWorldPos(Vector3 gridPos)
		{
			return (gridPos-centerOffset)*cellSize;
		}

		public Vector3 MousePosToScreenPos(Vector3 mousePos)
		{
			mousePos.y = Screen.height - mousePos.y ;
			return mousePos;
		}

		public Vector3 MousePosToWorldPos(Vector3 mousePos)
		{			
			return new Vector3 (mousePos.x / Screen.width-0.5f, 
				(mousePos.y / Screen.height-0.5f)*Screen.height/Screen.width, 
				0)*cellSize*cellsNumber;
		}
		
		public Vector3 MousePosToGridPos(Vector3 mousePos)
		{
			Vector3 newPos = MousePosToWorldPos (mousePos);
			newPos = WorldPosToGridPos(newPos);
			return newPos;
		}

		/*public GridObject Create(GameObject go)
		{
			GridObject gridObject = go.GetComponent<GridObject>();
			if(gridObject == null)
			{
				gridObject = go.AddComponent<GridObject>();
			}
			return gridObject;
		}

		/*public GridRoot FindGridRoot()
		{
			GridRoot gridRoot = GameObject.FindObjectOfType<GridRoot>();
			if(!gridRoot)
			{
				gridRoot = new GameObject("Grid").AddComponent<GridRoot>();
			}
			return gridRoot;
		}
		
		public Transform FindLayer(string layerName)
		{
			GridRoot gridRoot = FindGridRoot();
			Transform layer = gridRoot.transform.FindChild(layerName);
			if(!layer)
			{
				layer = (new GameObject(layerName)).transform;
				layer.parent = gridRoot.transform;
			}
			return layer;
		}*/
	}
}
