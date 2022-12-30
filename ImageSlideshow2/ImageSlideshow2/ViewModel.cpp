#include "pch.h"

#include "config.h"

#include "ViewModel.h"
#include "Model.h"
#include "View.h"

static const int STATIC_MS = 8000;
static const int TRANSITION_MS = 1000;

ViewModel::ViewModel(class Model& m, int w, int h, SDL_Renderer* renderer)
	: Model(m)
	, Renderer(renderer)
	, TexRect{ 0, 0, w, h }
{
	NextImagePath = Model.NextImagePath();
	Action = TRANSITION;
	SwitchTimer = 0;
}

std::shared_ptr<View> ViewModel::NextView()
{
	if (SwitchTimer > 0)
	{
		SwitchTimer--;
		ActionView->Advance();
		return ActionView;
	}

	// On finish
	switch (Action)
	{
	case STATIC:
		SwitchTimer = ms2frames(TRANSITION_MS);
		Action = TRANSITION;
		NextImagePath = Model.NextImagePath();
		ActionView = std::make_shared<TransitionView>(Renderer, &TexRect, CurrentImagePath, NextImagePath, ms2frames(TRANSITION_MS));

		break;
	case TRANSITION:
		int staticFrames = ms2frames(STATIC_MS);
		Action = STATIC;
		CurrentImagePath = NextImagePath;
		ActionView = std::make_shared<StaticView>(Renderer, &TexRect, CurrentImagePath);
		SwitchTimer = staticFrames;

		break;
	}

	return ActionView;
}