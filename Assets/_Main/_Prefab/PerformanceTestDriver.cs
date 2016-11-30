using UnityEngine;
using System.Collections;
using System.Diagnostics;

public class PerformanceTestDriver : MonoBehaviour
{
	void OnGUI ()
	{
		if (GUI.Button (new Rect (100, 100, 150, 100), "Resource Load")) {
			print ("---- Resource Load ----");
			ResourcesLoad ();
		}


		if (GUI.Button (new Rect (300, 100, 150, 100), "Create")) {
			print ("---- Create View Model ----");
			CreateViewModel ();
		}
	}

	GameObject PTestPrefab;
	GameObject PTest1Prefab;
	GameObject PTest2Prefab;

	void ResourcesLoad ()
	{
		Stopwatch sw = new Stopwatch ();

		sw.Reset ();
		sw.Start ();
		PTestPrefab = Resources.Load<GameObject> ("PTest");
		print (sw.ElapsedMilliseconds);

		sw.Reset ();
		sw.Start ();
		PTest1Prefab = Resources.Load<GameObject> ("PTest1");
		print (sw.ElapsedMilliseconds);

		sw.Reset ();
		sw.Start ();
		PTest2Prefab = Resources.Load<GameObject> ("PTest2");
		print (sw.ElapsedMilliseconds);

		sw.Stop ();
	}

	void CreateViewModel ()
	{
		Stopwatch sw = new Stopwatch ();

		sw.Reset ();
		sw.Start ();
		GameObject go = Instantiate<GameObject> (PTestPrefab);
		print (sw.ElapsedMilliseconds);

		sw.Reset ();
		sw.Start ();
		PTestView v = go.GetComponent<PTestView> ();
		v.CreateViewModel ();
		print (sw.ElapsedMilliseconds);

		sw.Reset ();
		sw.Start ();
		v.Bind ();
		print (sw.ElapsedMilliseconds);

		sw.Stop ();

		Destroy (go);
	}
}
