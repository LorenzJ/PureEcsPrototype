#version 330 core

out vec4 FragColor;
in vec2 Uv;

uniform sampler2D screenTexture;

void main()
{
	FragColor = mix(texture(screenTexture, Uv), vec4(Uv.x, Uv.y, 0, 1), 0.5);
}