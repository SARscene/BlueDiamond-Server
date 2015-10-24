using System;
using System.ComponentModel.DataAnnotations;

namespace BlueDiamond.DataModel
{
    public class SignIn
    {
        public SignIn()
        {
            SignInID = Guid.NewGuid();
            Core = new Core();
            SignedIn = DateTime.UtcNow;
        }

        public Guid SignInID { get; set; }
        
        public Core Core { get; set; }

        [Display(Name="Signed In")]
        public DateTime SignedIn { get; set; }

        [Display(Name = "Signed Out")]
        public DateTime? SignedOut { get; set; }


        public Guid IncidentID { get; set; }

        public virtual Incident Incident{ get; set; }

        public Guid MemberID { get; set; }

        public virtual Member Member { get; set; }
    }
}