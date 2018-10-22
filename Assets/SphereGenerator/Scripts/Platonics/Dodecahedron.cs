// Original source: https://github.com/alexisgea/sphere_generator and post: https://www.alexisgiard.com/icosahedron-sphere-remastered/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AlexisGea {
    public class Dodecahedron : IPlatonicSolid {
        public List<Vector3> Vertices { private set; get; }
        public List<TriangleFace> Faces { private set; get; }
		public Vector3 NorthPole { private set; get; }


        public Dodecahedron() {
			Vertices = CreateStartingVertices();
			Faces = CreateStartingFaces();
        }

		public List<Vector3> RemapVertices(List<Vector3> vertices, List<TriangleFace> faces) {
			return vertices;
		}

		private List<Vector3> CreateStartingVertices() {
			List<Vector3> startingVert = new List<Vector3>();
			float phi = (1f + Mathf.Sqrt(5f)) / 2f;

			NorthPole = Vector3.up;

			// center cube coordinates
			startingVert.Add(new Vector3(1f, 1f, 1f));			//0
			startingVert.Add(new Vector3(-1f, -1f, -1f));		//1
			startingVert.Add(new Vector3(-1f, 1f, 1f));			//2
			startingVert.Add(new Vector3(1f, -1f, 1f));			//3
			startingVert.Add(new Vector3(1f, 1f, -1f));			//4
			startingVert.Add(new Vector3(1f, -1f, -1f));		//5
			startingVert.Add(new Vector3(-1f, 1f, -1f));		//6
			startingVert.Add(new Vector3(-1f, -1f, 1f));		//7

			// yz plane coordinates
			startingVert.Add(new Vector3( 0f, 1f/phi, phi));	//8
			startingVert.Add(new Vector3( 0f, 1f/phi, -phi));	//9
			startingVert.Add(new Vector3( 0f, -1f/phi, phi));	//10
			startingVert.Add(new Vector3( 0f, -1f/phi, -phi));	//11

			// xy plnae coordinates
			startingVert.Add(new Vector3(1f/phi, phi, 0f));		//12
			startingVert.Add(new Vector3(-1f/phi, phi, 0f));	//13
			startingVert.Add(new Vector3(1f/phi, -phi, 0f));	//14
			startingVert.Add(new Vector3(-1f/phi, -phi, 0f));	//15

			// xz planet coordinates
			startingVert.Add(new Vector3(phi, 0f, 1f/phi));		//16
			startingVert.Add(new Vector3(-phi, 0f, 1f/phi));	//17
			startingVert.Add(new Vector3(phi, 0f, -1f/phi));	//18
			startingVert.Add(new Vector3(-phi, 0f, -1f/phi));	//19

			//vertex at the center of each face
			// Used these links and a lot of pen and papers
			// https://en.wikipedia.org/wiki/Regular_dodecahedron
			// https://math.stackexchange.com/questions/2244170/what-are-the-center-co%C3%B6rdinates-of-the-planes-of-a-regular-dodecahedron
			Vector3 centerFace1 = (startingVert[12] + startingVert[4] + startingVert[9] + startingVert[6] + startingVert[13]) / 5f;
			Vector3 centerFace2 = (startingVert[12] + startingVert[13] + startingVert[2] + startingVert[8] + startingVert[0]) / 5f;
			Vector3 centerFace3 = (startingVert[12] + startingVert[0] + startingVert[16] + startingVert[18] + startingVert[4]) / 5f;
			Vector3 centerFace4 = (startingVert[9] + startingVert[4] + startingVert[18] + startingVert[5] + startingVert[11]) / 5f;
			Vector3 centerFace5 = (startingVert[6] + startingVert[9] + startingVert[11] + startingVert[1] + startingVert[19]) / 5f;
			Vector3 centerFace6 = (startingVert[13] + startingVert[6] + startingVert[19] + startingVert[17] + startingVert[2]) / 5f;

			Vector3 centerFace7 = (startingVert[17] + startingVert[19] + startingVert[1] + startingVert[15] + startingVert[7]) / 5f;
			Vector3 centerFace8 = (startingVert[14] + startingVert[15] + startingVert[1] + startingVert[11] + startingVert[5]) / 5f;
			Vector3 centerFace9 = (startingVert[14] + startingVert[5] + startingVert[18] + startingVert[16] + startingVert[3]) / 5f;
			Vector3 centerFace10 = (startingVert[3] + startingVert[16] + startingVert[0] + startingVert[8] + startingVert[10]) / 5f;
			Vector3 centerFace11 = (startingVert[10] + startingVert[8] + startingVert[2] + startingVert[17] + startingVert[7]) / 5f;
			Vector3 centerFace12 = (startingVert[15] + startingVert[14] + startingVert[3] + startingVert[10] + startingVert[7]) / 5f;

			startingVert.Add(centerFace1); 						//20
			startingVert.Add(centerFace2); 						//21
			startingVert.Add(centerFace3); 						//22
			startingVert.Add(centerFace4); 						//23
			startingVert.Add(centerFace5); 						//24
			startingVert.Add(centerFace6); 						//25
			startingVert.Add(centerFace7); 						//26
			startingVert.Add(centerFace8); 						//27
			startingVert.Add(centerFace9); 						//28
			startingVert.Add(centerFace10); 					//29
			startingVert.Add(centerFace11); 					//30
			startingVert.Add(centerFace12); 					//31

			return startingVert;
		}

		private List<TriangleFace> CreateStartingFaces() {
			List<TriangleFace> startingFaces = new List<TriangleFace>();

			// face 1
			startingFaces.Add(new TriangleFace(20, 13, 12));
			startingFaces.Add(new TriangleFace(20, 12, 4));
			startingFaces.Add(new TriangleFace(20, 4, 9));
			startingFaces.Add(new TriangleFace(20, 9, 6));
			startingFaces.Add(new TriangleFace(20, 6, 13));

			// face 2
			startingFaces.Add(new TriangleFace(21, 13, 2));
			startingFaces.Add(new TriangleFace(21, 2, 8));
			startingFaces.Add(new TriangleFace(21, 8, 0));
			startingFaces.Add(new TriangleFace(21, 0, 12));
			startingFaces.Add(new TriangleFace(21, 12, 13));

			// face 3
			startingFaces.Add(new TriangleFace(22, 12, 0));
			startingFaces.Add(new TriangleFace(22, 0, 16));
			startingFaces.Add(new TriangleFace(22, 16, 18));
			startingFaces.Add(new TriangleFace(22, 18, 4));
			startingFaces.Add(new TriangleFace(22, 4, 12));

			// face 4
			startingFaces.Add(new TriangleFace(23, 4, 18));
			startingFaces.Add(new TriangleFace(23, 18, 5));
			startingFaces.Add(new TriangleFace(23, 5, 11));
			startingFaces.Add(new TriangleFace(23, 11, 9));
			startingFaces.Add(new TriangleFace(23, 9, 4));

			// face 5
			startingFaces.Add(new TriangleFace(24, 9, 11));
			startingFaces.Add(new TriangleFace(24, 11, 1));
			startingFaces.Add(new TriangleFace(24, 1, 19));
			startingFaces.Add(new TriangleFace(24, 19, 6));
			startingFaces.Add(new TriangleFace(24, 6, 9));

			// face 6
			startingFaces.Add(new TriangleFace(25, 6, 19));
			startingFaces.Add(new TriangleFace(25, 19, 17));
			startingFaces.Add(new TriangleFace(25, 17, 2));
			startingFaces.Add(new TriangleFace(25, 2, 13));
			startingFaces.Add(new TriangleFace(25, 13, 6));

			// face 7
			startingFaces.Add(new TriangleFace(26, 7, 17));
			startingFaces.Add(new TriangleFace(26, 17, 19));
			startingFaces.Add(new TriangleFace(26, 19, 1));
			startingFaces.Add(new TriangleFace(26, 1, 15));
			startingFaces.Add(new TriangleFace(26, 15, 7));

			// face 8
			startingFaces.Add(new TriangleFace(27, 15, 1));
			startingFaces.Add(new TriangleFace(27, 1, 11));
			startingFaces.Add(new TriangleFace(27, 11, 5));
			startingFaces.Add(new TriangleFace(27, 5, 14));
			startingFaces.Add(new TriangleFace(27, 14, 15));

			// face 9
			startingFaces.Add(new TriangleFace(28, 14, 5));
			startingFaces.Add(new TriangleFace(28, 5, 18));
			startingFaces.Add(new TriangleFace(28, 18, 16));
			startingFaces.Add(new TriangleFace(28, 16, 3));
			startingFaces.Add(new TriangleFace(28, 3, 14));

			// face 10
			startingFaces.Add(new TriangleFace(29, 3, 16));
			startingFaces.Add(new TriangleFace(29, 16, 0));
			startingFaces.Add(new TriangleFace(29, 0, 8));
			startingFaces.Add(new TriangleFace(29, 8, 10));
			startingFaces.Add(new TriangleFace(29, 10, 3));

			// face 11
			startingFaces.Add(new TriangleFace(30, 10, 8));
			startingFaces.Add(new TriangleFace(30, 8, 2));
			startingFaces.Add(new TriangleFace(30, 2, 17));
			startingFaces.Add(new TriangleFace(30, 17, 7));
			startingFaces.Add(new TriangleFace(30, 7, 10));

			// face 12
			startingFaces.Add(new TriangleFace(31, 15, 14));
			startingFaces.Add(new TriangleFace(31, 14, 3));
			startingFaces.Add(new TriangleFace(31, 3, 10));
			startingFaces.Add(new TriangleFace(31, 10, 7));
			startingFaces.Add(new TriangleFace(31, 7, 15));

			return startingFaces;
		}
    }
}
