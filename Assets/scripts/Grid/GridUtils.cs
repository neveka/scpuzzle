using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace SCPuzzle
{
	public class GridUtils  
	{
		protected PathFinder _finder = new PathFinder();
		private List<Vector3> _nearCellsCache = new List<Vector3>(){Vector3.left, Vector3.right, Vector3.forward, Vector3.back};
		private List<Vector3> _aroundCellsCache = new List<Vector3>(){Vector3.left, Vector3.right, Vector3.forward, Vector3.back,
			Vector3.left+Vector3.forward, Vector3.right+Vector3.forward, Vector3.left+Vector3.back, Vector3.right+Vector3.back};	
		private float cellSize = 100;
		private int cellsNumber = 5;

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
			return SnapToGrid(worldPos/cellSize+Vector3.one*(int)(cellsNumber/2));
		}

		public Vector3 GridPosToWorldPos(Vector3 gridPos)
		{
			return (gridPos-Vector3.one*(int)(cellsNumber/2))*cellSize;
		}

		public Vector3 MousePosToScreenPos(Vector3 mousePos)
		{
			mousePos.y = Screen.height - mousePos.y ;
			return mousePos;
		}

		public Vector3 MousePosToWorldPos(Vector3 mousePos, float level, bool forEditor)
		{			
			if (forEditor) 
			{
				mousePos.y = Screen.height - (mousePos.y+30);
			}
			else
			{
				mousePos.y -= 10;
			}
			Camera camera = forEditor ? Camera.current : Camera.main;
			Ray ray = camera.ScreenPointToRay(mousePos);
			float k = (level-ray.origin.y)/ray.direction.normalized.y;
			return ray.origin+k*ray.direction.normalized;
		}
		
		public Vector3 MousePosToGridPos(Vector3 mousePos, float level, bool forEditor)
		{
			Vector3 newPos = MousePosToWorldPos (mousePos, level, forEditor);
			
			//if (forEditor) 
			{
				newPos = WorldPosToGridPos(newPos);
			}
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
