using Spire.Abilities;

namespace Spire.UI;

[UseTemplate]
public partial class PlayerAbilityPanel : Panel
{
	public int Slot { get; set; } = 0;
	public PlayerAbility AbilityRef { get; set; }

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

	public PlayerAbilityPanel()
	{
	}

	public override void SetProperty( string name, string value )
	{
		base.SetProperty( name, value );

		if ( name == "type" )
		{
			Slot = value.ToInt( 0 );
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

		InputHint.SetButton( PlayerAbility.GetInputButtonFromSlot( Slot ) );
	}

	public void Update( PlayerCharacter character )
	{
		var ability = character.GetAbilityFromSlot( Slot );

		if ( AbilityRef == ability )
		{
			UpdateAbility();
			return;
		}

		if ( ability is not null )
		{
			UpdateAbility();
			AbilityRef = ability;
		}
	}
}
