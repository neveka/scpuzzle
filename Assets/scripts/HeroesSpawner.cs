using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace SCPuzzle
{
	public class HeroesSpawner
	{
		private IGrid _grid;
		private CoroutineStarter _coroutineStarter;

		private Canvas _canvas;
		private Dictionary<string, GameObject> _gameObjects = new Dictionary<string, GameObject> ();
		private Dictionary<string, Sprite> _sprites = new Dictionary<string, Sprite> ();

		public HeroesSpawner(IGrid grid, CoroutineStarter coroutineStarter)
		{
			_grid = grid;
			_coroutineStarter = coroutineStarter;

			_canvas = GameObject.FindObjectOfType<Canvas> ();
			_gameObjects["Ally"] = Resources.Load<GameObject> ("Ally");
			_gameObjects["Block"] = Resources.Load<GameObject> ("Block");
			_gameObjects["Enemy"] = Resources.Load<GameObject> ("Enemy");
			_gameObjects["Effect"] = Resources.Load<GameObject> ("Effect");
			_sprites ["annunaki"] = Resources.Load<Sprite> ("Heroes/annunaki");
			_sprites ["hierarchy"] = Resources.Load<Sprite> ("Heroes/hierarchy");
			_sprites ["terran"] = Resources.Load<Sprite> ("Heroes/terran");
			_sprites ["commander-bot-2"] = Resources.Load<Sprite> ("Heroes/commander-bot-2");
			_sprites ["sp01_mercenary"] = Resources.Load<Sprite> ("Heroes/sp01_mercenary");
			Spawn ();
		}

		public void Spawn()
		{
			for (int i = 0; i < 25; i++) 
			{
				bool ally = i == 10 || i == 2 || i == 18;
				bool block = i == 4 || i == 13 || i == 22;

				string prefabName = ally ? "Ally" : (block ? "Block" : "Enemy");
				
				GridObject gridObject = Spawn (new Vector3 (i % 5, i / 5, 0), _gameObjects[prefabName]);

				if (ally) {
					AllyObject.AttackType attackType = i == 10 ? AllyObject.AttackType.round : (i == 2 ? AllyObject.AttackType.line : AllyObject.AttackType.three_random);
					string spriteName = i == 10 ? "annunaki" : (i == 2 ? "hierarchy" : "terran");

					gridObject.AddProperty (new AllyObject (gridObject, attackType, (Vector3 pos) => ShowDamageEnemyEffect (pos)));
					gridObject.AddProperty (new SelectableWithArrows (gridObject));
					gridObject.AddProperty (new ImageChanger (gridObject, _sprites [spriteName]));
				} 
				else if (!block) 
				{
					int idx = UnityEngine.Random.Range (0, 2);
						
					gridObject.AddProperty (new EnemyObject (gridObject, idx==0?3:4, (Vector3 pos) => ShowDamagePlayerEffect (pos)));
					gridObject.AddProperty (new ImageChanger (gridObject, _sprites [idx==0?"commander-bot-2":"sp01_mercenary"]));
				}
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

		void ShowDamageEnemyEffect(Vector3 gridPos)
		{
			_coroutineStarter.StartCoroutine (ShowDamageEffectWaitAndHide (gridPos));
		}

		IEnumerator ShowDamageEffectWaitAndHide(Vector3 gridPos)
		{
			GameObject effect = GameObject.Instantiate (_gameObjects["Effect"]);
			effect.transform.SetParent(_canvas.transform);
			effect.transform.localPosition = _grid.Utils.GridPosToWorldPos(gridPos);
			effect.transform.localScale = Vector3.one;
			yield return new WaitForSeconds(0.3f);
			GameObject.DestroyImmediate (effect);
		}

		void ShowDamagePlayerEffect(Vector3 gridPos)
		{
			_coroutineStarter.StartCoroutine (ShowDamageEffectFlyAndHide (gridPos));
		}

		IEnumerator ShowDamageEffectFlyAndHide(Vector3 gridPos)
		{
			yield return new WaitForSeconds(0.5f);
			Vector3 fromPos = _grid.Utils.GridPosToWorldPos(gridPos);
			GameObject effect = GameObject.Instantiate (_gameObjects["Effect"]);
			effect.transform.SetParent(_canvas.transform);
			effect.transform.localPosition = fromPos;
			effect.transform.localScale = Vector3.one*0.3f;

			GameObject player = GameObject.Find ("PlayerHealthBar");
			float d = 1f;
			while (d >= 0) 
			{
				d -= Time.deltaTime;
				effect.transform.localPosition = Vector3.Lerp (fromPos, player.transform.localPosition, 1-d);
				yield return null;
			}
			GameObject.DestroyImmediate (effect);

			player.GetComponent<Image> ().fillAmount -= 0.01f;
		}

		void OnDestroyGridObject(Vector3 gridPos)
		{
			Vector3 spawnPos = gridPos;
			spawnPos.y = 5;
			while (_grid.GetFromCell (spawnPos) != null) 
			{
				spawnPos += Vector3.up;
			}
			GridObject gridObject = Spawn (spawnPos, _gameObjects["Enemy"]);

			int idx = UnityEngine.Random.Range (0, 2);

			gridObject.AddProperty (new EnemyObject (gridObject, idx==0?3:4, (Vector3 pos) => ShowDamagePlayerEffect (pos)));
			gridObject.AddProperty (new ImageChanger (gridObject, _sprites [idx==0?"commander-bot-2":"sp01_mercenary"]));

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
