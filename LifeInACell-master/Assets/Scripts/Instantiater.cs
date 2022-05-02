using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;


public class Instantiater : MonoBehaviour
{	
	public static float generationInterval = 1F;
    
	int[ , ] thiefArray;
	int[ , ] policemanArray;
	int[ , ] prisonArray;
	int[ , ] doughnutArray;

	public int gridHeight;
	private int gridWidth;

	private float cellSize;
	private float prisonSize;
	private float doughnutSize;

	public Regeneration regeneration;
	public Prison prison;
	public Policeman policeman;
	public Thief thief;


    private void Start()
    {
    	gridWidth = Mathf.RoundToInt(gridHeight * Camera.main.aspect);

    	cellSize = (Camera.main.orthographicSize * 2) / gridHeight;
    	prisonSize = (Camera.main.orthographicSize * 2) / gridHeight;
    	doughnutSize = (Camera.main.orthographicSize * 2) / gridHeight;

    	thiefArray = new int[gridHeight, gridWidth];
    	policemanArray = new int[gridHeight, gridWidth];
    	prisonArray = new int[gridHeight, gridWidth];
    	doughnutArray = new int[gridHeight, gridWidth];
    	
    	GenerateDoughnuts();
    	GeneratePrison();
        InvokeRepeating("NewGenerationUpdate", generationInterval, generationInterval);
    }

    private void NewGenerationUpdate()
    {
    	ApplyRules();
    	GenerateCells(thief, ref thiefArray, "Thief");
    	GenerateCells(policeman, ref policemanArray, "Policeman");
    }
    
	private bool CheckOverlapping(int[,] arr, int row, int col) {
	    int start, start1, final, final1;

	    if (row - 20 < 0)
	        start = 0;
	    else
	        start = row - 20;
	    if (row + 20 >= gridHeight)
	        final = gridHeight - 1;
	    else
	        final = row + 20;


	    if (col - 20 < 0)
	        start1 = 0;
	    else
	        start1 = col - 20;
	    if (col + 20 >= gridWidth)
	        final1 = gridWidth - 1;
	    else
	        final1 = col + 20;


	    for(int i = start; i <= final; i++)
	        for (int j = start1; j <= final1; j++)
	            if(arr[i,j] == 1)
	                return false;


	    return true;
	}
    
    private void GenerateDoughnuts()
    {
    	foreach (GameObject cell in GameObject.FindGameObjectsWithTag("Regeneration"))
    	{
    		Destroy(cell);
    	}
    	for (int i = 0; i < 3; i++)
    	{
    		doughnutArray[Random.Range(20, 70), Random.Range(30, 120)] = 1;
    	}

    	for (int i = 0; i < gridHeight; i++)
    	{
    		for (int j = 0; j < gridWidth; j++ )
    		{
    			if (doughnutArray[i, j] == 0) continue;
    			Vector3 Position = new Vector3(
    				j * doughnutSize + doughnutSize/2,
    				(doughnutSize * gridHeight) - (i * doughnutSize + doughnutSize/2),
    				0
    			);
                

    			Regeneration clone3 = Instantiate(regeneration, Position, Quaternion.identity) as Regeneration;
    		}
    	}
    }

    private void GeneratePrison()
    {
    	foreach (GameObject cell in GameObject.FindGameObjectsWithTag("Prison"))
    	{
    		Destroy(cell);
    	}

    	int count = 0;
    	while (count != 5)
    	{
    		int row = Random.Range(20, 70);
    		int col = Random.Range(30, 120);

    		if (!CheckOverlapping(doughnutArray, row, col))
    		{
    			prisonArray[row, col] = 1;
    			count++;
    		}
    	}

    	for (int i = 0; i < gridHeight; i++)
    	{
    		for (int j = 0; j < gridWidth; j++ )
    		{
    			if (prisonArray[i, j] == 0) continue;
    			Vector3 Position = new Vector3(
    				j * prisonSize + prisonSize/2,
    				(prisonSize * gridHeight) - (i * prisonSize + prisonSize/2),
    				0
    			);

    			Prison clone1 = Instantiate(prison, Position, Quaternion.identity) as Prison;
    		}
    	}
    }

    private void GenerateCells<T>(T animal, ref int[ , ] arr, string tag) where T: Unit
    {
    	foreach (GameObject cell in GameObject.FindGameObjectsWithTag(tag))
    	{
    		Destroy(cell);
    	}

    	for (int i = 0; i < 5; i++)
    	{
    		arr[Random.Range(20, 70), Random.Range(30, 120)] = 1;
    	}

    	for (int i = 0; i < gridHeight; i++)
    	{
    		for (int j = 0; j < gridWidth; j++ )
    		{
    			if (arr[i, j] == 0) continue;
    			Vector3 Position = new Vector3(
    				j * cellSize + cellSize/2,
    				(cellSize * gridHeight) - (i * cellSize + cellSize/2),
    				0
    			);

    			T clone = Instantiate(animal, Position, Quaternion.identity) as T;    			
    		}
    	}
    }


    private void ApplyRules()
    {
    	CatchThieves();
    	BeatPolice();
    	thiefArray = Breed(thiefArray);
    	policemanArray = Breed(policemanArray);
    }

    private int[,] Breed(int[, ] arr)
    {
    	int[ , ]nextGenGrid = new int[gridHeight, gridWidth];
    	for (int i = 0; i < gridHeight; i++)
    	{
    		for (int j = 0; j < gridWidth; j++ )
    		{
    			int livingNeighbours = CountLivingNeighbours(i, j, arr);
    			if (livingNeighbours == 3)	// Reproduction, exactly 3 neighbours
    			{
    				nextGenGrid[i, j] = 1;
    			}
    			else if (livingNeighbours == 2 && arr[i, j] == 1)	// exactly 2 neigh, the live cell survives
    			{
    				nextGenGrid[i, j] = 1;
    			}
    			else if (livingNeighbours == 0)
    			{
    				nextGenGrid[i, j] = 0;
    			}

    		}
    	}
       	
    	return arr;	// GOING TO THE NEXT GEN!!! 
    }

    //thieves will get caught 
	private void CatchThieves() 
    {
        for(int i = 1; i < gridHeight - 1; i++)
        {
            for(int j = 1; j < gridWidth - 1; j++ ) 
            {
                if (policemanArray[i, j] == 0) continue;

                if (thiefArray[i - 1, j] == 1) 
                {
	                thiefArray[i - 1, j] = 0;
	                policemanArray[i, j] = 0;
                    policemanArray[i - 1, j] = 1;
                    continue;
                }

                if (thiefArray[i - 1, j - 1] == 1) 
                {
	                thiefArray[i - 1, j - 1] = 0;
	                policemanArray[i - 1, j - 1] = 1;
	                policemanArray[i, j] = 0;
                    continue;
                }

                if (thiefArray[i - 1, j + 1] == 1) 
                {
	                thiefArray[i - 1, j + 1] = 0;
	                policemanArray[i - 1, j + 1] = 1;
	                policemanArray[i, j] = 0;
                    continue;
                }

                if (thiefArray[i, j - 1] == 1) 
                {
	                thiefArray[i, j - 1] = 0;
	                policemanArray[i, j - 1] = 1;
	                policemanArray[i, j] = 0;
                    continue;
                }

                if (thiefArray[i, j + 1] == 1) 
                {
	                thiefArray[i, j + 1] = 0;
	                policemanArray[i, j + 1] = 1;
	                policemanArray[i, j] = 0;
                    continue;
                }

                if (thiefArray[i + 1, j - 1] == 1) 
                {
	                thiefArray[i + 1, j - 1] = 0;
	                policemanArray[i + 1, j - 1] = 1;
	                policemanArray[i, j] = 0;
                    continue;
                }

                if (thiefArray[i + 1, j] == 1) 
                {
	                thiefArray[i + 1, j] = 0;
	                policemanArray[i + 1, j] = 1;
	                policemanArray[i, j] = 0;
                    continue;
                }

                if (thiefArray[i + 1, j + 1] == 1) 
                {
	                thiefArray[i + 1, j + 1] = 0;
	                policemanArray[i + 1, j + 1] = 1;
	                policemanArray[i, j] = 0;
                }
            }
        }
    }

    //thieves will beat a policeman
    private void BeatPolice()
    {
    	for(int i = 1; i < gridHeight - 1; i++)
        {
            for(int j = 1; j < gridWidth - 1; j++ ) 
            {
            	if (policemanArray[i, j] == 1)
            	{
	            	if (thiefArray[i - 1, j] == 1 && thiefArray[i + 1, j] == 1 && thiefArray[i, j - 1] == 1 && thiefArray[i, j + 1] == 1)
	            	{
		                policemanArray[i, j] = 0;
	            		continue;	
	            	}

	            	if (thiefArray[i - 1, j - 1] == 1 && thiefArray[i - 1, j + 1] == 1 && thiefArray[i + 1, j - 1] == 1 && thiefArray[i + 1, j + 1] == 1)
	            	{
		                policemanArray[i, j] = 0;
	            		continue;	
	            	}

	            	if (thiefArray[i + 1, j - 1] == 1 && thiefArray[i, j - 1] == 1 && thiefArray[i - 1, j - 1] == 1 && thiefArray[i - 1, j] == 1)
	            	{
		                policemanArray[i, j] = 0;
	            		continue;	
	            	}

	            	if (thiefArray[i - 1, j] == 1 && thiefArray[i - 1, j + 1] == 1 && thiefArray[i, j + 1] == 1 && thiefArray[i + 1, j + 1] == 1)
	            	{
		                policemanArray[i, j] = 0;
	            		continue;	
	            	}

	            	if (thiefArray[i + 1, j - 1] == 1 && thiefArray[i, j - 1] == 1 && thiefArray[i - 1, j - 1] == 1 && thiefArray[i + 1, j] == 1)
	            	{
		                policemanArray[i, j] = 0;
	            		continue;	
	            	}

	            	if (thiefArray[i + 1, j] == 1 && thiefArray[i - 1, j + 1] == 1 && thiefArray[i, j + 1] == 1 && thiefArray[i + 1, j + 1] == 1)
	            	{
		                policemanArray[i, j] = 0;
	            		continue;	
	            	}

	            	if (thiefArray[i - 1, j - 1] == 1 && thiefArray[i - 1, j] == 1 && thiefArray[i - 1, j + 1] == 1 && thiefArray[i, j + 1] == 1)
	            	{
		                policemanArray[i, j] = 0;
	            		continue;	
	            	}

	            	if (thiefArray[i - 1, j - 1] == 1 && thiefArray[i - 1, j] == 1 && thiefArray[i - 1, j + 1] == 1 && thiefArray[i, j - 1] == 1)
	            	{
		                policemanArray[i, j] = 0;
	            		continue;	
	            	}

	            	if (thiefArray[i, j - 1] == 1 && thiefArray[i + 1, j - 1] == 1 && thiefArray[i + 1, j] == 1 && thiefArray[i + 1, j + 1] == 1)
	            	{
		                policemanArray[i, j] = 0;
	            		continue;	
	            	}

	            	if (thiefArray[i, j + 1] == 1 && thiefArray[i + 1, j - 1] == 1 && thiefArray[i + 1, j] == 1 && thiefArray[i + 1, j + 1] == 1)
	            	{
		                policemanArray[i, j] = 0;
	            	}
	            }	
            }	
        }    
    }

    private int CountLivingNeighbours(int i, int j, int[,] arr)
    {
    	int result = 0;
    	for (int iNeigh = i - 1; iNeigh < i + 2; iNeigh++)
    	{
    		for (int jNeigh = j - 1; jNeigh < j + 2; jNeigh++)
    		{
    			if (iNeigh == i && jNeigh == j) continue;
    			try 
    			{
    				result += arr[iNeigh, jNeigh];
    			}
    			catch{}
    		}
    	}
        return result;
    }
}