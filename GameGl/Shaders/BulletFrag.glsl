#version 330 core

uniform float uTime;

in vec2 Position;

out vec4 FragColor;

void main()
{
	float t = uTime * 2.0;
    vec2 uv = Position;

    float circle = smoothstep(0.50, 0.49, length(uv));
	float alpha = circle;
	circle -= smoothstep(0.45, 0.44, length(uv));
    circle += smoothstep(0.4, 0.39, length(uv));
    
    vec2 orig = uv;
    uv.x += cos(t) * 0.2;
    uv.y += sin(t) * 0.2;
    circle -= smoothstep(0.1, 0.09, length(uv));
    uv = orig;
    uv.x -= cos(t) * 0.2;
    uv.y -= sin(t) * 0.2;
    circle -= smoothstep(0.1, 0.09, length(uv));
    uv = orig;
    uv.x += sin(t) * 0.2;
    uv.y -= cos(t) * 0.2;
    circle -= smoothstep(0.1, 0.09, length(uv));
    uv = orig;
    uv.x -= sin(t) * 0.2;
    uv.y += cos(t) * 0.2;
    circle -= smoothstep(0.1, 0.09, length(uv));
    
    vec3 color = vec3(1.0 + sin(uTime), cos(uv.x * 2.0), sin(uv.y * 2.0)) * circle;
    
	FragColor = vec4(color, alpha);
}
