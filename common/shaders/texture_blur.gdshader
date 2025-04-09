shader_type canvas_item;

uniform float mix_percentage: hint_range(0.0, 1.0) = 0;
uniform vec4 tint_color: source_color = vec4(0, 0, 0, 1);

void fragment()
{
	vec4 color = texture(TEXTURE, UV);
	color = mix(color, tint_color, mix_percentage);

	COLOR = color;
}
