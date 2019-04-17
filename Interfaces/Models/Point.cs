using System;

namespace Interfaces
{
	public class Point : ICloneable
	{
		public Int32 X { get; set; }
		public Int32 Y { get; set; }
		public PointDescription desc = new PointDescription();

		public Point() { }

		public Point(Int32 xPos, Int32 yPos)
		{
			X = xPos; Y = yPos;
		}

		public Point(Int32 xPos, Int32 yPos, String petName) : this(xPos, yPos)
		{
			desc.PetName = petName;
		}

		public override string ToString() => $"X={X}; Y={Y}; Name={desc.PetName};\nID={desc.PointId}";

		public object Clone()
		{
			Point clonedPoint=(Point)this.MemberwiseClone();
			clonedPoint.desc = new PointDescription();
			clonedPoint.desc.PetName = this.desc.PetName;
			return clonedPoint;			
		}
	}

	public class PointDescription
	{
		public String PetName { get; set; }
		public Guid PointId { get; set; }

		public PointDescription()
		{
			PetName = "No-name";
			PointId = Guid.NewGuid();
		}
	}
}
