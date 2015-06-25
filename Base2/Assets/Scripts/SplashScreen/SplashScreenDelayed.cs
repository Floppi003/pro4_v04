using UnityEngine;
using System.Collections;

public class SplashScreenDelayed : MonoBehaviour {
	public float delayTime = 4;
	public string nextLevel;

	IEnumerator Start(){
		yield return new WaitForSeconds (delayTime);
		//Application.LoadLevel ("Central");

		AutoFade.LoadLevel(nextLevel ,1,10,Color.black);
		//levelname, fadeouttime, fadeintime, fadecolor
	}
}