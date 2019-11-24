﻿using System;

namespace lab2
{
    public class Track : IEquatable<Track>, IComparable<Track>
    {
        private String name;
        private Artist _artist;
        private Genre _genre;
        private Album _album;
        private Collection _collection;
        private int _releaseDateTime;

        public Track()
        {
        }

        public Track(String name)
        {
            this.name = name;
        }

        public Track(String name, Artist _artist, Genre _genre, Album _album, Collection _collection,
            int _releaseDateTime)
        {
            this.name = name;
            this._artist = _artist;
            this._genre = _genre;
            this._album = _album;
            this._collection = _collection;
            this._releaseDateTime = _releaseDateTime;
        }

        public bool Equals(Track other)
        {
            return String.Equals(this.name, other.name, StringComparison.InvariantCultureIgnoreCase);
        }

        public bool YearComparison(int date)
        {
            return this._releaseDateTime == date;
        }

        public int CompareTo(Track other)
        {
            return String.CompareOrdinal(this.name, other.name);
        }

        public override string ToString()
        {
            return $"{_artist}\t - \t{_album}\t - \t{name}\t - \t{_genre}\t - \t{_collection}\t - \t{_releaseDateTime}";
        }

        public bool Contains(string part)
        {
            return this.name.Contains(part, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}