namespace Spire.Abilities;

public enum WeaponAbilityType : short
{
	Attack,
	Special,
	Ultimate
}

public static class WeaponAbilityTypeExtensions
{
	public static InputButton GetButton( this WeaponAbilityType type )
	{
		return type switch
		{
			WeaponAbilityType.Attack => InputButton.PrimaryAttack,
			WeaponAbilityType.Special => InputButton.Menu,
			WeaponAbilityType.Ultimate => InputButton.Reload,
			_ => InputButton.PrimaryAttack
		};
	}
}
