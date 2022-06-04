using Spire.Abilities;

namespace Spire.UI;

[UseTemplate]
public partial class WeaponAbilityPanel : Panel
{
	public WeaponAbilityType Type { get; set; } = WeaponAbilityType.Attack;
	public WeaponAbility AbilityRef { get; set; }

	// @ref
	public InputHint InputHint { get; set; }

	public string CooldownString
	{
		get
		{
			if ( AbilityRef is null || AbilityRef.InProgress || AbilityRef.LastUsed < 1f )
				return "";

			var nextUse = (float)AbilityRef.NextUse;
			if ( nextUse.Floor() <= 0 )
				return "";

			return $"{nextUse.Floor()}";
		}
	}

	public WeaponAbilityPanel()
	{
	}

	public override void SetProperty( string name, string value )
	{
		base.SetProperty( name, value );

		if ( name == "type" )
		{
			var enumType = Enum.Parse<WeaponAbilityType>( value, true );
			Type = enumType;
		}
	}

	protected void UpdateAbility()
	{
		if ( AbilityRef is not null )
		{
			SetClass( "in-use", AbilityRef.LastUsed < 1f || AbilityRef.InProgress );
			Style.SetBackgroundImage( AbilityRef.AbilityIcon );
		}
		else
		{
			SetClass( "in-use", false );
			Style.SetBackgroundImage( "" );
		}

		SetClass( "no-ability", AbilityRef is null );
		SetClass( "on-cooldown", AbilityRef?.NextUse > 0f );

		InputHint.SetButton( Type.GetButton() );
	}

	public void Update( BaseWeapon weapon )
	{
		if ( !weapon.IsValid() )
		{
			AbilityRef = null;

			UpdateAbility();
			return;
		}

		var ability = weapon._Abilities.FirstOrDefault( x => x.Type == Type );

		if ( AbilityRef == ability )
		{
			UpdateAbility();
			return;
		}

		UpdateAbility();
		AbilityRef = ability;
	}
}
