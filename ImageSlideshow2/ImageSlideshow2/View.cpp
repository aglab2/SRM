#include "pch.h"

#include "View.h"

View::View(SDL_Renderer* r, SDL_Rect* t) : Renderer(r), TexRect(t) { }

SDL_Texture* View::CreateTexture(std::string& path)
{
	return IMG_LoadTexture(Renderer, path.c_str());
}

StaticView::StaticView(SDL_Renderer* r, SDL_Rect* t, std::string path)
	: View(r, t)
	, Img(CreateTexture(path)) { }

StaticView::~StaticView()
{
	if (Img)
		SDL_DestroyTexture(Img);
}

void StaticView::Advance() { }

void StaticView::Draw()
{
	SDL_RenderClear(Renderer);
	SDL_RenderCopy(Renderer, Img, NULL, TexRect);
	SDL_RenderPresent(Renderer);
}

TransitionView::TransitionView(SDL_Renderer* r, SDL_Rect* t, std::string from, std::string to, int maxFrame)
	: View(r, t)
	, ImgFrom(CreateTexture(from))
	, ImgTo(CreateTexture(to)) 
	, FramesAdvanced(0)
	, MaxFrame(maxFrame)
{
	SDL_SetTextureBlendMode(ImgTo, SDL_BLENDMODE_BLEND);
	SDL_SetTextureAlphaMod(ImgTo, 0);
}

TransitionView::~TransitionView()
{
	if (ImgFrom) SDL_DestroyTexture(ImgFrom);
	if (ImgTo)   SDL_DestroyTexture(ImgTo);
}

void TransitionView::Advance()
{
	FramesAdvanced++;
	SDL_SetTextureAlphaMod(ImgTo, FramesAdvanced * 255 / MaxFrame);
}

void TransitionView::Draw()
{
	SDL_RenderClear(Renderer);
	SDL_RenderCopy(Renderer, ImgFrom, NULL, TexRect);
	SDL_RenderCopy(Renderer, ImgTo, NULL, TexRect);
	SDL_RenderPresent(Renderer);
}