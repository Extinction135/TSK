using System.Collections.Generic;

namespace DungeonRun
{
	public static class LevelData_SkullIsland
	{
		//levels specific to this island
		public static RoomXmlData SkullIsland_Colliseum = new RoomXmlData();
		public static RoomXmlData SkullIsland_ColliseumPit = new RoomXmlData();
		public static RoomXmlData SkullIsland_ShadowKing = new RoomXmlData();
		public static RoomXmlData SkullIsland_Town = new RoomXmlData();

		//level data
		static LevelData_SkullIsland()
		{

			#region SkullIsland_Colliseum

			SkullIsland_Colliseum.type = RoomID.SkullIsland_Colliseum;
			#endregion


			#region SkullIsland_ColliseumPit

			SkullIsland_ColliseumPit.type = RoomID.SkullIsland_ColliseumPit;
			#endregion


			#region SkullIsland_ShadowKing

			SkullIsland_ShadowKing.type = RoomID.SkullIsland_ShadowKing;
			#endregion


			#region SkullIsland_Town

			SkullIsland_Town.type = RoomID.SkullIsland_Town;
			#endregion

		}
	}
}