﻿using System;
using System.Collections.Generic;
using System.IO;
 using System.Linq;

 namespace lab2
{
    public class Catalogue
    {
        public List<Artist> _artists = new List<Artist>();
        public List<Genre> _genres = new List<Genre>();
        public List<Collection> _collections = new List<Collection>();
        public List<Album> _albums = new List<Album>();
        public List<Track> _tracks = new List<Track>();

        public void ReadFromFile(String path)
        {
            using (StreamReader reader = new StreamReader(path))
            {
                String line;
                while ((line = reader.ReadLine()) != null)
                {
                    String[] lines = line.Split(" - ");

                    Artist a = new Artist(lines[0]);
                    Genre g = new Genre(lines[3]);
                    Collection c = new Collection(lines[4]);
                    int d = Convert.ToInt32(lines[5]);
                    Album al = new Album(lines[1], d, g);
                    Track t = new Track(lines[2], a, g, al, c, d);
                    if (!_albums.Contains(al))
                    {
                        al.AddTrack(t);
                        _albums.Add(al);
                    }
                    else
                    {
                        FindAlbum(al).AddTrack(t);
                    }

                    if (!_artists.Contains(a))
                    {
                        a.AddTrack(t);
                        _artists.Add(a);
                    }
                    else
                    {
                        FindArtist(a).AddTrack(t);
                    }

                    if (!_collections.Contains(c))
                    {
                        c.AddTrack(t);
                        _collections.Add(c);
                    }
                    else
                    {
                        FindCollection(c).AddTrack(t);
                    }

                    if (!_genres.Contains(g))
                    {
                        g.AddTrack(t);
                        _genres.Add(g);
                    }
                    else
                    {
                        FindGenre(g).AddTrack(t);
                    }

                    _tracks.Add(t);
                }
            }

            _collections.Sort();
            _genres.Sort();
            _albums.Sort();
            _tracks.Sort();
            _artists.Sort();
        }

        public void LoadGenres(String path)
        {
            using (StreamReader reader = new StreamReader(path))
            {
                String line;
                while ((line = reader.ReadLine()) != null)
                {
                    String[] lines = line.Split(" - ");
                    Genre parent = FindGenre(lines[0]);

                    for (int i = 1; i < lines.Length; i++)
                    {
                        Genre genre = FindGenre(lines[i]);
                        parent.AddKid(genre);
                        genre.AddParent(parent);
                    }
                }
            }
        }

        public void PrintCatalogue()
        {
            Console.WriteLine("[ Catalogue tracks: ]");
            Console.WriteLine(String.Join("\n", _tracks));
            Console.WriteLine("[ Catalogue artists: ]");
            Console.WriteLine(String.Join("\n", _artists));
            Console.WriteLine("[ Catalogue genres: ]");
            Console.WriteLine(String.Join("\n", _genres));
            Console.WriteLine("[ Catalogue collections: ]");
            Console.WriteLine(String.Join("\n", _collections));
            Console.WriteLine("[ Catalogue albums: ]");
            Console.WriteLine(String.Join("\n", _albums));
        }

        Artist FindArtist(Artist other)
        {
            return _artists.Find(x => x.Equals(other));
        }

        Album FindAlbum(Album other)
        {
            return _albums.Find(x => x.Equals(other));
        }

        Genre FindGenre(Genre other)
        {
            return _genres.Find(x => x.Equals(other));
        }

        Genre FindGenre(String name)
        {
            Genre other = new Genre(name);
            return _genres.Find(x => x.Equals(other));
        }

        Collection FindCollection(Collection other)
        {
            return _collections.Find(x => x.Equals(other));
        }

        public void RequestHandler()
        {
            Console.WriteLine("Hey, Comrade! What's up? What parameters do u need?");
            int command = 0;
            while (command != 7)
            {
                Console.WriteLine("1) Track\n2) Artist\n3) Album\n4) Genre\n5) Collection\n6) Year\n7) Exit");
                command = Convert.ToInt32(Console.ReadLine());
                if (command > 7 || command < 1)
                {
                    Console.WriteLine("Wrong input");
                }
                else
                    switch (command)
                    {
                        case 1:
                            var readLine = Console.ReadLine();
                            //var track = new Track(readLine);
                            var tracks = _tracks.Where(x => x.Contains(readLine)).ToList();
                            foreach (var track in tracks)
                            {
                               Console.WriteLine(track);
                            }
                            break;
                        case 2:
                            var artist = new Artist(Console.ReadLine());
                            Console.WriteLine(_artists.Find(x => x.Equals(artist)).ToStringRequest());
                            break;
                        case 3:
                            var album = new Album(Console.ReadLine());
                            Console.WriteLine(_albums.Find(x => x.Equals(album)).ToStringRequest());
                            break;
                        case 4:
                            var genre = new Genre(Console.ReadLine());
                            Console.WriteLine(_genres.Find(x => x.Equals(genre)).ToStringRequest());
                            break;
                        case 5:
                            var collection = new Collection(Console.ReadLine());
                            Console.WriteLine(_collections.Find(x => x.Equals(collection)).ToStringRequest());
                            break;
                        case 6:
                            int year = Convert.ToInt32(Console.ReadLine());
                            List<String> current = new List<String>();
                            foreach (var x in _albums)
                            {
                                if (x.YearComparison(year))
                                    current.Add(x.ToStringRequest());
                            }

                            Console.WriteLine($"Here is What I've found in Albums:\n{String.Join("\n", current)}\n");
                            Console.WriteLine(
                                $"Here is What I've found in Tracks:\n{String.Join("\n", _tracks.FindAll(x => x.YearComparison(year)))}");
                            break;
                        case 7:
                            Console.WriteLine("Bye-Bye!");
                            break;
                        default:
                            Console.WriteLine("error");
                            break;
                    }
            }
        }
    }
}