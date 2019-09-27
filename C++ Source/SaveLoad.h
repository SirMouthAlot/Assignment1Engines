#pragma once

#include "ObjectInfo.h"

class PLUGIN_API SaveLoad
{
public:
	SaveLoad();
	~SaveLoad();

	void SaveFile(const char* name);
	void LoadFile(const char* name);

	void SaveScene();
	void LoadScene();
	
	void AddObject(ObjectInfo obj);
	void AddObject(float _position[3], float _rotation[3], int _type);
	ObjectInfo GetObject(int index);

	int GetObjectCount();

	void ClearList();
private:
	std::vector<ObjectInfo> objectsInScene;

	nlohmann::json* sceneFile = nullptr;
};