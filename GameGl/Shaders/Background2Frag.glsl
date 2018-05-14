#version 330 core

uniform float uTime;

in vec2 Position;

out vec4 FragColor;

void main()
{
    vec2 uv = Position;
    uv *= 20.;
    
    float r = smoothstep(.9, 1., sin(uv.y * 39.) * cos(uv.x * .5));
    float gb = mod(sin(uv.x) * iTime,  sin(uv.y + iTime));
	
    FragColor = vec4(r, gb, gb,1.0);
}