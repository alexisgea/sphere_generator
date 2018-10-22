// Original source: https://github.com/alexisgea/sphere_generator and post: https://www.alexisgiard.com/icosahedron-sphere-remastered/

using UnityEngine;

namespace AlexisGea {
	/// <summary>
	/// Store mesh data for procedural primitive generation.
	/// </summary>
	public class MeshData {
		public Vector3[] Vertices { private set; get; }
		public int[] Triangles { private set; get; }
		public Vector2[] Uv { private set; get; }
		public Vector3[] Normals {private set; get; }
		public Vector4[] Tangents { private set; get; }

		public MeshData(Vector3[] verts, int[] tris, Vector2[] uv, Vector3[] normals, Vector4[] tans) {
			Vertices = verts;
			Triangles = tris;
			Uv = uv;
			Normals = normals;
            Tangents = tans;
        }
	}
}
