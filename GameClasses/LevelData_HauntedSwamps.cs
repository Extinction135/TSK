using System.Collections.Generic;

namespace DungeonRun
{
	public static class LevelData_HauntedSwamps
	{
		//levels specific to this island
		public static RoomXmlData SwampIsland_MainEntrance = new RoomXmlData();

		//level data
		static LevelData_HauntedSwamps()
		{

			#region SwampIsland_MainEntrance

			SwampIsland_MainEntrance.type = RoomID.SwampIsland_MainEntrance;
			SwampIsland_MainEntrance.objs = new List<ObjXmlData>();
			#endregion

		}
	}
}