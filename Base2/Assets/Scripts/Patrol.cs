using UnityEngine;
using System.Collections;

public class Patrol : MonoBehaviour {
	public Transform[] patrolPoints;
	float moveSpeed = 20;
	private int currentPoint;

	// Use this for initialization
	void Start () {
		transform.position = patrolPoints[0].position;
		currentPoint = 0;
	}
	
	// Update is called once per frame
	void Update () {
		/*
		if (transform.position == patrolPoints[currentPoint].position)
		{
			currentPoint++;
		}

		if (currentPoint >= patrolPoints.Length)
		{
			currentPoint = 0;
			Debug.Log ("Reset Point" + currentPoint);
		}
		*/
		transform.position = Vector3.MoveTowards(transform.position, patrolPoints[currentPoint].position, moveSpeed * Time.deltaTime);
	}

	public void NextPatrolPoint(){
		Debug.Log ("Next Patrol Point" + currentPoint);
		currentPoint++;

		if (currentPoint >= patrolPoints.Length)
		{
			currentPoint = 0;
			Debug.Log ("Reset Point" + currentPoint);
		}
	}
}