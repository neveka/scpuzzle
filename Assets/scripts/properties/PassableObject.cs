using UnityEngine;
using System.Collections;
using System;

namespace SCPuzzle
{
	public class PassableObject: Property
	{
		public bool isObstacle;
		public bool isWorkPlace;
		public bool isTeleportPlace;

		protected Vector3 _prevPos;
		protected int _shootWaveIdx;
		protected int _walkWaveIdx;
		protected MovingObject _user;

		public PassableObject(IGridObject gridObject):base(gridObject)
		{
		}

		public Vector3 PrevPos
		{
			get{ return _prevPos; }
			set{ _prevPos = value; }
		}

		public int ShootWaveIdx
		{
			get{ return _shootWaveIdx; }
			set{ _shootWaveIdx = value; }
		}

		public int WalkWaveIdx
		{
			get{ return _walkWaveIdx; }
			set{ _walkWaveIdx = value; }
		}

		public MovingObject User
		{
			get{ return _user; }
			set{ _user = value; }
		}

		public IGridObject GetObstacle()
		{
			return _gridObject.Grid.FindFromCell<IGridObject> (_gridObject.GridPos + Vector3.up);
		}

		#if UNITY_EDITOR
		public override void OnDrawGizmos(Vector3 pos) 
		{
			if (isWorkPlace) 
			{
				UnityEditor.Handles.Label (pos + Vector3.down * 0.7f, User!=null ? "uw" : "w");
			} 
			else if (isTeleportPlace) 
			{
				UnityEditor.Handles.Label (pos + Vector3.down * 0.7f, User!=null ? "ut" : "t");
			}
		}
#endif
	}
}
