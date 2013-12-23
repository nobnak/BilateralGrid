using UnityEngine;
using System.Collections;

public class FullSize : MonoBehaviour {
	private Material _mat;

	// Use this for initialization
	void Start () {
		_mat = renderer.sharedMaterial;
		StartCoroutine(UpdateAspect());
	}
	
	IEnumerator UpdateAspect () {
		while (true) {
			yield return new WaitForSeconds(1f);
			var tex = _mat.mainTexture;
			if (tex == null)
				continue;

			var screenAspect = (float)Screen.width / Screen.height;
			transform.localScale = new Vector3(screenAspect, screenAspect * tex.height / tex.width, 1f);
		}
	}
}
