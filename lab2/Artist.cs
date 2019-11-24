using System;
using System.Collections.Generic;

namespace lab2
{
    public class Artist : IEquatable<Artist>, IComparable<Artist>
    {
        private String name;
        private List<Track> _tracks = new List<Track>();

        public Artist()
        {
        }

        public Artist(String name)
        {
            this.name = name;
        }

        public Artist(String name, List<Track> _tracks)
        {
            this.name = name;
            this._tracks = _tracks;
        }

        public bool Equals(Artist other)
        {
            return String.Equals(this.name, other.name, StringComparison.InvariantCultureIgnoreCase);
            //this.name == other.name; //StringComparison.InvariantCultureIgnoreCase
        }

        public void AddTrack(Track x)
        {
            _tracks.Add(x);
        }

        public int CompareTo(Artist other)
        {
            return String.CompareOrdinal(this.name, other.name);
        }

        public override string ToString()
        {
            return name;
        }

        public string ToStringRequest()
        {
            return $"NAME: {name}\n{String.Join("\n", _tracks)}";
        }
    }
}