using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EyePosition_L3 : MonoBehaviour {

	enum EyesOpened{None, Left, Right, Both};

	private EyeXHost eyexHost;
	private IEyeXDataProvider<EyeXEyePosition> dataProvider;
	public EyeXEyePosition lastEyePosition { get; private set; }

	private Queue<EyesOpened> eyesOpenedQueue;
	private int eyesClosedCounter;



	protected void awake() {
		eyexHost = EyeXHost.GetInstance();
		dataProvider = eyexHost.GetEyePositionDataProvider();
	}

	protected void OnEnable() {
		dataProvider.Start();
	}
	
	protected void OnDisable() {
		dataProvider.Stop();
	}


	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		lastEyePosition = dataProvider.Last;


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
		GameObject bridge = GameObject.Find ("bridge");
		
		if (eyesClosedCounter > 24) {
			bridge.transform.localScale = new Vector3(1, 1, 6);
			GameObject.Find ("BridgeCollider").GetComponent<BoxCollider>().enabled = false;
			//GetComponent<BridgeCollider>().enabled = false;
		} else {
			bridge.transform.localScale = new Vector3(1, 1, 1.0f);
			GameObject.Find ("BridgeCollider").GetComponent<BoxCollider>().enabled = true;
			//GetComponent<BridgeCollider>().enabled = true;
		}
	}
}
