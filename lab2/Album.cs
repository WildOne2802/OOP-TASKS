﻿using System;
using System.Collections.Generic;

namespace lab2
{
    public class Album : IEquatable<Album>, IComparable<Album>
    {
        private String name;
        private List<Track> _tracks = new List<Track>();
        private int _releaseDateTime;
        private Genre _genre = new Genre();

        public Album()
        {
        }

        public Album(String name)
        {
            this.name = name;
        }

        public Album(String name, int _releaseDateTime)
        {
            this.name = name;
            this._releaseDateTime = _releaseDateTime;
        }

        public Album(String name, int _releaseDateTime, Genre _genre)
        {
            this.name = name;
            this._releaseDateTime = _releaseDateTime;
            this._genre = _genre;
        }

        public Album(String name, List<Track> _tracks)
        {
            this.name = name;
            this._tracks = _tracks;
        }

        public void AddTrack(Track x)
        {
            _tracks.Add(x);
        }

        public bool Equals(Album other)
        {
            return String.Equals(this.name, other.name, StringComparison.InvariantCultureIgnoreCase);
        }

        public bool YearComparison(int date)
        {
            return this._releaseDateTime == date;
        }

        public int CompareTo(Album other)
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
                $"NAME: {name}\nRELEASE DATE: {_releaseDateTime}\nGENRE: {_genre}\nTRACKS:\n{String.Join("\n", _tracks)}";
        }
    }
}