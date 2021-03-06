﻿#version 330 core

uniform float uScale;

layout(location = 0) in vec2 aPosition;
layout(location = 1) in vec2 aOffset;

out vec2 Position;

void main()
{
	gl_Position = vec4(aPosition * vec2(1, .75) * uScale + aOffset * vec2(1, .75), -1, 1);
	Position = aPosition;
}