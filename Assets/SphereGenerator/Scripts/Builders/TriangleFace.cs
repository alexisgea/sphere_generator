// Original source: https://github.com/alexisgea/sphere_generator and post: https://www.alexisgiard.com/icosahedron-sphere-remastered/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AlexisGea {
	/// <summary>
	/// Stores the indices of the vertices forming a triangle face.
	/// </summary>
	public class TriangleFace {
		public int IndA {set; get;}
		public int IndB {set; get;}
		public int IndC {set; get;}

		public TriangleFace(int indA, int indB, int indC) {
			IndA = indA;
			IndB = indB;
			IndC = indC;
		}

		public void SetIndA(int newIa) { IndA = newIa;}
		public void SetIndB(int newIb) { IndB = newIb;}
		public void SetIndC(int newIc) { IndC = newIc;}
	}
}
