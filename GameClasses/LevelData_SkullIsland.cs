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
			{IndObjXmlData obj = new IndObjXmlData(); obj.type = IndestructibleType.ForestDungeon_Entrance; obj.direction = Direction.Down; obj.posX = 536; obj.posY = 472; SkullIsland_ShadowKing.inds.Add(obj);}
			{IndObjXmlData obj = new IndObjXmlData(); obj.type = IndestructibleType.MountainDungeon_Entrance; obj.direction = Direction.Down; obj.posX = 616; obj.posY = 472; SkullIsland_ShadowKing.inds.Add(obj);}
			{IndObjXmlData obj = new IndObjXmlData(); obj.type = IndestructibleType.SwampDungeon_Entrance; obj.direction = Direction.Down; obj.posX = 696; obj.posY = 456; SkullIsland_ShadowKing.inds.Add(obj);}
			{IndObjXmlData obj = new IndObjXmlData(); obj.type = IndestructibleType.Coliseum_Shadow_Spectator; obj.direction = Direction.Down; obj.posX = 792; obj.posY = 520; SkullIsland_ShadowKing.inds.Add(obj);}
			{IndObjXmlData obj = new IndObjXmlData(); obj.type = IndestructibleType.Coliseum_Shadow_Entrance; obj.direction = Direction.Down; obj.posX = 792; obj.posY = 456; SkullIsland_ShadowKing.inds.Add(obj);}
			#endregion


			#region SkullIsland_Town

			SkullIsland_Town.type = RoomID.SkullIsland_Town;
			#endregion

		}
	}
}