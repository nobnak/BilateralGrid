using UnityEngine;
using System.Collections;

public class MainCameraRenderer : MonoBehaviour {
	public const float DEG2RAD = Mathf.PI / 180f;

	public GameObject normalView;

	private SpriteSphereRenderer _sphere;
	private SpriteSphereRenderer.Result _sphereResult;

	// Use this for initialization
	void Start () {
		_sphere = (SpriteSphereRenderer)FindObjectOfType(typeof(SpriteSphereRenderer));

		var height = 2f * camera.farClipPlane * Mathf.Tan(0.5f * camera.fieldOfView * DEG2RAD);
		var width = height * (float)Screen.width / Screen.height;
		var scale = new Vector3(width, height, 1f);
		normalView.transform.localScale = scale;
		normalView.transform.position = (camera.farClipPlane - 1e-3f) * camera.transform.forward + camera.transform.position;
		normalView.transform.forward = camera.transform.forward;
	}

	void OnPreRender() {
		_sphereResult = _sphere.GetResult ();
		normalView.renderer.sharedMaterial.mainTexture = _sphereResult.normal;
	}
}
