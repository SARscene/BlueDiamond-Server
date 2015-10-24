using System;
using System.Collections.Generic;
using BlueDiamond.DataModel;

namespace BlueDiamond.Models
{
    public class CheckInViewModel
    {
        public CheckInViewModel()
        {
            SignIns = new List<MemberSignIn>();
        }

        public Incident Incident { get; set; }

        public List<MemberSignIn> SignIns { get; set; }
    }

    public class MemberSignIn
    {
        public MemberSignIn() { }

        public Guid MemberID { get { return Member.MemberID; } }
        public bool IsSignedIn
        {
            get { return SignIn != null && SignIn.SignedOut == null; }
        }

        public DateTime? SignInTime
        {
            get
            {
                return ((SignIn != null) ? SignIn.SignedIn.ToLocalTime() : (DateTime?)null);
            }
        }

        public DateTime? SignOutTime
        {
            get
            {
                return ((SignIn != null && SignIn.SignedOut.HasValue) ? SignIn.SignedOut.Value.ToLocalTime() : (DateTime?)null);
            }
        }
        
        public Member Member { get; set; }
        
        public SignIn SignIn { get; set; }
    }
}