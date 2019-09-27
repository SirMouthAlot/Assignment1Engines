#include "SaveLoad.h"

SaveLoad::SaveLoad()
{
	sceneFile = new nlohmann::json;
}

SaveLoad::~SaveLoad()
{
	if (sceneFile != nullptr)
	{
		delete sceneFile;
		sceneFile = nullptr;
	}
}

void SaveLoad::SaveFile(const char* name)
{
	File::SaveJSON(name, *sceneFile);
}

void SaveLoad::LoadFile(const char* name)
{
	*sceneFile = File::LoadJSON(name);
}

void SaveLoad::SaveScene()
{
	//Stores number of objects in scene first
	(*sceneFile)["numObjs"] = objectsInScene.size() - 1;

	for (int i = 0; i < objectsInScene.size(); i++)
	{
		//Loops and pushes every Object Info into the JSON file
		(*sceneFile)[std::to_string(i)] = objectsInScene[i];
	}
}

void SaveLoad::LoadScene()
{
	int counter = (*sceneFile)["numObjs"];

	for (int i = 0; i <= counter; i++)
	{
		//Loops and pulls out every Object Info from the JSON file
		objectsInScene.push_back((*sceneFile)[std::to_string(i)]);
	}
}

ObjectInfo SaveLoad::GetObject(int index)
{
	return objectsInScene[index];
}

int SaveLoad::GetObjectCount()
{
	return (int)objectsInScene.size();;
}

void SaveLoad::AddObject(ObjectInfo obj)
{
	objectsInScene.push_back(obj);
}

void SaveLoad::AddObject(float _position[3], float _rotation[3], int _type)
{
	Vector3 position = Vector3(_position[0], _position[1], _position[2]);
	Vector3 rotation = Vector3(_rotation[0], _rotation[1], _rotation[2]);

	objectsInScene.push_back(ObjectInfo(position, rotation, (ObjectType)_type));
}

void SaveLoad::ClearList()
{
	objectsInScene.clear();
}

