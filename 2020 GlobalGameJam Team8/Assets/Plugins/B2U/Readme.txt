# ------------------------------------------------------------------
# B2Unity 2.0
# Cogumelo Softworks - www.cogumelosoftworks.com
# B2U Reference Docs - http://cogumelosoftworks.com/b2udocs
# ------------------------------------------------------------------

B2Unity is a import/export system that provide a more 
streamlined way to export/import models, prefab librarys and/or
complete scenes from blender to Unity

# -----------------------------------------------------------------
# INSTALLING
# -----------------------------------------------------------------

Step 1 - Unity Importer:
Make sure the B2U unitypackage was imported at your unity project. The final path would like "Assets/B2U/"

Step 2 - Blender Exporter:
Go to File/User Preferences/Add-ons and Click in "Install Add-on From File" 
Set the path to the "b2uexporter.zip" file that is inside the "Assets/B2U" folder, install and set it active in blender.

# -----------------------------------------------------------------
# RELEASE LOGS
# -----------------------------------------------------------------

v2.0
	- New core to Support to Blender 2.8x
	- New importer system to Support Unity 2019.3
	- Support to Build-In, HDRP and URP Shaders.
	- Now Based on Eevee for Blender's Previews
	- Unified material setup at the Principled in Eevee
	- Transparency Settings are now based on Eevee Settings for Build-in Pipeline
	- Fixed many bugs and new UI improvements

v1.31 Hotfix
	- Fix bug in group data import/export

v1.3
    	- New documentation website: http://cogumelosoftworks.com/b2udocs
	- It's compatible with Unity 2018.3. Due the many changes in the material and prefab system it's not compatible with Unity 2018.2x anymore
	
	- Implements the new Unity's Prefab system. Now Scenes and Groups implement nested prefabs!
	- Auto Reimport Scene and Groups instanciated
	- B2U Material System was redone from the ground up. We received many requests from users about implement a more streamlined workflow instead the old interface based converter. Now B2U Materials are much more compact in settings but work a lot better! For this first version we will suport the most compatible and usefull materials. More will be added in future. This will allow us too keep our plans to make eevee support soon as possible.
	- Colors are now exported as Hex sRGB (Gamma Corrected) so you will get much more close results from Cycles to Unity materials.
	- Increse the Area Margin for auto generated UVMaps to 10 to fix problems with color bleeding
	- Fixed many bugs and general cleanup the code


v1.2
    - Better support to Unity 2018.1
    - Force External Materials (Legacy Mode) on Import Models
    - Rework the Color Interface to Support RGBA
    - Fix typo in "Default Layer"
    - Fix Bugs copying "Non Packed" Textures of Blender
    - Fix Bugs exporting "Unlit Texture" Materials
    - Rework the PBR Node to use the Standard (Roughness Setup)
    - UV Repeat Bug seems fixed in Blender 2.79b and Unity 2018.1

v1.1
	- Automatically convert relative to absolute paths
	- Added Inicial (Experimental) Support to BI Node materials
	- Better UI Tooltips and Warnings
	- Many more handlers for exceptions when using BI materials, lights and others.
	- Better Package Organization

v1.0 
	- Pre Material nodes and custom node interfaces
	- New folder struct system
	- New Scene Reconstruction System
	- New UI polish and context menus
	- Bug Fix in many areas as Group Import, Light, Material and Textures
	- Added support to parented Ojects in Group Import
	- Only export visible objects (Visible layers and non hided objects)

v0.43
	- Fixed bugs in Direct Node interfaces

v0.42
	- New material system based on nodes
	- New lamp objects based on nodes
	- New configurable prefix system
	- New group import system
	- UI Fixes and Save Editor Data

v0.41
	- UI Cleanup

v0.40
	- New Unified path for MetaDatas
	- Support to configurable MetaData Paths, Default is B2U/MetaData
	- Added Dir Button to Path Selectors in Blender
	- Faster export for shared packed textures

v0.30
	- Removed all warnings in the importer
	- Bug Fix : bug when using whitespace in material names
	- Bug Fix : Foward and Back Slashes in material names cause crash in the exporter. Hardcoded Replaced by "_"
	- Bug Fix bugs in Unity Window Editor
	- Improved support to diffuse color and alplha
	- Save texture format for blender internal textures is now configured in the exporter.

v0.26
	- Better behiavor for material reimport
	- New Support to group export/import as prefabs
	- New Support to ortographic camera size

v0.25
	- Better UI for import scene.
	- New Import Core in C#
	- New : "Create Folder" is a new Option that automatically create folders in paths with the Manual Path Mode.
	- Bug Fix : Bug when export material with texture and no image assign
	- Bug Fix : Fixed Bug in Object panel trying to draw a Collider prop.
	- Bug Fix : Fixed Bug in Reports with path check without "Automatic Folder Struct"
	- Bug Fix : fixed Bug in Paths when finish with "/" or "\". Now trying to always use fowardslash
	- Bug Fix : Not valid Scene path is a showstopper only if in Scene export mode.
	- Bug Fix: Only Meshes have Prefab and Object Parameters for now. fixed Panel
	- Better icons and texts for Report Folder Operations
	- Better icons and texts for Material Operations
	- Better Tool-tips for Path Fields

v0.24
	- Added support to set Collisions
	- Now Settings are Per Prefab and Per Object
		- Static : Per Prefab and Per Object
		- Layer : Per Prefab and Per Object
		- Tag : Per Prefab and Per Object
		- Colliders : Per Prefab
		- Export : Per Object

	- Better support to Camera
		- New FOV correction
		- Rotations now fixed but with yet problems when Y != 0

v0.23
	- Added support to Tag
	- Layer Settings
	- Added support to Set Static in prefab (not only on scene importer)
	- Material export autofixes:
		- On the start it will clean all non used material slots
		- If you have no material on your object it will create a B2UDefaultMaterial and export it
		- Is not recommended to use the B2UDefaultMaterial for commom usage since it need to be reseted sometimes. 
		- It's just a organized Placeholder better than the "No Name" material that Unity create.

v0.22
	- Added support to Lamp export/import, current supported features:
		- Lamp Transform (Position and Rotation)
		- Lamp Type : Point, Sun, Spot
		- Lamp Color, Energy and Distance
		- Lamp Spot Size ( Spot Only )
	- Added support to export models with modifiers
		- Hardcoded to use the VIEW paremeters, will export as see in the viewport.
	- Huge Texture Import/Export improvments
		- Added support to export packed and Generated textures
			- Save it as png and rename it to .png format
		- Added support to export/Import Normal Maps
		- Added support to export/Import Emit Maps
		- Added support to enable/disable textures

v0.21
	- New Prefab/Scene Export Mode Selection
	- New AutoGenerate Folder Structs
	- Fixed problems with backslash/fowardslash paths
	- New UI Panel

v0.20
	- Fixed Rotation problems
	- Fixed Multi Material export
	- Added initial support to Texture export
	- Removed per object Material export, now it's Global in Export Settings

v0.10
	- Initial Import/Export for Instanciated Objects
	- Fix Scale and Rotations during import/export
	- Initial Import/Export for Scene
	- Export in Blender's Render Panel
	- Initial support to Material Export