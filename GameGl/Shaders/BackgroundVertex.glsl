#version 330 core

layout (location = 0) in vec2 aPosition;

out vec2 Position;

void main() {
	gl_Position = vec4(aPosition * 2., 0., 1.);
	Position = aPosition;
}