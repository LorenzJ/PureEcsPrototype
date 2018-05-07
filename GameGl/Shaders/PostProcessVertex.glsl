#version 330 core

const vec4 vertices[6] = vec4[6]
(
	vec4(-1, 1,    0, 0),
	vec4(-1, -1,   0, 1),
	vec4(1, -1,    1, 1),

	vec4(1, -1,    1, 1),
	vec4(1, 1,     1, 0),
	vec4(-1, 1,    0, 0)
);

out vec2 Uv;

void main()
{
	gl_Position = vec4(vertices[gl_VertexID].xy, 0, 1);
	Uv = vertices[gl_VertexID].zw;
}
