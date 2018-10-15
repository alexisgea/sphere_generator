// Original source: https://github.com/alexisgea/sphere_generator and post: https://www.alexisgiard.com/icosahedron-sphere-remastered/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AlexisGea {
	/// <summary>
	/// Factory class to build a UV sphere (a warped uv plane).
	/// </summary>
	public static class UvSphereBuilder {
		/// <summary>
		/// Generate a UV sphere.
		/// </summary>
		/// <param name="resolution">Number of latitude lines (vertices on the y axis).</param>
		public static MeshData Generate(float radius, int resolution) {

			int vSize = 4 * resolution; // to make it closer to what the other have
			int uSize = vSize * 2;

			Vector3[] vertices = new Vector3[(uSize + 1) * (vSize + 1)];
			int[] triangles = new int[uSize * vSize * 6];
			Vector2[] uv = new Vector2[vertices.Length];
			Vector3[] normals = new Vector3[vertices.Length];
			Vector4[] tangents = new Vector4[vertices.Length];
			
			Vector4 tangent = new Vector4(1f, 0f, 0f, -1f);

            for (int i = 0, v = 0; v <= vSize; v++) {
				for (int u = 0; u <= uSize; u++, i++) {
					float theta = 2f * Mathf.PI * (float)u/uSize + Mathf.PI;
					float phi = Mathf.PI * (float)v/vSize;

					float x = Mathf.Cos(theta) * Mathf.Sin(phi) * radius;
					float y = -Mathf.Cos(phi) * radius;
					float z = Mathf.Sin(theta) * Mathf.Sin(phi) * radius;

					vertices[i] = new Vector3(x, y, z);
					uv[i] = new Vector2((float)u/uSize, (float)v/vSize);
					normals[i] = vertices[i].normalized;
					tangents[i] = tangent;
				}
			}

			for (int ti = 0, vi = 0, y = 0; y < vSize; y++, vi++) {
				for (int x = 0; x < uSize; x++, ti += 6, vi++) {
					triangles[ti] = vi;
					triangles[ti + 3] = triangles[ti + 2] = vi + 1;
					triangles[ti + 4] = triangles[ti + 1] = vi + uSize + 1;
					triangles[ti + 5] = vi + uSize + 2;
				}
			}

            return new MeshData(vertices, triangles, uv, normals, tangents);
        }
	}
}
