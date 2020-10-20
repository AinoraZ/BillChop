﻿using System;
using System.Net;

namespace BillChopBE.Exceptions
{
    public class BadRequestException : AbstractUserFriendlyException
    {
        public override HttpStatusCode StatusCode => HttpStatusCode.BadRequest;

        public BadRequestException() : base()
        {
        }

        public BadRequestException(string message) : base(message)
        {
        }

        public BadRequestException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}
