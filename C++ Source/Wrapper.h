#pragma once

#include "SaveLoad.h"

#ifdef __cplusplus
extern "C"
{
#endif
	//Saves the objects to the json object
	PLUGIN_API void SaveScene();
	//Saves the json object to the file
	PLUGIN_API void SaveFile(const char* name);
	
	//Loads the json object in from the file
	PLUGIN_API void LoadFile(const char* name);
	//Loads in the objects from the json object
	PLUGIN_API void LoadScene();

	//Add object to list
	PLUGIN_API int AddObject(float _position[3], float _rotation[3], int _type);

	//Get object position
	PLUGIN_API float GetObjectPosX(int index);
	PLUGIN_API float GetObjectPosY(int index);
	PLUGIN_API float GetObjectPosZ(int index);

	//Get object rotation
	PLUGIN_API float GetObjectRotX(int index);
	PLUGIN_API float GetObjectRotY(int index);
	PLUGIN_API float GetObjectRotZ(int index);

	//Gets the object type
	PLUGIN_API int GetObjectType(int index);

	//Gets the object count
	PLUGIN_API int GetObjectCount();

	//Clears out the list to prep for new save version
	PLUGIN_API void ClearList();

#ifdef __cplusplus
}
#endif