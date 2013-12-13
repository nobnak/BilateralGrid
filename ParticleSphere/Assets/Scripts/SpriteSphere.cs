using UnityEngine;
using System.Collections;

public class SpriteSphere : MonoBehaviour {
	public GameObject spherefab;
	public int nSpheres;

	void Start() {
		var hullRadius = Mathf.Pow(nSpheres, 0.3333f);
		for (var i = 0; i < nSpheres; i++) {
			var pos = hullRadius * Random.insideUnitSphere;
			var s = (GameObject)Instantiate(spherefab);
			s.transform.parent = transform;
			s.transform.localPosition = pos;
		}
	}
}
