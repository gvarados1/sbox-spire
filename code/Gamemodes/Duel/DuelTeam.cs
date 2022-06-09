namespace Spire.Gamemodes.Duel;

public enum DuelTeam
{
	Red,
	Blue
}

public static partial class DuelTeamHelpers
{
	public static DuelTeam GetLowestTeam()
	{
		DuelTeam currentLowest = DuelTeam.Blue;
		int value = 999;
		foreach ( var _team in Enum.GetValues<DuelTeam>() )
		{
			var members = _team.GetMembers();
			if ( members.Count() <= value )
			{
				value = members.Count();
				currentLowest = _team;
			}
		}

		return currentLowest;
	}
}

public static partial class DuelTeamExtensions
{
	public static IEnumerable<Client> GetMembers( this DuelTeam team )
	{
		return Client.All.Where( x => x.GetTeam() == team );
	}

	public static string GetName( this DuelTeam team )
	{
		return team switch
		{
			DuelTeam.Red => "Red",
			DuelTeam.Blue => "Blue",
			_ => "N/A"
		};
	}

	public static Color GetColor( this DuelTeam team )
	{
		return team switch
		{
			DuelTeam.Red => new Color( 228f / 255f, 52f / 255f, 52f / 255f ),
			DuelTeam.Blue => new Color( 52f / 255f, 99f / 255f, 228f / 255f ),
			_ => Color.Gray
		};
	}
}
