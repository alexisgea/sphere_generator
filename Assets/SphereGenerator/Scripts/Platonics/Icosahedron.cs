// Original source: https://github.com/alexisgea/sphere_generator and post: https://www.alexisgiard.com/icosahedron-sphere-remastered/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AlexisGea {
    public class Icosahedron : IPlatonicSolid {
        public List<Vector3> Vertices { private set; get; }
        public List<TriangleFace> Faces { private set; get; }
		public Vector3 NorthPole { private set; get; }


        public Icosahedron() {
			Vertices = CreateStartingVertices();
			Faces = CreateStartingFaces();
        }

		public List<Vector3> RemapVertices(List<Vector3> vertices, List<TriangleFace> faces) {
			// List<Vector3> originalVert = Vertices;
			// List<TriangleFace> checkedFaces = new List<TriangleFace>();
			// List<HexaFace> hexaFaces = new List<HexaFace>();
			// PintaFace[] pintaFaces = new PintaFace[12];
			// for(int i = 0; i < pintaFaces.Length; i ++) {
			// 	pintaFaces [i] = new PintaFace();
			// }

			// // first we identify the pinta faces (from the original vertices)
			// foreach(var face in faces) {
			// 	if(originalVert.Contains(vertices[face.IndA])) {
			// 		int index = originalVert.IndexOf(vertices[face.IndA]);
			// 		pintaFaces[index].AddFace(face, face.IndA);
			// 	}
			// 	else if(originalVert.Contains(vertices[face.IndB])) {
			// 		int index = originalVert.IndexOf(vertices[face.IndB]);
			// 		pintaFaces[index].AddFace(face, face.IndB);
			// 	}
			// 	else if(originalVert.Contains(vertices[face.IndC])) {
			// 		int index = originalVert.IndexOf(vertices[face.IndC]);
			// 		pintaFaces[index].AddFace(face, face.IndC);
			// 	}
			// }

			// // then we identify the hexa faces
			// while(checkedFaces.Count < faces.Count) {
			// 	foreach(var face in faces) {

			// 	}
			// }

			return vertices;
		}

		

		// create the 12 starting vertices of a icosahedron
		private List<Vector3> CreateStartingVertices() {
			List<Vector3> startingVert = new List<Vector3>();
			float phi = (1f + Mathf.Sqrt(5f)) / 2f;

			NorthPole = new Vector3(0f, phi, 0f);

			startingVert.Add(new Vector3(-1f, phi, 0f));
			startingVert.Add(new Vector3(1f, phi, 0f));
			startingVert.Add(new Vector3(-1f, -phi, 0f));
			startingVert.Add(new Vector3(1f, -phi, 0f));
	
			startingVert.Add(new Vector3(0f, -1f, phi));
			startingVert.Add(new Vector3(0f, 1f, phi));
			startingVert.Add(new Vector3(0f, -1f, -phi));
			startingVert.Add(new Vector3(0f, 1f, -phi));
	
			startingVert.Add(new Vector3(phi, 0f, -1f));
			startingVert.Add(new Vector3(phi, 0f, 1f));
			startingVert.Add(new Vector3(-phi, 0f, -1f));
			startingVert.Add(new Vector3(-phi, 0f, 1f));

			return startingVert;
		}

		// create the 20 starting triangle faces of the icosahedron
		private List<TriangleFace> CreateStartingFaces() {
			List<TriangleFace> startingFaces = new List<TriangleFace>();
	
			// 5 faces around point 0
			startingFaces.Add(new TriangleFace(0, 11, 5));
			startingFaces.Add(new TriangleFace(0, 5, 1));
			startingFaces.Add(new TriangleFace(0, 1, 7));
			startingFaces.Add(new TriangleFace(0, 7, 10));
			startingFaces.Add(new TriangleFace(0, 10, 11));
	
			// 5 adjacent faces 
			startingFaces.Add(new TriangleFace(1, 5, 9));
			startingFaces.Add(new TriangleFace(5, 11, 4));
			startingFaces.Add(new TriangleFace(11, 10, 2));
			startingFaces.Add(new TriangleFace(10, 7, 6));
			startingFaces.Add(new TriangleFace(7, 1, 8));
	
			// 5 faces around point 3
			startingFaces.Add(new TriangleFace(3, 9, 4));
			startingFaces.Add(new TriangleFace(3, 4, 2));
			startingFaces.Add(new TriangleFace(3, 2, 6));
			startingFaces.Add(new TriangleFace(3, 6, 8));
			startingFaces.Add(new TriangleFace(3, 8, 9));
	
			// 5 adjacent faces 
			startingFaces.Add(new TriangleFace(4, 9, 5));
			startingFaces.Add(new TriangleFace(2, 4, 11));
			startingFaces.Add(new TriangleFace(6, 2, 10));
			startingFaces.Add(new TriangleFace(8, 6, 7));
			startingFaces.Add(new TriangleFace(9, 8, 1));

			return startingFaces;
		}

		// private class PintaFace {
		// 	public List<TriangleFace> TriangleFaces { private set; get; }
		// 	public List<int> CenterVerticesIndex { private set; get; }
		// 	public Vector3 Center { set; get; }

		// 	public PintaFace() {
		// 		TriangleFaces = new List<TriangleFace>();
		// 	}

		// 	public void AddFace(TriangleFace face, int centerVert) {
		// 		if(TriangleFaces.Count >= 5){
		// 			Debug.LogError("too many face in PintaFace");
		// 			return;
		// 		}
		// 		TriangleFaces.Add(face);
		// 		CenterVerticesIndex.Add(centerVert);
		// 	}
		// }

		// private class HexaFace {
		// 	public List<TriangleFace> TriangleFaces { private set; get; }

		// 	public HexaFace() {
		// 		TriangleFaces = new List<TriangleFace>();
		// 	}

		// 	public void AddFace(TriangleFace face) {
		// 		if(TriangleFaces.Count >= 6){
		// 			Debug.LogError("too many face in PintaFace");
		// 			return;
		// 		}
		// 		TriangleFaces.Add(face);
		// 	}
		// }
    }
}

