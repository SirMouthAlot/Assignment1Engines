#pragma once

#include <nlohmann/json.hpp>
#include <fstream>

namespace File
{
	//Default directory for files
	static std::string defaultDir = "./Assets/Levels/";

	//Load in JSON file
	nlohmann::json LoadJSON(std::string fileName);

	//Saves our JSON file
	void SaveJSON(std::string fileName, nlohmann::json j);
}