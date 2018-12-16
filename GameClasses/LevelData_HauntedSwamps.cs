using System.Collections.Generic;

namespace DungeonRun
{
	public static class LevelData_HauntedSwamps
	{
		//levelData specific to this island
		public static RoomXmlData SwampIsland_MainEntrance = new RoomXmlData();

		//data is added here
		static LevelData_HauntedSwamps()
		{

			#region SwampIsland_MainEntrance

			SwampIsland_MainEntrance.type = RoomID.SwampIsland_MainEntrance;
			SwampIsland_MainEntrance.windDirection = Direction.None;
			SwampIsland_MainEntrance.windFrequency = 2;
			SwampIsland_MainEntrance.windIntensity = 0;
			#endregion

		}
	}
}