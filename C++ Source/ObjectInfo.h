#pragma once

#include "PluginSettings.h"

#include "Vector.h"
#include "File.h"

enum PLUGIN_API ObjectType
{
	GREENCUBE,
	PURPLECUBE,
	REDCUBE,
	YELLOWCUBE,
	BLUECUBE,
};

struct PLUGIN_API ObjectInfo
{
public:
	ObjectInfo() { };
	ObjectInfo(Vector3 _position, Vector3 _rotation, ObjectType _type);

	Vector3 position = Vector3();
	Vector3 rotation = Vector3();
	ObjectType type = GREENCUBE;

	friend std::ostream& operator<<(std::ostream& out, const ObjectInfo& obj);
};

//Sends Object Info TO json file
inline void to_json(nlohmann::json& j, const ObjectInfo& obj)
{
	j["Position"] = { obj.position.x, obj.position.y, obj.position.z };
	j["Rotation"] = { obj.rotation.x, obj.rotation.y, obj.rotation.z };
	j["Type"] = (int)obj.type;
}

//Reads Object Info in FROM json file
inline void from_json(const nlohmann::json& j, ObjectInfo& obj)
{
	obj.position = Vector3(j["Position"][0], j["Position"][1], j["Position"][2]);
	obj.rotation = Vector3(j["Rotation"][0], j["Rotation"][1], j["Rotation"][2]);
	obj.type = (ObjectType)j["Type"];
}