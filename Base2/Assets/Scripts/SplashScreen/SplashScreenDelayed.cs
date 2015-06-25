using UnityEngine;
using System.Collections;

public class SplashScreenDelayed : MonoBehaviour {
	public float delayTime;
	public string nextLevel;

	IEnumerator Start(){
		yield return new WaitForSeconds (delayTime);
		//Application.LoadLevel ("Central");

		AutoFade.LoadLevel(nextLevel ,1, 1, Color.black);
		//levelname, fadeouttime, fadeintime, fadecolor
	}
}