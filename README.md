# How to use
Generate a Sphere in Unity, subdivided from platonic solids, in one click.

Currently included base solids:
- tetrahedron
- cube (with remaped vertices to minimise distortion)
- octahedron
- dodecahedron
- icosahedron
- warped plane (uv)

-*the cube, icosahedron and plane are the best looking-

To use it, add this folder to your Assets folder of your unity project. Then just add the SphereGenerator.cs script to an empty gameobject and "poof!", magic happens!

On the inspector you can select which basic shape should be used as well as setting the size and resolution. There is also an option to have a non-smoothed shape, which looks quite nice for the icosphere. Then press "generate" to update the mesh. The mesh will be generated from scratch on play.

Hope you find this useful.

Check the blog post I made about this for more details: https://www.alexisgiard.com/icosahedron-sphere/ && https://www.alexisgiard.com/icosahedron-sphere-remastered/

Source on https://github.com/alexisgea/sphere_generator
