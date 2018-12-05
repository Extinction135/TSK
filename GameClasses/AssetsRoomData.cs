using System.Collections.Generic;

namespace DungeonRun
{
	public static class RoomData
	{
		//roomData is sorted to lists, based on type
		public static List<RoomXmlData> bossRooms = new List<RoomXmlData>();
		public static List<RoomXmlData> columnRooms = new List<RoomXmlData>();
		public static List<RoomXmlData> exitRooms = new List<RoomXmlData>();
		public static List<RoomXmlData> hubRooms = new List<RoomXmlData>();
		public static List<RoomXmlData> keyRooms = new List<RoomXmlData>();
		public static List<RoomXmlData> rowRooms = new List<RoomXmlData>();
		public static List<RoomXmlData> secretRooms = new List<RoomXmlData>();
		public static List<RoomXmlData> squareRooms = new List<RoomXmlData>();

		static RoomData()
		{

			#region Room - ForestIsland_BossRoom

			{
				RoomXmlData room = new RoomXmlData();
				room.type = RoomID.ForestIsland_BossRoom;
				room.objs = new List<ObjXmlData>();
				bossRooms.Add(room);
			}
			#endregion


			#region Room - DeathMountain_BossRoom

			{
				RoomXmlData room = new RoomXmlData();
				room.type = RoomID.DeathMountain_BossRoom;
				room.objs = new List<ObjXmlData>();
				bossRooms.Add(room);
			}
			#endregion


			#region Room - SwampIsland_BossRoom

			{
				RoomXmlData room = new RoomXmlData();
				room.type = RoomID.SwampIsland_BossRoom;
				room.objs = new List<ObjXmlData>();
				bossRooms.Add(room);
			}
			#endregion


			#region Room - Column

			{
				RoomXmlData room = new RoomXmlData();
				room.type = RoomID.Column;
				room.objs = new List<ObjXmlData>();
				columnRooms.Add(room);
			}
			#endregion


			#region Room - Column

			{
				RoomXmlData room = new RoomXmlData();
				room.type = RoomID.Column;
				room.objs = new List<ObjXmlData>();
				columnRooms.Add(room);
			}
			#endregion


			#region Room - Column

			{
				RoomXmlData room = new RoomXmlData();
				room.type = RoomID.Column;
				room.objs = new List<ObjXmlData>();
				columnRooms.Add(room);
			}
			#endregion


			#region Room - Column

			{
				RoomXmlData room = new RoomXmlData();
				room.type = RoomID.Column;
				room.objs = new List<ObjXmlData>();
				columnRooms.Add(room);
			}
			#endregion


			#region Room - Column

			{
				RoomXmlData room = new RoomXmlData();
				room.type = RoomID.Column;
				room.objs = new List<ObjXmlData>();
				columnRooms.Add(room);
			}
			#endregion


			#region Room - Column

			{
				RoomXmlData room = new RoomXmlData();
				room.type = RoomID.Column;
				room.objs = new List<ObjXmlData>();
				columnRooms.Add(room);
			}
			#endregion


			#region Room - Column

			{
				RoomXmlData room = new RoomXmlData();
				room.type = RoomID.Column;
				room.objs = new List<ObjXmlData>();
				columnRooms.Add(room);
			}
			#endregion


			#region Room - Column

			{
				RoomXmlData room = new RoomXmlData();
				room.type = RoomID.Column;
				room.objs = new List<ObjXmlData>();
				columnRooms.Add(room);
			}
			#endregion


			#region Room - Column

			{
				RoomXmlData room = new RoomXmlData();
				room.type = RoomID.Column;
				room.objs = new List<ObjXmlData>();
				columnRooms.Add(room);
			}
			#endregion


			#region Room - Column

			{
				RoomXmlData room = new RoomXmlData();
				room.type = RoomID.Column;
				room.objs = new List<ObjXmlData>();
				columnRooms.Add(room);
			}
			#endregion


			#region Room - Exit

			{
				RoomXmlData room = new RoomXmlData();
				room.type = RoomID.Exit;
				room.objs = new List<ObjXmlData>();
				exitRooms.Add(room);
			}
			#endregion


			#region Room - Exit

			{
				RoomXmlData room = new RoomXmlData();
				room.type = RoomID.Exit;
				room.objs = new List<ObjXmlData>();
				exitRooms.Add(room);
			}
			#endregion


			#region Room - Exit

			{
				RoomXmlData room = new RoomXmlData();
				room.type = RoomID.Exit;
				room.objs = new List<ObjXmlData>();
				exitRooms.Add(room);
			}
			#endregion


			#region Room - ForestIsland_HubRoom

			{
				RoomXmlData room = new RoomXmlData();
				room.type = RoomID.ForestIsland_HubRoom;
				room.objs = new List<ObjXmlData>();
				hubRooms.Add(room);
			}
			#endregion


			#region Room - DeathMountain_HubRoom

			{
				RoomXmlData room = new RoomXmlData();
				room.type = RoomID.DeathMountain_HubRoom;
				room.objs = new List<ObjXmlData>();
				hubRooms.Add(room);
			}
			#endregion


			#region Room - SwampIsland_HubRoom

			{
				RoomXmlData room = new RoomXmlData();
				room.type = RoomID.SwampIsland_HubRoom;
				room.objs = new List<ObjXmlData>();
				hubRooms.Add(room);
			}
			#endregion


			#region Room - Key

			{
				RoomXmlData room = new RoomXmlData();
				room.type = RoomID.Key;
				room.objs = new List<ObjXmlData>();
				keyRooms.Add(room);
			}
			#endregion


			#region Room - Key

			{
				RoomXmlData room = new RoomXmlData();
				room.type = RoomID.Key;
				room.objs = new List<ObjXmlData>();
				keyRooms.Add(room);
			}
			#endregion


			#region Room - Key

			{
				RoomXmlData room = new RoomXmlData();
				room.type = RoomID.Key;
				room.objs = new List<ObjXmlData>();
				keyRooms.Add(room);
			}
			#endregion


			#region Room - Key

			{
				RoomXmlData room = new RoomXmlData();
				room.type = RoomID.Key;
				room.objs = new List<ObjXmlData>();
				keyRooms.Add(room);
			}
			#endregion


			#region Room - Key

			{
				RoomXmlData room = new RoomXmlData();
				room.type = RoomID.Key;
				room.objs = new List<ObjXmlData>();
				keyRooms.Add(room);
			}
			#endregion


			#region Room - Key

			{
				RoomXmlData room = new RoomXmlData();
				room.type = RoomID.Key;
				room.objs = new List<ObjXmlData>();
				keyRooms.Add(room);
			}
			#endregion


			#region Room - Key

			{
				RoomXmlData room = new RoomXmlData();
				room.type = RoomID.Key;
				room.objs = new List<ObjXmlData>();
				keyRooms.Add(room);
			}
			#endregion


			#region Room - Key

			{
				RoomXmlData room = new RoomXmlData();
				room.type = RoomID.Key;
				room.objs = new List<ObjXmlData>();
				keyRooms.Add(room);
			}
			#endregion


			#region Room - Row

			{
				RoomXmlData room = new RoomXmlData();
				room.type = RoomID.Row;
				room.objs = new List<ObjXmlData>();
				rowRooms.Add(room);
			}
			#endregion


			#region Room - Row

			{
				RoomXmlData room = new RoomXmlData();
				room.type = RoomID.Row;
				room.objs = new List<ObjXmlData>();
				rowRooms.Add(room);
			}
			#endregion


			#region Room - Row

			{
				RoomXmlData room = new RoomXmlData();
				room.type = RoomID.Row;
				room.objs = new List<ObjXmlData>();
				rowRooms.Add(room);
			}
			#endregion


			#region Room - Row

			{
				RoomXmlData room = new RoomXmlData();
				room.type = RoomID.Row;
				room.objs = new List<ObjXmlData>();
				rowRooms.Add(room);
			}
			#endregion


			#region Room - Row

			{
				RoomXmlData room = new RoomXmlData();
				room.type = RoomID.Row;
				room.objs = new List<ObjXmlData>();
				rowRooms.Add(room);
			}
			#endregion


			#region Room - Row

			{
				RoomXmlData room = new RoomXmlData();
				room.type = RoomID.Row;
				room.objs = new List<ObjXmlData>();
				rowRooms.Add(room);
			}
			#endregion


			#region Room - Row

			{
				RoomXmlData room = new RoomXmlData();
				room.type = RoomID.Row;
				room.objs = new List<ObjXmlData>();
				rowRooms.Add(room);
			}
			#endregion


			#region Room - Row

			{
				RoomXmlData room = new RoomXmlData();
				room.type = RoomID.Row;
				room.objs = new List<ObjXmlData>();
				rowRooms.Add(room);
			}
			#endregion


			#region Room - Row

			{
				RoomXmlData room = new RoomXmlData();
				room.type = RoomID.Row;
				room.objs = new List<ObjXmlData>();
				rowRooms.Add(room);
			}
			#endregion


			#region Room - Row

			{
				RoomXmlData room = new RoomXmlData();
				room.type = RoomID.Row;
				room.objs = new List<ObjXmlData>();
				rowRooms.Add(room);
			}
			#endregion


			#region Room - Square

			{
				RoomXmlData room = new RoomXmlData();
				room.type = RoomID.Square;
				room.objs = new List<ObjXmlData>();
				squareRooms.Add(room);
			}
			#endregion


			#region Room - Square

			{
				RoomXmlData room = new RoomXmlData();
				room.type = RoomID.Square;
				room.objs = new List<ObjXmlData>();
				squareRooms.Add(room);
			}
			#endregion


			#region Room - Square

			{
				RoomXmlData room = new RoomXmlData();
				room.type = RoomID.Square;
				room.objs = new List<ObjXmlData>();
				squareRooms.Add(room);
			}
			#endregion


			#region Room - Square

			{
				RoomXmlData room = new RoomXmlData();
				room.type = RoomID.Square;
				room.objs = new List<ObjXmlData>();
				squareRooms.Add(room);
			}
			#endregion


			#region Room - Square

			{
				RoomXmlData room = new RoomXmlData();
				room.type = RoomID.Square;
				room.objs = new List<ObjXmlData>();
				squareRooms.Add(room);
			}
			#endregion


			#region Room - Square

			{
				RoomXmlData room = new RoomXmlData();
				room.type = RoomID.Square;
				room.objs = new List<ObjXmlData>();
				squareRooms.Add(room);
			}
			#endregion


			#region Room - Square

			{
				RoomXmlData room = new RoomXmlData();
				room.type = RoomID.Square;
				room.objs = new List<ObjXmlData>();
				squareRooms.Add(room);
			}
			#endregion


			#region Room - Square

			{
				RoomXmlData room = new RoomXmlData();
				room.type = RoomID.Square;
				room.objs = new List<ObjXmlData>();
				squareRooms.Add(room);
			}
			#endregion


			#region Room - Square

			{
				RoomXmlData room = new RoomXmlData();
				room.type = RoomID.Square;
				room.objs = new List<ObjXmlData>();
				squareRooms.Add(room);
			}
			#endregion


			#region Room - Square

			{
				RoomXmlData room = new RoomXmlData();
				room.type = RoomID.Square;
				room.objs = new List<ObjXmlData>();
				squareRooms.Add(room);
			}
			#endregion

		}
	}
}