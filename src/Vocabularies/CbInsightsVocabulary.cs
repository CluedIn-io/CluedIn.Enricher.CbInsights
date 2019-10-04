// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ClearBitVocabulary.cs" company="Clued In">
//   Copyright Clued In
// </copyright>
// <summary>
//   Defines the ClearBitVocabulary type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace CluedIn.ExternalSearch.Providers.CbInsights.Vocabularies
{
    /// <summary>The clear bit vocabulary.</summary>
    public static class CbInsightsVocabulary
    {
        /// <summary>
        /// Initializes static members of the <see cref="CbInsightsVocabulary" /> class.
        /// </summary>
        static CbInsightsVocabulary()
        {
            Organization = new CbInsightsOrganizationVocabulary();
        }

        /// <summary>Gets the organization.</summary>
        /// <value>The organization.</value>
        public static CbInsightsOrganizationVocabulary Organization { get; private set; }
    }
}