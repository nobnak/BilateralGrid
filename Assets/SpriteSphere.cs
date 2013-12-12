using UnityEngine;
using System.Collections;

public class SpriteSphere : MonoBehaviour {
	private Mesh _mesh;

	void Start () {
		_mesh = new Mesh();
		var vertices = new Vector3[]{ Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero };
		var triangles = new int[]{ 0, 3, 1, 0, 2, 3 };
		var uv = new Vector2[]{ new Vector2(0f, 0f), new Vector2(1f, 0f), new Vector2(0f, 1f), new Vector2(1f, 1f) };
		var normals = new Vector3[]{ Vector3.back, Vector3.back, Vector3.back, Vector3.back };

		_mesh.vertices = vertices;
		_mesh.triangles = triangles;
		_mesh.uv = uv;
		_mesh.normals = normals;
		_mesh.RecalculateBounds();

		GetComponent<MeshFilter>().mesh = _mesh;
	}

	void OnDestroy() {
		Destroy(_mesh);
	}
}
