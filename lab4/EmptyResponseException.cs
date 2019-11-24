using System;
using System.IO;

namespace Lab4
{
    public class EmptyResponseException : FileNotFoundException
    {
        public override string Message
        {
            get { return "No such cortages"; }
        }
    }
}