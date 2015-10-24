using System;

namespace BlueDiamond.DataModel
{
    [System.Diagnostics.DebuggerDisplay("Role: {Name}({Value}):{Data}")]
    public class Role :
        IEquatable<Role>
    {
        public Role()
        {
            RoleID = Guid.NewGuid();
            Core = new Core();
        }

        public Guid RoleID { get; set; }

        public Core Core { get; set; }

        /// <summary>
        /// Role name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Arbitrary data
        /// </summary>
        public string Data { get; set; }

        public string Value { get; set; }

        #region equality
        public bool Equals(Role other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return this.RoleID == other.RoleID;
        }

        public static bool operator ==(Role a, Role b)
        {
            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(a, b))
                return true;

            // If one is null, but not both, return false.
            if (((object)a == null) || ((object)b == null))
                return false;

            return a.Equals((Role)b);
        }

        public static bool operator !=(Role a, Role b)
        {
            return !(a == b);
        }
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;

            Role incident = obj as Role;
            if (incident == null)
                return false;

            return (incident.RoleID == this.RoleID);
        }

        public override int GetHashCode()
        {
            return RoleID.GetHashCode();
        }

        public override string ToString()
        {
            return string.IsNullOrEmpty(this.Name) ?
                RoleID.ToString() :
                string.Format("Role: \"{0}\"", this.Name);
        }
        #endregion

    }
}
