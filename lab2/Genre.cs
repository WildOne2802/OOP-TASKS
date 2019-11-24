﻿using System;
using System.Collections.Generic;

namespace lab2
{
    public class Genre : IEquatable<Genre>, IComparable<Genre>
    {
        private String name;
        private List<Track> _tracks = new List<Track>();
        private Genre parent;
        private List<Genre> _kids = new List<Genre>();

        public Genre()
        {
        }

        public Genre(String name)
        {
            this.name = name;
        }

        public Genre(String name, List<Track> _tracks)
        {
            this.name = name;
            this._tracks = _tracks;
        }

        public void AddKid(Genre g)
        {
            _kids.Add(g);
        }

        public void AddParent(Genre g)
        {
            parent = g;
        }

        public void AddTrack(Track x)
        {
            _tracks.Add(x);
        }

        public bool Equals(Genre other)
        {
            return String.Equals(this.name, other.name, StringComparison.InvariantCultureIgnoreCase);
        }

        public int CompareTo(Genre other)
        {
            return String.CompareOrdinal(this.name, other.name);
        }

        public override string ToString()
        {
            return name;
        }

        public string ToStringRequest()
        {
            List<String> array = new List<String>();
            foreach (var kid in _kids)
            {
                array.Add(kid.ToStringRequest());
            }

            return
                $"NAME: {name}\nPARENT GENRE: {parent}\nKID GENRES:\n{String.Join("\n", _kids)}\nTRACKS: \n{String.Join("\n", _tracks)}\nKIDS:\n{String.Join("\n", array)}";
        }
    }
}