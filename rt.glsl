#shader vertex
#version 330 core

layout(location = 0) in vec3 _position;
layout(location = 1) in vec2 _texCoord;

out vec2 texCoord;

void main()
{
    gl_Position =  vec4(_position, 1.0f);
    texCoord = _texCoord;
}
#shader fragment
#version 330 core

uniform sampler2D tex;

in vec2 texCoord;
out vec4 aColor;

void main()
{
    aColor = texture(tex, texCoord);
}