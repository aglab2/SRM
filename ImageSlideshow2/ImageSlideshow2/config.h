#pragma once

#define WIDTH 1280
#define HEIGHT 624

#define FRAMES_PER_SECOND 40
#define MS_PER_FRAME (1000 / 40)

static inline int ms2frames(int ms)
{
	return ms / MS_PER_FRAME;
}

static inline int frames2ms(int frames)
{
	return frames * MS_PER_FRAME;
}