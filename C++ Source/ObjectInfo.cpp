#include "ObjectInfo.h"

ObjectInfo::ObjectInfo(Vector3 _position, Vector3 _rotation, ObjectType _type)
{
	position = _position;
	rotation = _rotation;
	type = _type;
}

std::ostream& operator<<(std::ostream& out, const ObjectInfo& obj)
{
	out << "Position: " << obj.position;
	out << "Rotation: " << obj.rotation;
	out << "Type: " << obj.type << std::endl << std::endl;

	return out;
}
