using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataStoring : MonoBehaviour
{
	public string uName = "Guest";
    // Start is called before the first frame update
    void Start()
    {
		DontDestroyOnLoad(gameObject);
    }
	
}
