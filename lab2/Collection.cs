﻿using System;
using System.Collections.Generic;

namespace lab2
{
    public class Collection : IEquatable<Collection>, IComparable<Collection>
    {
        private String name;
        private List<Track> _tracks = new List<Track>();

        public Collection()
        {
        }

        public Collection(String name)
        {
            this.name = name;
        }

        public Collection(String name, List<Track> _tracks)
        {
            this.name = name;
            this._tracks = _tracks;
        }

        public void AddTrack(Track x)
        {
            _tracks.Add(x);
        }

        public bool Equals(Collection other)
        {
            return String.Equals(this.name, other.name, StringComparison.InvariantCultureIgnoreCase);
        }

        public int CompareTo(Collection other)
        {
            return String.CompareOrdinal(this.name, other.name);
        }

        public override string ToString()
        {
            return name;
        }

        public string ToStringRequest()
        {
            return
                $"NAME: {name}\nTRACKS:\n{String.Join("\n", _tracks)}";
        }
    }
}