using System;

namespace EverTech.Permission.Molecules
{
	public class UserRoleMolecule
	{
        public int Id { get; set; }
        public int UserId { get; set; }
        public int RoleId { get; set; }
        public DateTime AddTime { get; set; }
        public DateTime EditTime { get; set; }
        public bool Enable { get; set; }


    }
}