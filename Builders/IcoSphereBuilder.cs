// Original source: https://github.com/alexisgea/sphere_generator and post: https://www.alexisgiard.com/icosahedron-sphere-remastered/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace AlexisGea {
	/// <summary>
	/// Factory class to build the mesh of a subdivided Icosahedron adjusted to a sphere.
	/// </summary>
	public static class IcoSphereBuilder {

		/// <summary>
		/// Generate a icosphere by subdiving a base icosahedron.
		/// </summary>
		/// <param name="resolution">Number of subdivision, starting at 1 for a base icosahedron.</param>
		public static MeshData Generate(float radius, int resolution) {

			List<Vector3> vertices = new List<Vector3>();
			List< int > triangles = new List<int>();
			Dictionary<long, int> middlePointIndexCache = new Dictionary<long, int>();
			List<FaceVertIndices> faces = new List<FaceVertIndices>();
			Dictionary<int, int> vertWithWarpedU = new Dictionary<int, int>();
			Dictionary<int, float> poleVertIndicesCorrectU = new Dictionary<int, float>();

			Vector2[] uv;
			Vector3[] normals;
			Vector4[] tangents;

			Vector4 tangent = new Vector4(1f, 0f, 0f, -1f);

			// create the base icosahedron primitive
			vertices = CreateStartingVertices(radius);
			faces = CreateStartingFaces();
			
			// subdivide the triangles faces
			for (int i = 1; i < resolution; i++) {
				List<FaceVertIndices> faces2 = new List<FaceVertIndices>();
				foreach (var tri in faces) {
					// replace triangle by 4 triangles
					int a = GetMiddlePoint(tri.IndA, tri.IndB, ref vertices, ref middlePointIndexCache, radius);
					int b = GetMiddlePoint(tri.IndB, tri.IndC, ref vertices, ref middlePointIndexCache, radius);
					int c = GetMiddlePoint(tri.IndC, tri.IndA, ref vertices, ref middlePointIndexCache, radius);
	
					faces2.Add(new FaceVertIndices(tri.IndA, a, c));
					faces2.Add(new FaceVertIndices(tri.IndB, b, a));
					faces2.Add(new FaceVertIndices(tri.IndC, c, b));
					faces2.Add(new FaceVertIndices(a, b, c));
				}
				faces = faces2;
			}

			// fix warped uv on seam
			vertWithWarpedU = FindAndFixeWarpedFaces(faces, vertices);
			poleVertIndicesCorrectU = FindAndFixPoleVertices(faces, vertices, radius);

			// create mesh triangles
			for( int i = 0; i < faces.Count; i++ ) {
				triangles.Add( faces[i].IndA );
				triangles.Add( faces[i].IndB );
				triangles.Add( faces[i].IndC );
			}		

			// generate uv, normals, and tangents
			uv = new Vector2[vertices.Count];
			normals = new Vector3[vertices.Count];
			tangents = new Vector4[vertices.Count];

			for(int i = 0; i < vertices.Count; i++) {
				Vector3 n = vertices[i].normalized;
				float u = (Mathf.Atan2(n.z, n.x) / (2f * Mathf.PI)) + 0.5f; // remove the 0.5f
				float v = (Mathf.Asin(n.y) / Mathf.PI) + 0.5f;

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
				normals[i] = n;
				tangents[i] = tangent;
			}

			Debug.Log("Icosphere generated: " + triangles.Count + " tris and " + vertices.Count + " verts.");

            return new MeshData(vertices.ToArray(), triangles.ToArray(), uv, normals, tangents);
        }

		// create the 12 starting vertices of a icosahedron
		private static List<Vector3> CreateStartingVertices(float radius) {
			List<Vector3> startingVert = new List<Vector3>();
			float t = (1f + Mathf.Sqrt(5f)) / 2f;

			startingVert.Add(new Vector3(-1f,  t,  0f).normalized * radius);
			startingVert.Add(new Vector3( 1f,  t,  0f).normalized * radius);
			startingVert.Add(new Vector3(-1f, -t,  0f).normalized * radius);
			startingVert.Add(new Vector3( 1f, -t,  0f).normalized * radius);
	
			startingVert.Add(new Vector3( 0f, -1f,  t).normalized * radius);
			startingVert.Add(new Vector3( 0f,  1f,  t).normalized * radius);
			startingVert.Add(new Vector3( 0f, -1f, -t).normalized * radius);
			startingVert.Add(new Vector3( 0f,  1f, -t).normalized * radius);
	
			startingVert.Add(new Vector3( t,  0f, -1f).normalized * radius);
			startingVert.Add(new Vector3( t,  0f,  1f).normalized * radius);
			startingVert.Add(new Vector3(-t,  0f, -1f).normalized * radius);
			startingVert.Add(new Vector3(-t,  0f,  1f).normalized * radius);

			return startingVert;
		}

		// create the 20 starting triangle faces of the icosahedron
		private static List<FaceVertIndices> CreateStartingFaces() {
			List<FaceVertIndices> startingFaces = new List<FaceVertIndices>();
	
			// 5 faces around point 0
			startingFaces.Add(new FaceVertIndices(0, 11, 5));
			startingFaces.Add(new FaceVertIndices(0, 5, 1));
			startingFaces.Add(new FaceVertIndices(0, 1, 7));
			startingFaces.Add(new FaceVertIndices(0, 7, 10));
			startingFaces.Add(new FaceVertIndices(0, 10, 11));
	
			// 5 adjacent faces 
			startingFaces.Add(new FaceVertIndices(1, 5, 9));
			startingFaces.Add(new FaceVertIndices(5, 11, 4));
			startingFaces.Add(new FaceVertIndices(11, 10, 2));
			startingFaces.Add(new FaceVertIndices(10, 7, 6));
			startingFaces.Add(new FaceVertIndices(7, 1, 8));
	
			// 5 faces around point 3
			startingFaces.Add(new FaceVertIndices(3, 9, 4));
			startingFaces.Add(new FaceVertIndices(3, 4, 2));
			startingFaces.Add(new FaceVertIndices(3, 2, 6));
			startingFaces.Add(new FaceVertIndices(3, 6, 8));
			startingFaces.Add(new FaceVertIndices(3, 8, 9));
	
			// 5 adjacent faces 
			startingFaces.Add(new FaceVertIndices(4, 9, 5));
			startingFaces.Add(new FaceVertIndices(2, 4, 11));
			startingFaces.Add(new FaceVertIndices(6, 2, 10));
			startingFaces.Add(new FaceVertIndices(8, 6, 7));
			startingFaces.Add(new FaceVertIndices(9, 8, 1));

			return startingFaces;
		}

		// return index of point in the middle of p1 and p2
		private static int GetMiddlePoint(int p1, int p2, ref List<Vector3> vertices, ref Dictionary<long, int> cache, float radius) {
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
			vertices.Add( middle.normalized * radius ); 
	
			// store it, return index
			cache.Add(key, i);
	
			return i;
		}

		private static Dictionary<int, int> FindAndFixeWarpedFaces(List<FaceVertIndices> faces, List<Vector3> vertices) {
			List<FaceVertIndices> warpedFaces = new List<FaceVertIndices>();
			Dictionary<int, int> checkedVert = new Dictionary<int, int>();

			// find warped faces
			foreach(FaceVertIndices face in faces) {
				Vector3 coordA = GetUvCoordinates(vertices[face.IndA]);
				Vector3 coordB = GetUvCoordinates(vertices[face.IndB]);
				Vector3 coordC = GetUvCoordinates(vertices[face.IndC]);

				Vector3 texNormal = Vector3.Cross(coordB - coordA, coordC - coordA);
				if (texNormal.z > 0) { 
					warpedFaces.Add(face);
				}
			}

			// fix warped faces
			foreach(FaceVertIndices wFace in warpedFaces) {
				float xCoordA = GetUvCoordinates(vertices[wFace.IndA]).x;
				float xCoordB = GetUvCoordinates(vertices[wFace.IndB]).x;
				float xCoordC = GetUvCoordinates(vertices[wFace.IndC]).x;

				if(xCoordA <0.25f) {
					int newIa = wFace.IndA;
					if(!checkedVert.TryGetValue(wFace.IndA, out newIa)){
						vertices.Add(vertices[wFace.IndA]);
						checkedVert[wFace.IndA] = vertices.Count - 1;
						newIa = vertices.Count - 1;
					}
					wFace.ChangeIndiceA(newIa);
				}
				if(xCoordB <0.25f) {
					int newIb = wFace.IndB;
					if(!checkedVert.TryGetValue(wFace.IndB, out newIb)){
						vertices.Add(vertices[wFace.IndB]);
						checkedVert[wFace.IndB] = vertices.Count - 1;
						newIb = vertices.Count - 1;
					}
					wFace.ChangeIndiceB(newIb);
				}
				if(xCoordC <0.25f) {
					int newIc = wFace.IndC;
					if(!checkedVert.TryGetValue(wFace.IndC, out newIc)){
						vertices.Add(vertices[wFace.IndC]);
						checkedVert[wFace.IndC] = vertices.Count - 1;
						newIc = vertices.Count - 1;
					}
					wFace.ChangeIndiceC(newIc);
				}
			}

			return checkedVert;
		}

		//fix pole vertices incorrect U
		private static Dictionary<int, float> FindAndFixPoleVertices(List<FaceVertIndices> faces, List<Vector3> vertices, float radius) {
            Vector3 north = new Vector3(0, radius, 0);
            Vector3 south = new Vector3(0, -radius, 0);

            List<int> poleVerticeInd = new List<int>();
            Dictionary<int, float> poleVertIndicesCorrectU = new Dictionary<int, float>();

			foreach(FaceVertIndices face in faces) {
				if(vertices[face.IndA] == north || vertices[face.IndA] == south) {
					if(!poleVerticeInd.Contains(face.IndA)) {
                        poleVerticeInd.Add(face.IndA);
                    }
					else {
						vertices.Add(vertices[face.IndA] == north ? north : south);
						face.ChangeIndiceA(vertices.Count - 1);
					}
					float xCoordB = GetUvCoordinates(vertices[face.IndB]).x;
					float xCoordC = GetUvCoordinates(vertices[face.IndC]).x;
                    float correctedU = (xCoordB + xCoordC) / 2f + 0.5f; // I am not sure why it is needed but it seems needed...

                    poleVertIndicesCorrectU[face.IndA] = correctedU;
				}
			}

			return poleVertIndicesCorrectU;
		}

		private static Vector2 GetUvCoordinates(Vector3 vertice) {
			Vector3 vertCoord = vertice.normalized;
			float u = (Mathf.Atan2(vertCoord.z,vertCoord.x) / (2f * Mathf.PI));
			float v = (Mathf.Asin(vertCoord.y) / Mathf.PI) + 0.5f;

			return new Vector2(u, v);
		}

		/// <summary>
		/// Stores the indices of the vertices forming a triangle face.
		/// </summary>
		private class FaceVertIndices {
			public int IndA {private set; get;}
			public int IndB {private set; get;}
			public int IndC {private set; get;}
	
			public FaceVertIndices(int indA, int indB, int indC) {
				IndA = indA;
				IndB = indB;
				IndC = indC;
			}

			public void ChangeIndiceA(int newIa) { IndA = newIa;}
			public void ChangeIndiceB(int newIb) { IndB = newIb;}
			public void ChangeIndiceC(int newIc) { IndC = newIc;}
		}
	}
}
