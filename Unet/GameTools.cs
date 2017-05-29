using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public static class GameTools 
{

    //繪製Debug用射線,從CharacterBehaviorController接收
	public static RaycastHit2D GameRaycast(Vector2 rayOriginPoint, Vector2 rayDirection, float rayDistance, LayerMask mask,bool debug,Color color)
	{			
		Debug.DrawRay( rayOriginPoint, rayDirection*rayDistance, color );
		return Physics2D.Raycast(rayOriginPoint,rayDirection,rayDistance,mask);		
	}

	public static void DebugLogTime(object message)
	{
		Debug.Log (Time.deltaTime + " " + message);

	}

    public static IEnumerator FadeImage(Image target, float duration, Color color)
    {
        if (target == null)
            yield break;

        float alpha = target.color.a;

        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / duration)
        {
            if (target == null)
                yield break;
            Color newColor = new Color(color.r, color.g, color.b, Mathf.SmoothStep(alpha, color.a, t));
            target.color = newColor;
            yield return null;
        }
    }

	public static IEnumerator FadeText(Text target, float duration, Color color)
	{
		if (target==null)
			yield break;
			
		float alpha = target.color.a;
		
		for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / duration)
		{
			if (target==null)
				yield break;
			Color newColor = new Color(color.r, color.g, color.b, Mathf.SmoothStep(alpha,color.a,t));
			target.color=newColor;
			yield return null;
		}
	}

	public static IEnumerator FadeSprite(SpriteRenderer target, float duration, Color color)
	{
		if (target==null)
			yield break;
	
		float alpha = target.material.color.a;		
		
		float t=0f;
		while (t<1.0f)
		{
			if (target==null)
				yield break;
								
			Color newColor = new Color(color.r, color.g, color.b, Mathf.SmoothStep(alpha,color.a,t));
			target.material.color=newColor;
			
			t += Time.deltaTime / duration;
			
			yield return null;
				
		}
		Color finalColor = new Color(color.r, color.g, color.b, Mathf.SmoothStep(alpha,color.a,t));
		target.material.color=finalColor;
	}
}
