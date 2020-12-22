using UnityEngine;
using UnityEngine.Rendering;
using UnityEditor;

public class PlaceTreeShadowCasters
{

	//[@MenuItem("Terrain/Place Tree Shadow Casters")]
	//static void SratGovnom()
	//{
	//	Terrain[] terrains = Terrain.activeTerrains;
		
	//	foreach (Terrain terrain in terrains)
	//	{
	//		TerrainData td = terrain.terrainData;

	//		GameObject parent = new GameObject("Tree Shadow Casters");
	//		foreach (TreeInstance tree in td.treeInstances)
	//		{
	//			Vector3 pos = Vector3.Scale(tree.position, td.size) + terrain.transform.position;

	//			TreePrototype treeProt = td.treePrototypes[tree.prototypeIndex];
	//			GameObject prefab = treeProt.prefab;

	//			GameObject obj = Object.Instantiate(prefab, pos, Quaternion.AngleAxis(tree.rotation, Vector3.up)) as GameObject;
	//			MeshRenderer renderer = obj.GetComponent<MeshRenderer>();
	//			renderer.receiveShadows = false;
	//			renderer.shadowCastingMode = ShadowCastingMode.On;
	//			GameObjectUtility.SetStaticEditorFlags(obj, StaticEditorFlags.ContributeGI);
	//			Debug.Log("here");
	//			Transform t = obj.transform;
	//			t.localScale = new Vector3(tree.widthScale, tree.heightScale, tree.widthScale);
	//			t.parent = parent.transform;
	//		}
	//	}
	//}
}
