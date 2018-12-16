using System.Collections.Generic;

namespace DungeonRun
{
	public static class LevelData_CloudIsland
	{
		//levels specific to this island
		public static RoomXmlData CloudIsland_MainEntrance = new RoomXmlData();

		//level data
		static LevelData_CloudIsland()
		{

			#region CloudIsland_MainEntrance

			CloudIsland_MainEntrance.type = RoomID.CloudIsland_MainEntrance;
			CloudIsland_MainEntrance.windDirection = Direction.None;
			CloudIsland_MainEntrance.windFrequency = 2;
			CloudIsland_MainEntrance.windIntensity = 0;
			#endregion

		}
	}
}