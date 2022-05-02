using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Policeman : Unit
{
   private void OnTriggerEnter2D(Collider2D collider)
    {

	    Prison prison = collider.GetComponent<Prison>();

    	if (prison)
    	{
    		Destroy(gameObject);
    	}

    	Regeneration regeneration = collider.GetComponent<Regeneration>();

    	if (regeneration)
    	{
    		int temp = Random.Range(1, 5);
    		if (temp % 4 == 1)
    			Instantiate(gameObject);
    	}
    }
}
