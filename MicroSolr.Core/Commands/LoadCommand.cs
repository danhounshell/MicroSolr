﻿// -----------------------------------------------------------------------
// <copyright file="LoadCommand.cs" company="EF">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace MicroSolr.Core.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class LoadCommand : ILoadCommand
    {
        public FormatType ResponseFormat 
        {
            get;
            set;
        }

        public long StartIndex
        {
            get;
            set;
        }

        public long MaxRows
        {
            get;
            set;
        }

        public bool GetAll
        {
            get;
            set;
        }

        public string Query
        {
            get;
            set;
        }

        public string FilterQuery
        {
            get;
            set;
        }

        public string FieldFilter
        {
            get;
            set;
        }
    }
}
