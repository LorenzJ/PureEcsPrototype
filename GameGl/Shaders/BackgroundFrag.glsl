#version 330 core

uniform float uTime;

in vec2 Position;

out vec4 FragColor;

void main()
{
    vec2 uv = Position;
    uv.y += uTime * 0.2;
    uv.x = mod(sin(uv.y), cos(uv.x));
    uv.y = mod(-sin(uv.x), -cos(uv.y));;
    float mask1 = smoothstep(-0.50, -0.495, uv.x - uv.y * 0.2);
    mask1 *= smoothstep(0.50, 0.495, uv.x + uv.y * 0.2);
    mask1 -= smoothstep(0.40, 0.395, length(uv - vec2(0, 0.8)));
    mask1 -= smoothstep(0.49, 0.495, uv.y);
    mask1 -= smoothstep(-0.49, -0.495, uv.y);
    
    float mask2 = smoothstep(-0.3, -0.305, uv.y);
    mask2 -= smoothstep(-0.40, -0.39, uv.y);
    mask2 *= smoothstep(0.4, 0.399, uv.y);
    
    vec3 color = mask1 * vec3(sin(uTime), 0.8, 1.0) - mask2 * vec3(0.0, .8, 1.0);
    FragColor = vec4(color,1.0);
}