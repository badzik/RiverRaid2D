using UnityEngine;
using System.Collections;

public class Container : MonoBehaviour {
    public static Container i;

    public int[][] ActualLevel { get; set; }
    public int SavedLevel { get; set; }
    // Use this for initialization
    void Start()
    {
        SavedLevel = 1;
        ActualLevel = new int[32][];
        for (int i = 0; i < ActualLevel.Length; i++)
        {
            ActualLevel[i] = new int[2];
        }
    }

    void Awake()
    {
        if (i == null)
        {
            i = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(this); // or gameObject
    }

    // Update is called once per frame
    void Update () {
	
	}
}
