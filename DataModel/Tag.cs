using System;
using System.Collections.Generic;
using System.Linq;

namespace BlueDiamond.DataModel
{
    [System.Diagnostics.DebuggerDisplay("Tag: {Name}:({Value})")]
    public class Tag :
        IEquatable<Tag>
    {
        public Tag()
        {
        }

        public static Tag Empty = new Tag { Name = "Empty" };

        public static Tag Create(string tag)
        {

            if(string.IsNullOrEmpty(tag))
                return  Tag.Empty;

            string[] v = tag.Split(':');
            if(v.Length!=2)
                return  Tag.Empty;

            Tag t = new Tag()
            {
                Name = v[0],
                Value = v[1]
            };
            return t;
        }

        public string Name { get; set; }

        public string Value { get; set; }

        #region IEquatable

        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(Tag other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            // Return true if the IDs match:
            return
                this.Name == other.Name &&
                this.Value == other.Value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator ==(Tag a, Tag b)
        {
            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(a, b))
                return true;

            // If one is null, but not both, return false.
            if (((object)a == null) || ((object)b == null))
                return false;

            return a.Equals((Tag)b);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator !=(Tag a, Tag b)
        {
            return !(a == b);
        }

        /// <summary>
        /// Compare the IDs to test for equality
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;

            Tag other = obj as Tag;
            if (other == null)
                return false;

            return (                
                this.Name == other.Name &&
                this.Value == other.Value);
        }

        /// <summary>
        /// Use the hashcode of the ID for uniqueness
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }

        /// <summary>
        /// write the name and id of the item
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("{0}:{1}", this.Name, this.Value);
        }

        public bool IsEmpty()
        {
            if(string.IsNullOrEmpty(Name) && string.IsNullOrEmpty(this.Value))
                return true;
            return this == Tag.Empty;
        }

        #endregion
    }


    public class TagCollection: List<Tag>
    {
        public bool ContainsName(string name)
        {
            return this.FirstOrDefault(x => x.Name == name) != null;
        }

        public override string ToString()
        {
            if (this.Count == 0)
                return null;
            var array = this.Select<Tag, string>(x => x.ToString()).ToArray();
            return string.Join("|", array);
        }

        public static TagCollection ToTags(string tagString)
        {
            TagCollection tagCollection = new TagCollection();
            if (!string.IsNullOrEmpty(tagString))
            {
                var list = tagString
                    .Split('|')
                    .Select<string, Tag>(x => Tag.Create(x))
                    .ToList();
                tagCollection.AddRange(list);
            }
            return tagCollection;
        }

        public bool RemoveByName(string name)
        {
            Tag t = this.FirstOrDefault(x => x.Name == name);
            if (t == null)
                return false;
            this.Remove(t);
            return true;
        }

        public void Replace(string name, string value)
        {
            Tag t = this.FirstOrDefault(x => x.Name == name);
            if (t == null)
                this.Add(name, value);
            else
                t.Value = value;
        }

        public void Add(string name, string value)
        {
            this.Add(new Tag() { Name = name, Value = value });
        }

        /// <summary>
        /// Get the tag named as the given value
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        public T GetValue<T>(string name, Func<string,T> parse)
        {
            Tag t = this.FirstOrDefault(x => x.Name == name);
            if (t == null)
                return default(T);

            return parse(t.Value);
        }
    }
}
