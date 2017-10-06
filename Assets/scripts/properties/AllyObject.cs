using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace SCPuzzle
{
	public class AllyObject : Property 
	{
		public enum AttackType
		{
			round,
			line,
			three_random
		}

		private AttackType _attackType;
		private float _attackForse = 0.25f;
		private Vector3 _gridPosOnStartMove;
		private Action<Vector3> _showEffect;

		public AllyObject(IGridObject gridObject, AttackType attackType, Action<Vector3> showEffect):base(gridObject)
		{
			_attackType = attackType;
			_showEffect = showEffect;
		}

		public void OnMoveStarted()
		{
			_gridPosOnStartMove = _gridObject.GridPos;
		}

		public void OnMoveFinished(IGridObject opponent)
		{
			_gridObject.Grid.GetAll<EnemyObject> ().ForEach(e=>e.Initiative--);
			//1 атакует после свайпа всех вокруг себя.
			//2 атакует по линии в направлении места, где он был до свайпа.
			//3 после свайпа атакует 3 случайных противников на поле.
			if (opponent.GetProperty<EnemyObject> () != null) 
			{
				switch (_attackType) {
				case AttackType.round:
					List<Vector3> cells = _gridObject.Grid.Utils.GetAroundCells();
					foreach(Vector3 cell in cells)
					{
						EnemyObject e = _gridObject.Grid.GetFromCell<EnemyObject> (cell + _gridObject.GridPos);
						_showEffect(cell + _gridObject.GridPos);
						if (e != null)
							e.Health -= _attackForse;
					}
					break;
				case AttackType.line:
					Vector3 dir = _gridPosOnStartMove - _gridObject.GridPos;
					Vector3 currentPos = _gridObject.GridPos;
					while(_gridObject.Grid.Utils.IsInside(currentPos))
					{
						currentPos += dir;
						EnemyObject e = _gridObject.Grid.GetFromCell<EnemyObject> (currentPos);
						_showEffect(currentPos);
						if (e != null)
							e.Health -= _attackForse;
					}
					break;
				case AttackType.three_random:
					List<EnemyObject> enemies = _gridObject.Grid.GetAll<EnemyObject> ();
					for (int i = 0; i < 3; i++) 
					{
						EnemyObject e = enemies [UnityEngine.Random.Range (0, enemies.Count)];
						_showEffect(e.GridObject.GridPos);
						e.Health -= _attackForse;
					}
					break;
				}

			}
		}
	}
}