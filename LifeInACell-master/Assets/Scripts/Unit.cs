using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.UpArrow))
		{
			Instantiater.generationInterval += 1F;
			Debug.Log(Instantiater.generationInterval);
		}
		if (Input.GetKeyDown(KeyCode.DownArrow))
		{
			Instantiater.generationInterval -= 1F;
			Debug.Log(Instantiater.generationInterval);
		}
		Debug.Log(Instantiater.generationInterval);
	}
}