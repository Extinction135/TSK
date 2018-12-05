using System.Collections.Generic;

namespace DungeonRun
{
	public static class LevelData_DeathMountain
	{
		//levels specific to this island
		public static RoomXmlData DeathMountain_MainEntrance = new RoomXmlData();

		//level data
		static LevelData_DeathMountain()
		{

			#region DeathMountain_MainEntrance

			DeathMountain_MainEntrance.type = RoomID.DeathMountain_MainEntrance;
			DeathMountain_MainEntrance.objs = new List<ObjXmlData>();
			#endregion

		}
	}
}