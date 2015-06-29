using UnityEngine;
using System.Collections;

public class BlackFades : MonoBehaviour
{
	private static BlackFades m_Instance = null;
	private Material m_Material = null;
	private bool m_Fading = false;
	
	private static BlackFades Instance
	{
		get
		{
			if (m_Instance == null)
			{
				m_Instance = (new GameObject("BlackFades")).AddComponent<BlackFades>();
			}
			return m_Instance;
		}
	}

	public static bool Fading
	{
		get { return Instance.m_Fading; }
	}
	
	private void Awake()
	{
		DontDestroyOnLoad(this);
		m_Instance = this;
		m_Material = new Material("Shader \"Plane/No zTest\" { SubShader { Pass { Blend SrcAlpha OneMinusSrcAlpha ZWrite Off Cull Off Fog { Mode Off } BindChannels { Bind \"Color\",color } } } }");
	}
	
	private void DrawQuad(Color aColor,float aAlpha)
	{
		aColor.a = aAlpha;
		m_Material.SetPass(0);
		GL.PushMatrix();
		GL.LoadOrtho();
		GL.Begin(GL.QUADS);
		GL.Color(aColor);   // moved here, needs to be inside begin/end
		GL.Vertex3(0, 0, -1);
		GL.Vertex3(0, 1, -1);
		GL.Vertex3(1, 1, -1);
		GL.Vertex3(1, 0, -1);
		GL.End();
		GL.PopMatrix();
	}
	
	private IEnumerator FadeInLoop(float aFadeOutTime, float aFadeInTime, Color aColor)
	{
		float t = 1.0f;

		while (t>0.0f)
		{
			Debug.Log ("Fade out before t: " + t);
			yield return new WaitForEndOfFrame();
			t = Mathf.Clamp01(t - Time.deltaTime / aFadeInTime);
			Debug.Log ("Fade out after t: " + t);
			DrawQuad(aColor,t);
		}
		m_Fading = false;
	}

	private IEnumerator FadeOutLoop(float aFadeOutTime, float aFadeInTime, Color aColor)
	{
		float t = 0.0f;
		while (t<1.0f)
		{
			Debug.Log ("Fade in");
			yield return new WaitForEndOfFrame();
			t = Mathf.Clamp01(t + Time.deltaTime / aFadeOutTime);
			DrawQuad(aColor,t);
		}
		m_Fading = false;
	}

	private void FadeInStart(float aFadeOutTime, float aFadeInTime, Color aColor)
	{
		m_Fading = true;
		StartCoroutine(FadeInLoop(aFadeOutTime, aFadeInTime, aColor));
	}

	private void FadeOutStart(float aFadeOutTime, float aFadeInTime, Color aColor)
	{
		m_Fading = true;
		StartCoroutine(FadeOutLoop(aFadeOutTime, aFadeInTime, aColor));
	}

	public static void FadeIn(float aFadeOutTime, float aFadeInTime, Color aColor)
	{
		if (Fading) return;
		Instance.FadeInStart(aFadeOutTime, aFadeInTime, aColor);
	}

	public static void FadeOut(float aFadeOutTime, float aFadeInTime, Color aColor)
	{
		if (Fading) return;
		Instance.FadeOutStart(aFadeOutTime, aFadeInTime, aColor);
	}
}