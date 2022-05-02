using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IntervalText : MonoBehaviour
{
	Text intervalText;


	private void Awake()
	{
		intervalText = GetComponent<Text>();
		intervalText.text = Instantiater.generationInterval.ToString();
	}

    private void Update()
	{
		if (Input.GetKeyDown(KeyCode.UpArrow))
		{
			Instantiater.generationInterval += 1F;
		}
		if (Input.GetKeyDown(KeyCode.DownArrow))
		{
			Instantiater.generationInterval -= 1F;
		}  
		intervalText.text = Instantiater.generationInterval.ToString();                       
	}

}
