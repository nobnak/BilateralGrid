using UnityEngine;
using UnityEditor;
using System.Collections;

public class SpriteMeshGen {
	[MenuItem("Assets/Custom/SpriteMesh")]
	public static void GenSpriteMesh() {
		var mesh = new Mesh();
		var vertices = new Vector3[]{ Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero };
		var triangles = new int[]{ 0, 3, 1, 0, 2, 3 };
		var uv = new Vector2[]{ new Vector2(0f, 0f), new Vector2(1f, 0f), new Vector2(0f, 1f), new Vector2(1f, 1f) };
		var normals = new Vector3[]{ Vector3.back, Vector3.back, Vector3.back, Vector3.back };
		
		mesh.vertices = vertices;
		mesh.triangles = triangles;
		mesh.uv = uv;
		mesh.normals = normals;
		mesh.bounds = new Bounds(Vector3.zero, new Vector3(2f, 2f, 0f));

		AssetDatabase.CreateAsset(mesh, "Assets/Generated/Sprite.asset");
	}
}
