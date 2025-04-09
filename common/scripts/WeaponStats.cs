using Godot;
using Godot.Collections;

namespace Entity.Stats;

[GlobalClass]
public partial class WeaponStats : Resource
{
	[ExportGroup("Core Attributes")]
	[Export(PropertyHint.Range, "0, 1, or_greater, hide_slider")] public int Damage { get; private set; }
	[Export(PropertyHint.Range, "0, 1, or_greater, hide_slider")] public float FireRate { get; private set; }
	[Export(PropertyHint.Range, "0, 1, or_greater, hide_slider")] public int MagazineSize { get; private set; }
	[Export(PropertyHint.Range, "0, 1, or_greater, hide_slider")] public float ReloadTime { get; private set; }

	[ExportGroup("Accuracy & Range")]
	[Export(PropertyHint.Range, "0, 1, or_greater, hide_slider")] public int EffectiveRange { get; private set; }
	[Export(PropertyHint.Range, "0.1, 180")] public float BaseSpread { get; private set; }
	[Export(PropertyHint.Range, "0, 180")] public float MovementSpread { get; private set; }
	[Export(PropertyHint.Range, "0, 1, or_greater, hide_slider")] public int AimedRange { get; private set; }
	[Export(PropertyHint.Range, "0, 360")] public float AimedSpread { get; private set; }

	[ExportGroup("Penetration & Damage Scaling")]
	[Export(PropertyHint.Range, "0, 100")] public float PiercingPower { get; private set; }
	[Export(PropertyHint.Range, "0, 100")] public float ArmorPenetration { get; private set; }
	[Export(PropertyHint.Range, "0, 1, or_greater, hide_slider")] public float HealthDamageMultiplier { get; private set; }
	[Export(PropertyHint.Range, "0, 1, or_greater, hide_slider")] public float ArmorDamageMultiplier { get; private set; }

	[ExportGroup("Handling & Control")]
	[Export(PropertyHint.Range, "0, 1, or_greater, hide_slider")] public float BulletVelocity { get; private set; }
	[Export(PropertyHint.Range, "0, 100")] public float Recoil { get; private set; }
	[Export(PropertyHint.Range, "0, 100")] public float Stability { get; private set; }
	[Export(PropertyHint.Range, "0, 1, or_greater, hide_slider")] public float Weight { get; private set; }
	[Export(PropertyHint.Range, "0, 1, or_greater, hide_slider")] public float AimingTime { get; private set; }

	[ExportGroup("Stealth & Suppression")]
	[Export(PropertyHint.Range, "0, 1, or_greater, hide_slider")] public float NoiseRadius { get; private set; }

	public Dictionary<string, Stat> GetWeaponStats()
	{
		return new()
		{
			{ "WeaponDamage", new ScaledStat(Damage, 0.0f, Damage) },
			{ "WeaponFireRate", new ScaledStat(FireRate, 0.0f, FireRate) },
			{ "WeaponMagazineSize", new ScaledStat(MagazineSize, 0.0f, MagazineSize) },
			{ "WeaponReloadTime", new ScaledStat(ReloadTime, 0.0f, ReloadTime) },
			{ "WeaponEffectiveRange", new ScaledStat(EffectiveRange, 0.0f, EffectiveRange) },
			{ "WeaponBaseSpread", new LockedStat(BaseSpread, 0.0f, 180.0f) },
			{ "WeaponMovementSpread", new LockedStat(MovementSpread, 0.0f, 180.0f) },
			{ "WeaponAimedRange", new ScaledStat(AimedRange, 0.0f, AimedRange) },
			{ "WeaponAimedSpread", new LockedStat(AimedSpread, 0.0f, 360) },
			{ "WeaponPiercingPower", new LockedStat(PiercingPower, 0.0f, 100.0f) },
			{ "WeaponArmorPenetration", new LockedStat(ArmorPenetration, 0.0f, 100.0f) },
			{ "WeaponHealthDamageMultiplier", new ScaledStat(HealthDamageMultiplier, 0.0f, HealthDamageMultiplier) },
			{ "WeaponArmorDamageMultiplier", new ScaledStat(ArmorDamageMultiplier, 0.0f, ArmorDamageMultiplier) },
			{ "WeaponBulletVelocity", new ScaledStat(BulletVelocity, 0.0f, BulletVelocity) },
			{ "WeaponRecoil", new LockedStat(Recoil, 0.0f, 100.0f) },
			{ "WeaponStability", new LockedStat(Stability, 0.0f, 100.0f) },
			{ "WeaponWeight", new ScaledStat(Weight, 0.0f, Weight) },
			{ "WeaponAimingTime", new ScaledStat(AimingTime, 0.0f, AimingTime) },
			{ "WeaponNoiseRadius", new ScaledStat(NoiseRadius, 0.0f, NoiseRadius) },
		};
	}
}
