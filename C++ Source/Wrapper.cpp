#include "Wrapper.h"

//Our saveload object
SaveLoad sceneEdit;

void SaveScene()
{
	return sceneEdit.SaveScene();
}

void SaveFile(const char* name)
{
	return sceneEdit.SaveFile(name);
}

void LoadFile(const char* name)
{
	return sceneEdit.LoadFile(name);
}

void LoadScene()
{
	return sceneEdit.LoadScene();
}

int AddObject(float _position[3], float _rotation[3], int _type)
{
	sceneEdit.AddObject(_position, _rotation, _type);

	return 12;
}

float GetObjectPosX(int index)
{
	return sceneEdit.GetObject(index).position.x;
}

float GetObjectPosY(int index)
{
	return sceneEdit.GetObject(index).position.y;
}

float GetObjectPosZ(int index)
{
	return sceneEdit.GetObject(index).position.z;
}

float GetObjectRotX(int index)
{
	return sceneEdit.GetObject(index).rotation.x;
}

float GetObjectRotY(int index)
{
	return sceneEdit.GetObject(index).rotation.y;
}

float GetObjectRotZ(int index)
{
	return sceneEdit.GetObject(index).rotation.z;
}

int GetObjectType(int index)
{
	return (int)sceneEdit.GetObject(index).type;
}

int GetObjectCount()
{
	return sceneEdit.GetObjectCount();
}

void ClearList()
{
	return sceneEdit.ClearList();
}
