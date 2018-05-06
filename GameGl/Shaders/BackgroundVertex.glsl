#version 330 core

uniform mat4 uViewProjection;

layout (location = 0) in vec2 aPosition;

out vec2 Position;

void main() {
	
	gl_Position = uViewProjection * vec4(aPosition, 0., 1.);
	Position = aPosition *2.;
}