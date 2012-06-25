// ****************************************************************
// Copyright 2008, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org
// ****************************************************************

using System;
using System.Threading;

namespace NUnit.Framework
{
	// Just dummy
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class | AttributeTargets.Assembly, AllowMultiple = false, Inherited=false)]
    public class TimeoutAttribute : Attribute
    {
        /// <summary>
        /// Construct a TimeoutAttribute given a time in milliseconds
        /// </summary>
        /// <param name="timeout">The timeout value in milliseconds</param>
		public TimeoutAttribute( int timeout ) { }
    }
}
