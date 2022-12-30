#pragma once

#include <SDL.h>
#include <SDL_image.h>

#include <string>

enum ViewModelActions
{
	STATIC,
	TRANSITION,
};

class View;
class Model;
class ViewModel
{
public:
	ViewModel(Model&, int w, int h, SDL_Renderer* r);

	std::shared_ptr<View> NextView();

private:
	Model& Model;

	// What to draw
	std::string CurrentImagePath;
	std::string NextImagePath;

	// How to draw
	SDL_Renderer* Renderer;
	SDL_Rect TexRect;

	enum ViewModelActions Action;
	std::shared_ptr<View> ActionView;
	int SwitchTimer;
};