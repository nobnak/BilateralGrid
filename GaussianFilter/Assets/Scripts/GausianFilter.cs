using UnityEngine;
using System.Collections;

public class GausianFilter : MonoBehaviour {
	public Material gaussianX;
	public Material gaussianY;

	void OnRenderImage(RenderTexture src, RenderTexture dst) {
		var tmp0 = RenderTexture.GetTemporary(src.width, src.height);
		try {
			Graphics.Blit(src, tmp0, gaussianX);
			Graphics.Blit(tmp0, dst, gaussianY);
		} finally {
			RenderTexture.ReleaseTemporary(tmp0);
		}
	}
}
