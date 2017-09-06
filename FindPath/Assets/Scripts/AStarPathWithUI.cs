using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AStarPathWithUI : MonoBehaviour
{
    public Transform wall;
    public Transform plane;

	void Start ()
    {
        wall = Resources.Load<Transform>("Prefabs/Tran_FindPath");
        plane = gameObject.transform.Find("Img_Plane");

        GameObject wall_clone = Instantiate(wall.gameObject);
        wall_clone.transform.SetParent(plane);
        wall_clone.transform.localPosition = new Vector2(15, 15);
    }
	
	void Update ()
    {
		
	}
}
