using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SCPuzzle
{
	public class HeroesSpawner
	{
		private IGrid _grid;

		public HeroesSpawner(IGrid grid)
		{
			_grid = grid;
			Spawn ();
		}

		public void Spawn()
		{
			Canvas canvas = GameObject.FindObjectOfType<Canvas> ();
			GameObject heroGameObjectTemplate = Resources.Load<GameObject> ("Hero");
			for (int i = 0; i < 25; i++) 
			{
				GameObject heroGameObject = GameObject.Instantiate (heroGameObjectTemplate);
				heroGameObject.transform.parent = canvas.transform;

				GridObject heroGridObject = new GridObject (heroGameObject, _grid);
				heroGridObject.GridPos = new Vector3 (i % 5, i / 5, 0);
				heroGridObject.AddProperty (new SelectableWithImageColor (heroGridObject));

				if(i == 10|| i== 2 ||i == 18)
					heroGridObject.GetProperty<SelectableWithImageColor> ().Select ();
			}
		}
	}
}
