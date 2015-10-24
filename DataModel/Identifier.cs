using System;

namespace BlueDiamond.DataModel
{
    public class Identifier
    {
        public Identifier()
        {
            IdentifierID = Guid.NewGuid();
        }

        public Guid IdentifierID { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Value { get; set; }


    }
}