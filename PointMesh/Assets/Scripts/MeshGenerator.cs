using UnityEngine;
using System.Collections.Generic;

public class MeshGenerator : MonoBehaviour {
	public const int VERTEX_LIMIT = 65536;
	public Material src;

	private List<Mesh> _meshes;

	// Use this for initialization
	void Start () {
		Generate();
	}

	void OnDestroy() {
		foreach (var m in _meshes)
			Destroy(m);
	}

	void Generate() {
		_meshes = new List<Mesh>();
		var tex = src.mainTexture;

		var width = tex.width;
		var height = tex.height;
		var nRowsInMesh = VERTEX_LIMIT / width;
		var texelSize = new Vector2(1f / width, 1f / height);
		var texelOffset = 0.5f * texelSize;

		for (var i = 0; i < height; i+=nRowsInMesh) {
			var nActualRows = Mathf.Min(nRowsInMesh, height - i);
			var size = nActualRows * width;
			var vertices = new Vector3[size];
			var indices = new int[size];
			var uvs = new Vector2[size];
			var counter = 0;
			for (var yOffset = 0; yOffset < nActualRows; yOffset++) {
				for (var x = 0; x < width; x++) {
					var y = yOffset + i;
					vertices[counter] = new Vector3(x, y, 0f);
					indices[counter] = x + yOffset * width;
					uvs[counter] = new Vector2(x * texelSize.x, y * texelSize.y) + texelOffset;
					counter++;
				}
			}
			var m = new Mesh();
			m.vertices = vertices;
			m.SetIndices(indices, MeshTopology.Points, 0);
			m.uv = uvs;
			m.bounds = new Bounds(Vector3.zero, 1000f * Vector3.one);
			_meshes.Add(m);
			var child = new GameObject();
			var mf = child.AddComponent<MeshFilter>();
			var r = child.AddComponent<MeshRenderer>();
			mf.mesh = m;
			r.sharedMaterial = src;
			child.transform.parent = transform;
			child.layer = gameObject.layer;
		}
	}
}
