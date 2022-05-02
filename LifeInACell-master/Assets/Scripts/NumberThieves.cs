using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NumberThieves : MonoBehaviour
{
    Text numberThieves; 

	private void Awake()
	{
		numberThieves = GetComponent<Text>();
	}

    private void Update()
	{
		GameObject[] theives = GameObject.FindGameObjectsWithTag ("Thief");
		int Total = theives.Length;

		numberThieves.text = "Total theives: " + Total;
	}	
}
