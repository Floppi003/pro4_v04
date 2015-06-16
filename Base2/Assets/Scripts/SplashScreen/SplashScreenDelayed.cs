using UnityEngine;
using System.Collections;

public class SplashScreenDelayed : MonoBehaviour {
	public float delayTime = 2;

	IEnumerator Start(){
		yield return new WaitForSeconds (delayTime);
		//Application.LoadLevel ("Central");

		//AutoFade.LoadLevel("Central" ,1,10,Color.black);
		//levelname, fadeouttime, fadeintime, fadecolor
	}
}