#include "pch.h"

#include "Model.h"

#include <algorithm>
#include <filesystem>
#include <random>

Model::Model(std::filesystem::path path) : DirPath(path) { }

std::string Model::NextImagePath()
{
	if (Paths.empty())
		Refresh();

	std::string ret = Paths.front();
	Paths.pop_front();
	return ret;
}

void Model::CheckDir(std::filesystem::path path)
{
	for (const auto& entry : std::filesystem::directory_iterator(path))
	{
		if (entry.is_directory())
			CheckDir(entry.path());
		if (entry.is_regular_file())
			Paths.push_back(entry.path().string());
	}
}

void Model::Refresh()
{
	CheckDir(DirPath);

	std::random_device rd;
	std::mt19937 gen(rd());
	std::shuffle(Paths.begin(), Paths.end(), gen);
}