using System;

namespace Lab4
{
    public class NotEnoughException : Exception
    {
        public override string Message
        {
            get { return "Not enough items to buying consignment"; }
        }
    }
}