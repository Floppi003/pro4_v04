using Tobii.EyeX.Framework;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[AddComponentMenu("Tobii EyeX/Eye Position Data")]
public class EyePosition_L3 : MonoBehaviour {

	enum EyesOpened{None, Left, Right, Both};

	private EyeXHost eyexHost;
	private IEyeXDataProvider<EyeXEyePosition> dataProvider;
	public EyeXEyePosition lastEyePosition { get; private set; }

	private Queue<EyesOpened> eyesOpenedQueue;
	private int eyesClosedCounter;



	protected void Awake () {
		Debug.Log ("awake");
		this.eyexHost = EyeXHost.GetInstance();
		this.dataProvider = eyexHost.GetEyePositionDataProvider();
		this.eyesOpenedQueue = new Queue<EyesOpened> ();
	}

	protected void OnEnable() {
		Debug.Log ("Enable");
		this.dataProvider.Start();
	}
	
	protected void OnDisable() {
		Debug.Log ("Disable");
		this.dataProvider.Stop();
	}


	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		lastEyePosition = dataProvider.Last;
		//EyeXEyePosition lastEyePosition = GetComponent<EyePositionDataComponent>().LastEyePosition;


		if (!lastEyePosition.LeftEye.IsValid && !lastEyePosition.RightEye.IsValid) {
			// Eyes closed
			this.eyesOpenedQueue.Enqueue (EyesOpened.None);
			eyesClosedCounter++;
			//Debug.Log ("!!! - !!! both eyes closed!!!!!!!!");
			
		} else if (lastEyePosition.LeftEye.IsValid && !lastEyePosition.RightEye.IsValid) {
			// only left eye is opened
			this.eyesOpenedQueue.Enqueue (EyesOpened.Left);
			
		} else if (!lastEyePosition.LeftEye.IsValid && lastEyePosition.RightEye.IsValid) {
			// only right eye is opened
			this.eyesOpenedQueue.Enqueue (EyesOpened.Right);
			
		} else {
			// both eyes opened
			this.eyesOpenedQueue.Enqueue (EyesOpened.Both);
			eyesClosedCounter = eyesClosedCounter / 3;
		}


		// check whether bridge should be broad or not
		// make bridge wider if both eyes are closed
		GameObject bridge = GameObject.Find ("Bridge");
		Debug.Log ("eyesClosedCounter: " + eyesClosedCounter);

		if (eyesClosedCounter > 24) {
			bridge.transform.localScale = new Vector3(1, 1, 8);
			GameObject.Find ("BridgeCollider").GetComponent<BoxCollider>().enabled = false;
			//GetComponent<BridgeCollider>().enabled = false;
		} else {
			bridge.transform.localScale = new Vector3(1, 1, 1.0f);
			GameObject.Find ("BridgeCollider").GetComponent<BoxCollider>().enabled = true;
			//GetComponent<BridgeCollider>().enabled = true;
		}
	}
}
