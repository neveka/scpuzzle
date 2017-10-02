using UnityEngine;
using System.Collections;

namespace SCPuzzle
{
	public class MovingPath
	{
		public float speed;
		Vector3[] _points;
		int _idx;

		public MovingPath(Vector3[] points, float speed)
		{
			this.speed = speed;// * (points[0] - points[points.Length-1]).magnitude;
			_points = points;
			_idx = 0;
		}
		
		
		public Vector3 GetTargetPosition()
		{
			return _points[_points.Length-1];
		}

		public bool UpdateMovement(float deltaTime, Vector3 currentPosition, out Vector3 delta)
		{
			Vector3 fromPoint = _points[_idx];
			Vector3 toPoint = _points[_idx+1];
			delta = (toPoint-fromPoint).normalized*speed*deltaTime;
			Vector3 doneDelta = (currentPosition + delta)-fromPoint;
			Vector3 allDelta = toPoint-fromPoint;
			if(doneDelta.sqrMagnitude >= allDelta.sqrMagnitude)
			{
				delta = toPoint - currentPosition;//?
				if(_idx < _points.Length-2)
				{
					_idx++;
				}
				else
					return false;
			}
			return true;
		}	
	}
}