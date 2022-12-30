#pragma once

#include <SDL.h>
#include <SDL_image.h>

#include <string>

class View
{
public:
	virtual ~View() {}

	virtual void Advance() = 0;
	virtual void Draw() = 0;

	//virtual void Advance() = 0;

protected:
	View(SDL_Renderer*, SDL_Rect*);

	SDL_Renderer* Renderer;
	SDL_Rect* TexRect;

	SDL_Texture* CreateTexture(std::string& path);
};

struct StaticView : public View
{
public:
	virtual ~StaticView();
	StaticView(SDL_Renderer* r, SDL_Rect* t, std::string path);

	virtual void Advance();
	virtual void Draw();

private:
	SDL_Texture* Img;
};

struct TransitionView : public View
{
public:
	virtual ~TransitionView();
	TransitionView(SDL_Renderer* r, SDL_Rect* t, std::string from, std::string to, int maxFrame);

	virtual void Advance();
	virtual void Draw();

private:
	int FramesAdvanced;
	const int MaxFrame;
	SDL_Texture* ImgFrom;
	SDL_Texture* ImgTo;
};
