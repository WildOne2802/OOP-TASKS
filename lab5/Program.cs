using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using MySql.Data.MySqlClient;

namespace Lab5
{
    class Point
    {
        public readonly Double x;

        public readonly Double y;

        public Point(Double x, Double y)
        {
            this.x = x;
            this.y = y;
        }

        public override String ToString()
        {
            return $"[{x},{y}]";
        }
    }

    class Triangle
    {
        public readonly Point a;
        public readonly Point b;
        public readonly Point c;

        public Triangle(Point a, Point b, Point c)
        {
            this.a = a;
            this.b = b;
            this.c = c;
        }

        public override String ToString()
        {
            return $"Point A: {a.ToString()}\nPoint B: {b.ToString()}\nPoint C: {c.ToString()}";
        }
    }

    interface ISerializer
    {
        void Serialize(String path, Triangle triangle);
        Triangle Deserialize(String path);
    }

    class SerializerXML : ISerializer
    {
        public void Serialize(String path, Triangle triangle)
        {
            Stream stream = File.Create(path);
            XmlSerializer xml = new XmlSerializer(typeof(Triangle));
            xml.Serialize(stream, triangle);
            stream.Close();
        }

        public Triangle Deserialize(String path)
        {
            Stream stream = File.OpenRead(path);
            XmlSerializer xml = new XmlSerializer(typeof(Triangle));
            Triangle result = (Triangle) xml.Deserialize(stream);
            stream.Close();
            return result;
        }
    }

    class SerializerBinary : ISerializer
    {
        public void Serialize(String path, Triangle triangle)
        {
            Stream stream = File.Create(path);
            BinaryFormatter bin = new BinaryFormatter();
            bin.Serialize(stream, triangle);
            stream.Close();
        }

        public Triangle Deserialize(String path)
        {
            Stream stream = File.OpenRead(path);
            BinaryFormatter bin = new BinaryFormatter();
            Triangle result = (Triangle) bin.Deserialize(stream);
            stream.Close();
            return result;
        }
    }

    class DBSaver
    {
        private MySqlConnection connection;

        public DBSaver(String host, Int32 port, String database, String username, String password)
        {
            this.connection =
                new MySqlConnection(
                    $"Server={host}; Database={database}; Port={port}; User Id={username}; Password={password}");
            connection.Open();
        }

        ~DBSaver()
        {
            connection.Close();
        }

        public Boolean PointExistence(Point point)
        {
            String command = $"SELECT COUNT(point_id) FROM triangleSchema.points WHERE x={point.x} AND y={point.y}";
            MySqlCommand sql = new MySqlCommand(command, connection);
            Int32 amount = Int32.Parse(sql.ExecuteScalar().ToString());

            return amount != 0;
        }

        public Int32 GetPointID(Point point)
        {
            String command = $"SELECT point_id FROM triangleSchema.points WHERE x={point.x} AND y={point.y}";
            MySqlCommand sql = new MySqlCommand(command, connection);
            MySqlDataReader reader = sql.ExecuteReader();

            Int32 result = 0;
            while (reader.Read())
            {
                result = Int32.Parse(reader[0].ToString());
            }

            reader.Close();
            return result;
        }

        public Boolean TriangleExistence(Triangle triangle)
        {
            if (PointExistence(triangle.a) && PointExistence(triangle.b) && PointExistence(triangle.c))
            {
                Int32 A = GetPointID(triangle.a);
                Int32 B = GetPointID(triangle.b);
                Int32 C = GetPointID(triangle.c);

                String command =
                    $"SELECT COUNT(triangle_id) FROM triangleSchema.triangles WHERE A={A} AND B={B} AND C={C}";
                MySqlCommand sql = new MySqlCommand(command, connection);
                Int32 count = Int32.Parse(sql.ExecuteScalar().ToString());

                return count != 0;
            }

            return false;
        }

        private Int32 CreatePoint(Point point)
        {
            if (!PointExistence(point))
            {
                String command = $"INSERT INTO triangleSchema.points (x,y) VALUES ({point.x}, {point.y})";
                MySqlCommand sql = new MySqlCommand(command, connection);
                sql.ExecuteNonQuery();
            }

            return GetPointID(point);
        }

        public void SaveTriangle(Triangle triangle)
        {
            if (TriangleExistence(triangle)) return;

            Int32 A = CreatePoint(triangle.a);
            Int32 B = CreatePoint(triangle.b);
            Int32 C = CreatePoint(triangle.c);

            String command = $"INSERT INTO triangleSchema.triangles (A, B, C) VALUES ({A}, {B}, {C})";
            MySqlCommand sql = new MySqlCommand(command, connection);
            sql.ExecuteNonQuery();
        }

        public List<Triangle> GetTriangles()
        {
            String command =
                "SELECT pointA.x, pointA.y, pointB.x, pointB.y, pointC.x, pointC.y FROM ((triangleSchema.triangles AS triangle INNER JOIN triangleSchema.points AS pointA ON triangle.A = pointA.point_id) INNER JOIN triangleSchema.points AS pointB ON triangle.B = pointB.point_id) INNER JOIN triangleSchema.points AS pointC ON triangle.C = pointC.point_id";

            MySqlCommand sql = new MySqlCommand(command, connection);
            MySqlDataReader reader = sql.ExecuteReader();

            List<Triangle> triangles = new List<Triangle>();
            while (reader.Read())
            {
                Point a = new Point(Int32.Parse(reader[0].ToString()), Int32.Parse(reader[1].ToString()));
                Point b = new Point(Int32.Parse(reader[2].ToString()), Int32.Parse(reader[3].ToString()));
                Point c = new Point(Int32.Parse(reader[4].ToString()), Int32.Parse(reader[5].ToString()));
                triangles.Add(new Triangle(a, b, c));
            }

            reader.Close();
            return triangles;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Triangle triangle = new Triangle(new Point(0, 0), new Point(0, 10), new Point(10, 0));

            Console.WriteLine(" === Binary file === ");
            string binPath = @"..\..\SerializedBin.bin";
            SerializerBinary bin = new SerializerBinary();
            bin.Serialize(binPath, triangle);
            Console.WriteLine(bin.Deserialize(binPath).ToString());

            Console.WriteLine(" === Xml file === ");
            string xmlPath = @"..\..\SerializedXml.xml";
            SerializerXML xml = new SerializerXML();
            xml.Serialize(xmlPath, triangle);
            Console.WriteLine(xml.Deserialize(xmlPath).ToString());

            /*Console.WriteLine(" === Data Base === ");
            DBSaver db = new DBSaver("localhost", 3306, "", "root", "yolo2802");
            db.SaveTriangle(triangle);
            List<Triangle> triangles = db.GetTriangles();
            foreach (var tr in triangles) Console.WriteLine(tr.ToString());
            */
        }
    }
}