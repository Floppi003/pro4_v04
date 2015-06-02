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

		if (lastEyePosition.IsValid) {
			// Get the position of the left eye.
			Vector3 leftEyePosition = new Vector3 (lastEyePosition.LeftEye.X,
			                                       lastEyePosition.LeftEye.Y,
			                                       lastEyePosition.LeftEye.Z);
			
			
			// The Eye Position of a closed eye will be (0, 0, 0)
			// Sometimes single eye positions can be (0, 0, 0) even though the eye was opened

			// Get the position of the right eye.
			Vector3 rightEyePosition = new Vector3 (lastEyePosition.RightEye.X,
			                                       lastEyePosition.RightEye.Y,
			                                       lastEyePosition.RightEye.Z);


			if (!lastEyePosition.LeftEye.IsValid && !lastEyePosition.RightEye.IsValid) {
				// Eyes closed
				this.eyesOpenedQueue.Enqueue (EyesOpened.None);
				eyesClosedCounter++;
			
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

			// calculate the amount of each eye-State in the queue!
			if (this.eyesOpenedQueue.Count > 30) {
				EyesOpened[] eyesOpenedArray = this.eyesOpenedQueue.ToArray ();
				int noneCount = 0;
				int leftCount = 0;
				int rightCount = 0;
				int bothCount = 0;
			
				foreach (EyesOpened eyesOpened in eyesOpenedArray) {
				
					switch (eyesOpened) {
					case EyesOpened.None:
						{
							noneCount++;
							break;
						} 

					case EyesOpened.Left:
						{
							leftCount++;
							break;
						}
					
					case EyesOpened.Right:
						{
							rightCount++;
							break;
						}
					
					case EyesOpened.Both:
						{
							bothCount++;
							break;
						}
					}
				}


				// check which left-eye / right-eye objects should be visible
				if (noneCount >= leftCount && noneCount >= rightCount && noneCount >= bothCount) {
					// "none" was most often recognized
					this.hideLeftEyeObjects ();
					this.hideRightEyeObjects ();
				
				} else if (leftCount >= rightCount && leftCount >= bothCount) {
					// "left" was most often recognized
					this.showLeftEyeObjects ();
					this.hideRightEyeObjects ();
				
				} else if (rightCount >= bothCount) {
					// "right" was most often recognized
					this.hideLeftEyeObjects ();
					this.showRightEyeObjects ();
				
				} else {
					// "both" was most often recognized
					this.hideLeftEyeObjects ();
					this.hideRightEyeObjects ();
				}
			
				// check if eyes were closed 
				if (leftCount == 0 && rightCount == 0 && bothCount == 0) {
				
					// both eyes were closed for a while, if you are in felldown mode respawn
					FirstPersonController fps = GameObject.Find ("Player").GetComponent<FirstPersonController> ();
				}
			
				this.eyesOpenedQueue.Dequeue ();
			}

			// check whether bridge should be broad or not
			// make bridge wider if both eyes are closed
			GameObject bridge = GameObject.Find ("bridge");
		
			if (eyesClosedCounter > 24) {
				bridge.transform.localScale = new Vector3 (1, 1, 6);
				GameObject.Find ("BridgeCollider").GetComponent<BoxCollider> ().enabled = false;
				//GetComponent<BridgeCollider>().enabled = false;
			} else {
				bridge.transform.localScale = new Vector3 (1, 1, 1.0f);
				GameObject.Find ("BridgeCollider").GetComponent<BoxCollider> ().enabled = true;
				//GetComponent<BridgeCollider>().enabled = true;
			}
		}
	}
	
	private void showLeftEyeObjects() {
		GameObject[] leftEyeObjects = GameObject.FindGameObjectsWithTag ("LeftEye");
		
		foreach (GameObject leftEyeObject in leftEyeObjects) {
			//leftEyeObject.GetComponent<MeshCollider>().enabled = true;
			leftEyeObject.GetComponent<MeshRenderer>().enabled = true;
		}
	}
	
	private void showRightEyeObjects() {
		GameObject[] rightEyeObjects = GameObject.FindGameObjectsWithTag ("RightEye");
		
		foreach (GameObject rightEyeObject in rightEyeObjects) {
			//rightEyeObject.GetComponent<MeshCollider>().enabled = true;
			rightEyeObject.GetComponent<MeshRenderer>().enabled = true;
		}
	}
	
	private void hideLeftEyeObjects() {
		GameObject[] leftEyeObjects = GameObject.FindGameObjectsWithTag ("LeftEye");
		
		foreach (GameObject leftEyeObject in leftEyeObjects) {
			//leftEyeObject.GetComponent<MeshCollider>().enabled = false;
			leftEyeObject.GetComponent<MeshRenderer>().enabled = false;
		}
	}
	
	private void hideRightEyeObjects() {
		GameObject[] rightEyeObjects = GameObject.FindGameObjectsWithTag ("RightEye");
		
		foreach (GameObject rightEyeObject in rightEyeObjects) {
			//rightEyeObject.GetComponent<MeshCollider>().enabled = false;
			rightEyeObject.GetComponent<MeshRenderer>().enabled = false;
		}
	}
}
