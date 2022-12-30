#pragma once

#include <deque>
#include <filesystem>
#include <string>

class Model
{
public:
	Model(std::filesystem::path path);

	std::string NextImagePath();

protected:
	void Refresh();

private:
	void CheckDir(std::filesystem::path path);

	std::filesystem::path DirPath;
	std::deque<std::string> Paths;
};