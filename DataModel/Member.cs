using System;
using System.ComponentModel.DataAnnotations;

namespace BlueDiamond.DataModel
{
    public class Member : IEquatable<Member>
    {
        public Member()
        {
            MemberID = Guid.NewGuid();
            Core = new Core();
        }

        public static readonly Guid SYSTEM_ADMINISTRATOR = new Guid("{6A4A0509-C53E-4BAD-A902-5496840C937B}");

        public Guid MemberID { get; set; }

        public Core Core { get; set; }

        #region properties

        [Display(Name="First Name")]
        [Required(ErrorMessage = "First Name is required")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "Last Name is required")]
        public string LastName { get; set; }

        [Phone]
        [Display(Name = "Phone Number")]
        [Required(ErrorMessage = "Phone Number is required")]
        public string PhoneNumber { get; set; }

        [EmailAddress]
        [Display(Name = "Email Address")]
        [Required(ErrorMessage = "Email Addressis required")]
        public string EmailAddress { get; set; }

        #endregion

        #region Agency

        public Guid? AgencyID { get; set; }

        public virtual Agency Agency { get; set; }

        #endregion

        #region Role
        public Guid? RoleID { get; set; }

        public virtual Role Role { get; set; }

        #endregion

        #region equality

        public bool Equals(Member other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return
                this.Core == other.Core &&
                this.FirstName == other.FirstName &&
                this.LastName == other.LastName &&
                this.PhoneNumber == other.PhoneNumber;
        }

        public static bool operator ==(Member a, Member b)
        {
            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(a, b))
                return true;

            // If one is null, but not both, return false.
            if (((object)a == null) || ((object)b == null))
                return false;

            return a.Equals((Member)b);
        }

        public static bool operator !=(Member a, Member b)
        {
            return !(a == b);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;

            Member other = obj as Member;
            if (other == null)
                return false;

            return
                this.Core == other.Core &&
                this.FirstName == other.FirstName &&
                this.LastName == other.LastName &&
                this.PhoneNumber == other.PhoneNumber;
        }

        public override int GetHashCode()
        {
            return this.Core.Created.GetHashCode();
        }

        public override string ToString()
        {
            return string.Format("{0}, {1} ({2})", this.LastName, this.FirstName, this.PhoneNumber);
        }
        #endregion

        public void Update(Member model)
        {
            if (model == null)
                return;
            this.FirstName = model.FirstName;
            this.LastName = model.LastName;
            this.PhoneNumber = model.PhoneNumber;
            this.Core.Modify("");
        }
    }
}