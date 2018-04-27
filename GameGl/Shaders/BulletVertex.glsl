#version 400 core

layout(location = 0) in vec2 aPosition;
layout(location = 1) in vec2 aOffset;

out vec2 Position;

void main()
{
	gl_Position = vec4(aPosition * 0.1 + aOffset, 0, 1);
	Position = aPosition;
}