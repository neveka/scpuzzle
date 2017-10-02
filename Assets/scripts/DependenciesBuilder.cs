using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SCPuzzle
{
	public class DependenciesBuilder : MonoBehaviour 
	{
		void Awake () 
		{
			IGrid grid = new Grid ();

			HeroesSpawner heroesSpawner = new HeroesSpawner (grid);
		}
	
	}
}