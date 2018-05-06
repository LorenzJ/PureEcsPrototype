#version 330 core

uniform float uTime;

in vec2 Position;
out vec4 FragColor;

void main()
{
	vec2 uv = Position;
	float r = mod(uv.y + uTime, 0.2) * .5 + mod(uv.x / sin(uv.x * uTime) + uTime, 2.) * .5;
    float g = smoothstep(0.1, 0.09, length(uv));
	FragColor = vec4(r, g, 0, 1);
}