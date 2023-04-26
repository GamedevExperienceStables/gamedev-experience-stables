Introduction
-------------------------------------------------
Yapp (Yet Another Prefab Painter) allows you to paint prefabs in the scene.

Supported Unity Versions: 2019+

Usage
-------------------------------------------------

General
	* right mouse button in Hierarchy -> Yapp -> Create PrefabPainter

	or

	* create container gameobject for the instantiated prefabs
	* create an editor gameobject, add the PrefabPainter script to it
	* in the inspector assign the container and a prefab to the prefab painter editor settings

	then

	* assign a container or create a new one by pressing the New button
	* drop prefabs into the prefab drop area
	* adjust prefab distribution settings

Paint Mode

	* in scene view adjust the radius via ctrl + mouse wheel
	* start painting prefabs by holding shift & dragging the mouse while holding the left mouse button

Spline Mode

	* add control points: shift + click
	* delete control points: shift + ctrl + click
	* press A to change the add (and delete) mode
		+ bounds: add (and delete) control points only to start and end control points
		+ inbetween: add (and delete) control points between control points

Operations

	* paint prefabs, as describe in General
	* hit "Run Simulation" in the Physics Settings
	
Delete

	* Click "Remove Container Children" to quickly remove the children of the current container

Important Notes
-------------------------------------------------
Templates
	+ when you change a template (prefab settings or template collection), you need to save your scene first or otherwise the changes won't be applied

Features in future updates
-------------------------------------------------	
+ rotation offset
+ lanes: directly on spline, left & right, multiple left & right
+ limit random scale in specific directions
+ limit rotation
+ consider prefab dimensions during distribution, align them next to each other
+ random distance during distribution
+ check if playmode is affected, maybe disable component when Application.isPlaying()
+ prefabmodule: serializedproperty in list


Credits
-------------------------------------------------

- Sebastian Lague
  
  Physics Simulation
  https://www.youtube.com/watch?v=SnhfcdtGM2E

  Patreon
  https://www.patreon.com/SebastianLague

- Kyle Halladay

  Spline
  http://kylehalladay.com/blog/tutorial/2014/03/30/Placing-Objects-On-A-Spline.html

- Fernando Zapata

  Interpolate.cs
  http://wiki.unity3d.com/index.php?title=Interpolate#Interpolate.cs

- Unity

  Getting the SerializedProperty from an object instead of a string. See example:
  https://github.com/Unity-Technologies/PostProcessing/blob/v2/PostProcessing/Editor/PostProcessVolumeEditor.cs

- Gregory Schlomoff

  Poisson Disc Sampling in Unity
  http://gregschlom.com/devlog/2014/06/29/Poisson-disc-sampling-Unity.html
