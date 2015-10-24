using System;

namespace BlueDiamond.DataModel
{
    public class Core : IEquatable<Core>
    {
        public Core()
        {
            Created = DateTime.UtcNow;
            Version = 0;
        }

        public DateTime Created { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? Modified { get; set; }

        public string ModifiedBy { get; set; }
        
        public bool IsDeleted { get; set; }

        public int Version { get; set; }

        /// <summary>
        /// For displaying an ISO string
        /// </summary>
        public string CreatedISO { get { return Created.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'Z'"); } }

        public string CreatedLocal { get { return Created.ToLocalTime().ToString(); } }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static Core Create(string userName)
        {
            return new Core()
            {
                CreatedBy = userName,
                Created = DateTime.UtcNow,
                Modified = DateTime.UtcNow,
                Version=0
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool IsModified()
        {
            if (!Modified.HasValue)
                return false;
            if (Modified == Created)
                return false;
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        public void Modify(string userName)
        {
            Modified = DateTime.UtcNow;
            ModifiedBy = userName;
            Version++;
        }

        /// <summary>
        /// Delete the item
        /// </summary>
        /// <param name="userName"></param>
        public void Delete(string userName)
        {
            Modify(userName);
            IsDeleted = true;
        }

        #region equality

        public bool Equals(Core other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return
                this.CreatedBy == other.CreatedBy &&
                this.Created == other.Created &&
                this.ModifiedBy == other.ModifiedBy &&
                this.Modified.GetValueOrDefault() == other.Modified.GetValueOrDefault();
        }

        public static bool operator ==(Core a, Core b)
        {
            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(a, b))
                return true;

            // If one is null, but not both, return false.
            if (((object)a == null) || ((object)b == null))
                return false;

            return a.Equals((Core)b);
        }

        public static bool operator !=(Core a, Core b)
        {
            return !(a == b);
        }
       
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;

            Core other = obj as Core;
            if (other == null)
                return false;

            return
                this.CreatedBy == other.CreatedBy &&
                this.Created == other.Created &&
                this.ModifiedBy == other.ModifiedBy &&
                this.Modified.GetValueOrDefault() == other.Modified.GetValueOrDefault();
        }

        public override int GetHashCode()
        {
            return this.Created.GetHashCode();
        }

        public override string ToString()
        {
            return string.Format("Created by \"{0}\" on {1}", this.CreatedBy, this.CreatedLocal);
        }
        #endregion

    }
}   