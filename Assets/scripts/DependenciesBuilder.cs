using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace SCPuzzle
{
	public class DependenciesBuilder : MonoBehaviour 
	{
		#if UNITY_EDITOR	
		[MenuItem("Edit/Reset Playerprefs")]
		public static void DeletePlayerPrefs()
		{
			PlayerPrefs.DeleteAll();
		}
		#endif

		void Awake () 
		{
			Screen.SetResolution (500, 800, false);
			IGrid grid = new Grid ();
			CoroutineStarter coroutineStarter = gameObject.AddComponent<CoroutineStarter> ();
			HeroesSpawner heroesSpawner = new HeroesSpawner (grid, coroutineStarter);
			InputManager inputManager = gameObject.AddComponent<InputManager> ();
			inputManager.Init (grid);
		}
	
	}
}