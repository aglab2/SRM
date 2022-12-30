// ImageSlideshow2.cpp : This file contains the 'main' function. Program execution begins and ends there.
//

#include "pch.h"
#include <iostream>

#include <SDL.h>
#include <SDL_image.h>

#include <Windows.h>

#include "config.h"

#include "View.h"
#include "Model.h"
#include "ViewModel.h"

std::filesystem::path GetFilesPath()
{
	std::string path;
	for (int i = 32; ; i *= 2)
	{
		path = std::string(i, 0);
		
		if (GetModuleFileNameA(NULL, path.data(), i) && GetLastError() != ERROR_INSUFFICIENT_BUFFER)
			break;
	}

	std::filesystem::path exePath(path);
	return exePath.parent_path().append("_OUTPUT");
}

int main(int argc, char *argv[])
{
	Model model(GetFilesPath());

	SDL_Window *win = NULL;
	SDL_Renderer *renderer = NULL;
	SDL_Texture *img = NULL;

	// Initialize SDL.
	if (SDL_Init(SDL_INIT_VIDEO) < 0)
		return 1;

	win = SDL_CreateWindow("Image Slideshow", 100, 100, WIDTH, HEIGHT, 0);
	renderer = SDL_CreateRenderer(win, -1, SDL_RENDERER_ACCELERATED);

	ViewModel vm(model, WIDTH, HEIGHT, renderer);

	while (1) {
		unsigned int start = SDL_GetTicks();
		{
			SDL_Event e;
			if (SDL_PollEvent(&e)) {
				if (e.type == SDL_QUIT)
					break;
				else if (e.type == SDL_KEYUP && e.key.keysym.sym == SDLK_ESCAPE)
					break;
			}

			vm.NextView()->Draw();
		}
		unsigned int end = SDL_GetTicks();
		unsigned int elapsed = end - start;
		if (elapsed < MS_PER_FRAME)
			SDL_Delay(MS_PER_FRAME - elapsed);
	}

	SDL_DestroyTexture(img);
	SDL_DestroyRenderer(renderer);
	SDL_DestroyWindow(win);

	return 0;
}