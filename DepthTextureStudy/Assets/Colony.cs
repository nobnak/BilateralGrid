using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class Colony : MonoBehaviour {
	public GameObject cellfab;
	public int nCells;

	private GameObject[] _cells;

	void Update() {
		if (_cells != null && _cells.Length == nCells)
			return;
		if (_cells.Length != nCells)
			ClearCells();

		_cells = new GameObject[nCells];
		var radius = Mathf.Pow(nCells, 0.333333f);
		for (var i = 0; i < nCells; i++) {
			var pos = radius * Random.insideUnitSphere;
			var cell = (GameObject)Instantiate(cellfab);
			cell.transform.parent = transform;
			cell.transform.localPosition = pos;
			_cells[i] = cell;
		}
	}

	void OnDestroy() {
		ClearCells ();
	}

	void ClearCells () {
		if (_cells == null)
			return;
		foreach (var tr in _cells) {
			if (tr == transform)
				continue;
			if (Application.isEditor)
				DestroyImmediate(tr.gameObject);
			else
				Destroy(tr.gameObject);
		}
		_cells = null;

	}
}
