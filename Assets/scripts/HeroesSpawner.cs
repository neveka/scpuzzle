using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace SCPuzzle
{
	public class HeroesSpawner
	{
		private IGrid _grid;
		private CoroutineStarter _coroutineStarter;

		private Canvas _canvas;
		private GameObject _allyGameObjectTemplate;
		private GameObject _blockGameObjectTemplate;
		private GameObject _enemyGameObjectTemplate;
		private GameObject _effectGameObjectTemplate;

		public HeroesSpawner(IGrid grid, CoroutineStarter coroutineStarter)
		{
			_grid = grid;
			_coroutineStarter = coroutineStarter;

			_canvas = GameObject.FindObjectOfType<Canvas> ();
			_allyGameObjectTemplate = Resources.Load<GameObject> ("Ally");
			_blockGameObjectTemplate = Resources.Load<GameObject> ("Block");
			_enemyGameObjectTemplate = Resources.Load<GameObject> ("Enemy");
			_effectGameObjectTemplate = Resources.Load<GameObject> ("Effect");

			Spawn ();
		}

		public void Spawn()
		{

			for (int i = 0; i < 25; i++) 
			{
				bool ally = i == 10 || i == 2 || i == 18;
				bool block = i == 4 || i == 13 || i == 22;
				
				GridObject gridObject = Spawn (new Vector3 (i % 5, i / 5, 0), ally?_allyGameObjectTemplate:(block?_blockGameObjectTemplate:_enemyGameObjectTemplate));

				if (ally) 
				{
					AllyObject.AttackType attackType = i == 10 ? AllyObject.AttackType.round : (i == 2 ? AllyObject.AttackType.line : AllyObject.AttackType.three_random);
					gridObject.AddProperty (new AllyObject (gridObject, attackType, (Vector3 pos)=>ShowDamageEffect(pos)));
					gridObject.AddProperty (new SelectableWithArrows (gridObject));
				}
				else if (!block)
					gridObject.AddProperty (new EnemyObject (gridObject));
			}
		}

		GridObject Spawn(Vector3 gridPos, GameObject template)
		{
			GameObject gameObject = GameObject.Instantiate (template);

			gameObject.transform.SetParent(_canvas.transform);
			gameObject.transform.localScale = Vector3.one;

			GridObject gridObject = new GridObject (gameObject, _grid, OnDestroyGridObject);
			gridObject.GridPos = gridPos;
			//gridObject.AddProperty (new SelectableWithImageColor (heroGridObject));
			gridObject.AddProperty (new MovingObject (gridObject, _coroutineStarter));
			return gridObject;
		}

		void ShowDamageEffect(Vector3 gridPos)
		{
			GameObject effect = GameObject.Instantiate (_effectGameObjectTemplate);
			effect.transform.SetParent(_canvas.transform);
			effect.transform.localPosition = _grid.Utils.GridPosToWorldPos(gridPos);
			_coroutineStarter.StartCoroutine (WaitAndHide (effect));
		}

		IEnumerator WaitAndHide(GameObject effect)
		{
			yield return new WaitForSeconds(0.3f);
			GameObject.DestroyImmediate (effect);
		}

		void OnDestroyGridObject(Vector3 gridPos)
		{
			Vector3 spawnPos = gridPos;
			spawnPos.y = 5;
			while (_grid.GetFromCell (spawnPos) != null) 
			{
				spawnPos += Vector3.up;
			}
			GridObject enemy = Spawn (spawnPos, _enemyGameObjectTemplate);
			enemy.AddProperty( new EnemyObject(enemy));
			_coroutineStarter.StartCoroutine (waitAndMove(gridPos));
		}

		IEnumerator waitAndMove(Vector3 gridPos)
		{
			yield return new WaitForEndOfFrame ();
			Vector3 currentPos = gridPos;
			Vector3 dir = Vector3.up;
			while (currentPos.y<=10) 
			{
				currentPos += dir;
				List<IGridObject> objs = _grid.GetAllFromCell (currentPos);
				if(objs != null)
					foreach (IGridObject obj in objs) {
						MovingObject m = obj.GetProperty<MovingObject>();
						if (m != null)
							_coroutineStarter.StartCoroutine (waitAndGoDown (m));
					}
			}
		}

		IEnumerator waitAndGoDown(MovingObject m)
		{
			while(m.IsMoving())
				yield return null;
			m.StartMovingTo (m.GridObject.GridPos + Vector3.down, null);
		}
	}
}
