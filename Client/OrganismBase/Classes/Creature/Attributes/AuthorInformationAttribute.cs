//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                               
//------------------------------------------------------------------------------

using System;

namespace OrganismBase
{
    /// <summary>
    ///  <para>
    ///   This attribute is required to be present on all creatures so that
    ///   a creature's author can be identified for prize competitions.  It accepts
    ///   both an Author Name, and an Email.  The Author Name will be used on
    ///   charting pages and top x graphs, while the Email will ONLY be made available
    ///   to the Terrarium team to contact users for prizes.
    ///  </para>
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly, Inherited = true, AllowMultiple = false)]
    public sealed class AuthorInformationAttribute : Attribute
    {
        /// <summary>
        ///  <para>
        ///   This overload allows the user to set only the Author Name field.
        ///   In this way an author doesn't have to give out their personal
        ///   email address.
        ///  </para>
        /// </summary>
        /// <param name="authorName">
        ///  The creature author's name.  Should be a name that can be displayed on a top x population chart.
        /// </param>
        public AuthorInformationAttribute(string authorName)
        {
            AuthorName = authorName;
            AuthorEmail = "";
        }

        /// <summary>
        ///  <para>
        ///   This overload allows you to easily set both your name and email on
        ///   your creature.  This is the recommended usage of the attribute whenever
        ///   introducing a new creature into the EcoSystem.
        ///  </para>
        /// </summary>
        /// <param name="authorName">
        ///  The creature author's name.  Should be a name that can be displayed on a top x population chart.
        /// </param>
        /// <param name="authorEmail">
        ///  The creature author's email.  Should be a valid email the Terrarium team can use for contact.
        /// </param>
        public AuthorInformationAttribute(string authorName, string authorEmail)
        {
            AuthorName = authorName;
            AuthorEmail = authorEmail;
        }

        /// <summary>
        ///  Read-only access to the name of the author as specified in the original
        ///  attribute.
        /// </summary>
        public string AuthorName { get; private set; }

        /// <summary>
        ///  Read-only access to the email of the author as specified in the original
        ///  attribute.
        /// </summary>
        public string AuthorEmail { get; private set; }
    }
}