shader_type canvas_item;

uniform sampler2D screen_texture: hint_screen_texture, filter_linear_mipmap;
uniform float amount: hint_range(0.0, 5.0) = 1.0;
uniform float mix_percentage: hint_range(0.0, 1.0) = 0.3;
uniform vec4 tint_color: source_color = vec4(0, 0, 0, 1);

void fragment()
{
	vec4 color = textureLod(screen_texture, SCREEN_UV, amount);
	color = mix(color, tint_color, mix_percentage);
	COLOR = color;
}
