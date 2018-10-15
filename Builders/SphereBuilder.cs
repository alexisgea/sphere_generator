// Original source: https://github.com/alexisgea/sphere_generator and post: https://www.alexisgiard.com/icosahedron-sphere-remastered/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AlexisGea {
	/// <summary>
	/// Factory class to build a UV sphere (a warped uv plane).
	/// </summary>
	public static class SphereBuilder {
		/// <summary>
		/// Generate a UV sphere.
		/// </summary>
		/// <param name="resolution">Number of latitude lines (vertices on the y axis).</param>
		public static MeshData Build(IPlatonicSolid platonic, float radius, int resolution, bool smooth, bool remapVertices) {

			List<Vector3> vertices = new List<Vector3>();
			List< int > triangles = new List<int>();
			List<TriangleFace> faces = new List<TriangleFace>();
			Dictionary<long, int> middlePointIndexCache = new Dictionary<long, int>();
			Dictionary<int, int> vertWithWarpedU = new Dictionary<int, int>();
			Dictionary<int, float> poleVertIndicesCorrectU = new Dictionary<int, float>();

			Vector2[] uv;
			Vector3[] normals;
			Vector4[] tangents;

			Vector4 tangent = new Vector4(1f, 0f, 0f, -1f);

			// create the base cube primitive
			vertices = platonic.Vertices;
			faces = platonic.Faces;

			// subdivide the triangles faces
			for (int i = 1; i < resolution; i++) {
				List<TriangleFace> newFaces = new List<TriangleFace>();
				foreach (var tri in faces) {
					// replace triangle by 4 triangles
					int a = GetMiddlePoint(tri.IndA, tri.IndB, ref vertices, ref middlePointIndexCache);
					int b = GetMiddlePoint(tri.IndB, tri.IndC, ref vertices, ref middlePointIndexCache);
					int c = GetMiddlePoint(tri.IndC, tri.IndA, ref vertices, ref middlePointIndexCache);

					newFaces.Add(new TriangleFace(tri.IndA, a, c));
					newFaces.Add(new TriangleFace(tri.IndB, b, a));
					newFaces.Add(new TriangleFace(tri.IndC, c, b));
					newFaces.Add(new TriangleFace(a, b, c));
				}
				faces = newFaces;
			}

			// fix warped uv on seam
			vertWithWarpedU = FindAndFixeWarpedFaces(ref faces, ref vertices);
			poleVertIndicesCorrectU = FindAndFixPoleVertices(platonic, ref faces, ref vertices);

			// makes unique vertice to have a non smooth sphere
			if(!smooth) {
				MakeVerticesUnique(ref faces, ref vertices, ref vertWithWarpedU, ref poleVertIndicesCorrectU);
			}

			// create mesh triangles
			for( int i = 0; i < faces.Count; i++ ) {
				triangles.Add(faces[i].IndA);
				triangles.Add(faces[i].IndB);
				triangles.Add(faces[i].IndC);
			}

			if(remapVertices) {
				vertices = platonic.RemapVertices(vertices, faces);
			}

			// generate uv, normals, and tangents
			uv = new Vector2[vertices.Count];
			normals = new Vector3[vertices.Count];
			tangents = new Vector4[vertices.Count];

			for(int i = 0; i < vertices.Count; i++) {
				Vector3 normal = vertices[i].normalized;
				float u = (Mathf.Atan2(normal.z, normal.x) / (2f * Mathf.PI)) + 0.5f; // remove the 0.5f
				float v = (Mathf.Asin(normal.y) / Mathf.PI) + 0.5f;

				vertices[i] = normal * radius;

				// correct uv issues
				if(poleVertIndicesCorrectU.ContainsKey(i)) {
                    u = poleVertIndicesCorrectU[i];
                }

				if(vertWithWarpedU.ContainsValue(i)) {
					u += 1;
				}

				if(vertWithWarpedU.ContainsValue(i) && poleVertIndicesCorrectU.ContainsKey(i)) {
					u -= 0.5f; // found through trial and error, it was working so I had to remove the 0.5 added when recalculating
				}

				uv[i] = new Vector2(u, v);
				normals[i] = normal;
				tangents[i] = tangent;
			}

			// recompute normals
			if(!smooth) {
				foreach(TriangleFace face in faces) {
					Vector3 normal = GetFaceCenter(face, vertices);
					normals[face.IndA] = normal;
					normals[face.IndB] = normal;
					normals[face.IndC] = normal;
				}
			}

            return new MeshData(vertices.ToArray(), triangles.ToArray(), uv, normals, tangents);
        }


		// return index of point in the middle of p1 and p2
		private static int GetMiddlePoint(int p1, int p2, ref List<Vector3> vertices, ref Dictionary<long, int> cache) {
			// first check if we have it already
			bool firstIsSmaller = p1 < p2;
			long smallerIndex = firstIsSmaller ? p1 : p2;
			long greaterIndex = firstIsSmaller ? p2 : p1;
			long key = (smallerIndex << 32) + greaterIndex;

			int ret;
			if (cache.TryGetValue(key, out ret))
			{
				return ret;
			}

			// not in cache, calculate it
			Vector3 point1 = vertices[p1];
			Vector3 point2 = vertices[p2];
			Vector3 middle = new Vector3
			(
				(point1.x + point2.x) / 2f,
				(point1.y + point2.y) / 2f,
				(point1.z + point2.z) / 2f
			);

			// add vertex makes sure point is on unit sphere
			int i = vertices.Count;
			vertices.Add(middle);

			// store it, return index
			cache.Add(key, i);

			return i;
		}

		private static Dictionary<int, int> FindAndFixeWarpedFaces(ref List<TriangleFace> faces, ref List<Vector3> vertices) {
			List<TriangleFace> warpedFaces = new List<TriangleFace>();
			Dictionary<int, int> checkedVert = new Dictionary<int, int>();

			// find warped faces
			foreach(TriangleFace face in faces) {
				Vector3 coordA = GetUvCoordinates(vertices[face.IndA]);
				Vector3 coordB = GetUvCoordinates(vertices[face.IndB]);
				Vector3 coordC = GetUvCoordinates(vertices[face.IndC]);

				Vector3 texNormal = Vector3.Cross(coordB - coordA, coordC - coordA);
				if (texNormal.z > 0) {
					warpedFaces.Add(face);
				}
			}

			// fix warped faces
			foreach(TriangleFace wFace in warpedFaces) {
				float xCoordA = GetUvCoordinates(vertices[wFace.IndA]).x;
				float xCoordB = GetUvCoordinates(vertices[wFace.IndB]).x;
				float xCoordC = GetUvCoordinates(vertices[wFace.IndC]).x;

				if(xCoordA < 0.25f) {
					int newIa = wFace.IndA;
					if(!checkedVert.TryGetValue(wFace.IndA, out newIa)){
						vertices.Add(vertices[wFace.IndA]);
						checkedVert[wFace.IndA] = vertices.Count - 1;
						newIa = vertices.Count - 1;
					}
					wFace.SetIndA(newIa);
				}
				if(xCoordB < 0.25f) {
					int newIb = wFace.IndB;
					if(!checkedVert.TryGetValue(wFace.IndB, out newIb)){
						vertices.Add(vertices[wFace.IndB]);
						checkedVert[wFace.IndB] = vertices.Count - 1;
						newIb = vertices.Count - 1;
					}
					wFace.SetIndB(newIb);
				}
				if(xCoordC < 0.25f) {
					int newIc = wFace.IndC;
					if(!checkedVert.TryGetValue(wFace.IndC, out newIc)){
						vertices.Add(vertices[wFace.IndC]);
						checkedVert[wFace.IndC] = vertices.Count - 1;
						newIc = vertices.Count - 1;
					}
					wFace.SetIndC(newIc);
				}
			}

			return checkedVert;
		}

		//fix pole vertices incorrect U
		private static Dictionary<int, float> FindAndFixPoleVertices(IPlatonicSolid solid, ref List<TriangleFace> faces, ref List<Vector3> vertices) {
            Vector3 north = solid.NorthPole;
            Vector3 south = -solid.NorthPole;

            List<int> poleVerticeInd = new List<int>();
            Dictionary<int, float> poleVertIndicesCorrectU = new Dictionary<int, float>();

			foreach(TriangleFace face in faces) {
				if(vertices[face.IndA] == north || vertices[face.IndA] == south) {
					if(!poleVerticeInd.Contains(face.IndA)) {
                        poleVerticeInd.Add(face.IndA);
                    }
					else {
						vertices.Add(vertices[face.IndA] == north ? north : south);
						face.SetIndA(vertices.Count - 1);
					}
					float xCoordB = GetUvCoordinates(vertices[face.IndB]).x;
					float xCoordC = GetUvCoordinates(vertices[face.IndC]).x;
                    float correctedU = (xCoordB + xCoordC) / 2f + 0.5f; // I am not sure why it is needed but it seems needed...

                    poleVertIndicesCorrectU[face.IndA] = correctedU;
				}
			}

			return poleVertIndicesCorrectU;
		}

		private static Vector3 GetFaceCenter(TriangleFace face, List<Vector3> vertices) {
			return (vertices[face.IndA] + vertices[face.IndB] + vertices[face.IndC]) / 3f;
		}

		private static Vector2 GetUvCoordinates(Vector3 vertice) {
			Vector3 vertCoord = vertice.normalized;
			float u = (Mathf.Atan2(vertCoord.z,vertCoord.x) / (2f * Mathf.PI));
			float v = (Mathf.Asin(vertCoord.y) / Mathf.PI) + 0.5f;

			return new Vector2(u, v);
		}

		private static void MakeVerticesUnique(ref List<TriangleFace> faces, ref List<Vector3> vertices,
		ref Dictionary<int, int> vertWithWarpedU, ref Dictionary<int, float> poleVertIndicesCorrectU) {
			List<int> vertCache = new List<int>();
				foreach(TriangleFace face in faces) {
					if(vertCache.Contains(face.IndA)) {
						Vector3 newVert = vertices[face.IndA];
						vertices.Add(newVert);

						if(vertWithWarpedU.ContainsValue(face.IndA)) {
							vertWithWarpedU[vertices.Count - 1] = vertices.Count - 1;
						}

						if(poleVertIndicesCorrectU.ContainsKey(face.IndA)) {
							poleVertIndicesCorrectU[vertices.Count -1] = poleVertIndicesCorrectU[face.IndA];
						}

						face.SetIndA(vertices.Count - 1);
					}

					if(vertCache.Contains(face.IndB)) {
						Vector3 newVert = vertices[face.IndB];
						vertices.Add(newVert);
						
						if(vertWithWarpedU.ContainsValue(face.IndB)) {
							vertWithWarpedU[vertices.Count - 1] = vertices.Count - 1;
						}

						if(poleVertIndicesCorrectU.ContainsKey(face.IndB)) {
							poleVertIndicesCorrectU[vertices.Count -1] = poleVertIndicesCorrectU[face.IndB];
						}
						
						face.SetIndB(vertices.Count - 1);
					}

					if(vertCache.Contains(face.IndC)) {
						Vector3 newVert = vertices[face.IndC];
						vertices.Add(newVert);
						
						if(vertWithWarpedU.ContainsValue(face.IndC)) {
							vertWithWarpedU[vertices.Count - 1] = vertices.Count - 1;
						}

						if(poleVertIndicesCorrectU.ContainsKey(face.IndC)) {
							poleVertIndicesCorrectU[vertices.Count -1] = poleVertIndicesCorrectU[face.IndC];
						}
						
						face.SetIndC(vertices.Count - 1);
					}

					vertCache.Add(face.IndA);
					vertCache.Add(face.IndB);
					vertCache.Add(face.IndC);
				}
		}
	}
}
