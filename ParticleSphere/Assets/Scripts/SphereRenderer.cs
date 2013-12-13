using UnityEngine;
using System.Collections;

public class SphereRenderer : MonoBehaviour {
	public const float DEG2RAD = Mathf.PI / 180f;

	public Material normal;
	void Start() {
		camera.hdr = true;

		UpdateFOV();
	}

	void OnRenderImage(RenderTexture src, RenderTexture dst) {
		Graphics.Blit(src, dst, normal);
	}

	void UpdateFOV () {
		var fovY = Mathf.Tan (camera.fieldOfView * DEG2RAD);
		var fovX = (float)Screen.width / Screen.height * fovY;
		normal.SetVector ("_Fov", new Vector4 (fovX, fovY, 0, 0));
	}
}
