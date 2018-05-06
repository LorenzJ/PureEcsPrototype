#version 330 core

uniform float uTime;

in vec2 Position;

out vec4 FragColor;

void main()
{
	float t = uTime * 2.0;
    vec2 uv = Position;

    float circle = smoothstep(1, 0.99, length(uv));
	float alpha = circle;
	circle -= smoothstep(0.9, 0.89, length(uv));
    circle += smoothstep(0.8, 0.79, length(uv));
    
    vec2 orig = uv;
    uv.x += cos(t) * 0.4;
    uv.y += sin(t) * 0.4;
    circle -= smoothstep(0.2, 0.19, length(uv));
    uv = orig;
    uv.x -= cos(t) * 0.4;
    uv.y -= sin(t) * 0.4;
    circle -= smoothstep(0.2, 0.19, length(uv));
    uv = orig;
    uv.x += sin(t) * 0.4;
    uv.y -= cos(t) * 0.4;
    circle -= smoothstep(0.2, 0.19, length(uv));
    uv = orig;
    uv.x -= sin(t) * 0.4;
    uv.y += cos(t) * 0.4;
    circle -= smoothstep(0.2, 0.19, length(uv));
    
    vec3 color = vec3(1.0 + sin(uTime), cos(uv.x * 2.0), sin(uv.y * 2.0)) * circle;
    
	FragColor = vec4(color, alpha);
}
