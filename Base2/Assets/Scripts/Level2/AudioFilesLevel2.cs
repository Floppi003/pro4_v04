using UnityEngine;
using System.Collections;

public class AudioFilesLevel2 : MonoBehaviour {

	public AudioClip firstRiddleSuccessSound;
	public AudioClip platformMovingSound;
	public AudioClip buttonPressedSound;
	public AudioClip buttonReleasedSound;
	public AudioClip buttonGazedSound;


	// Sonny Voice Clips
	public AudioClip L2_Start;
	public AudioClip[] R1_DrueckDenSchalter;
	public AudioClip R1_Schalter;
	public AudioClip R2_GuteAlteSchalter;
	public AudioClip R2_Start;
	public AudioClip R3_BlauerSchalter;
	public AudioClip Bombenentschaerfer;
	public AudioClip[] R3_Blau;
	public AudioClip[] R3_Gelb;
	public AudioClip[] R3_Gruen;
	public AudioClip[] R3_Rot;
	public AudioClip[] R3_Geschafft;
	public AudioClip R3_Planaenderung;
	public AudioClip R3_ZuerstBlau;
	public AudioClip R4_MengeSchalter;
	public AudioClip R4_ReiseGehtWeiter;
	public AudioClip R4_SchonBesser;
	public AudioClip R4_SoWirdDasNix;


	public AudioClip getAudioClipR1_DrueckDenschalter() {
		int index = Random.Range (0, this.R1_DrueckDenSchalter.Length);
		return this.R1_DrueckDenSchalter [index];
	}
}
