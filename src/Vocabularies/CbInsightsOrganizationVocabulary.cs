// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FacebookGraphOrganizationVocabulary.cs" company="Clued In">
//   Copyright Clued In
// </copyright>
// <summary>
//   Defines the FacebookGraphOrganizationVocabulary type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using CluedIn.Core.Data;
using CluedIn.Core.Data.Vocabularies;

namespace CluedIn.ExternalSearch.Providers.CbInsights.Vocabularies
{
    /// <summary>The facebook graph organization vocabulary.</summary>
    /// <seealso cref="CluedIn.Core.Data.Vocabularies.SimpleVocabulary" />
    public class CbInsightsOrganizationVocabulary : SimpleVocabulary
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CbInsightsOrganizationVocabulary"/> class.
        /// </summary>
        public CbInsightsOrganizationVocabulary()
        {
            this.VocabularyName = "CbInsights Organization";
            this.KeyPrefix      = "cbinsights.organization";
            this.KeySeparator   = ".";
            this.Grouping       = EntityType.Organization;

            this.Description         = this.Add(new VocabularyKey("description"));
            this.Category            = this.Add(new VocabularyKey("category"));
            this.Mission             = this.Add(new VocabularyKey("mission"));
            this.AcquiredCount = this.Add(new VocabularyKey("acquiredCount", VocabularyKeyDataType.Integer));
            this.Rounds = this.Add(new VocabularyKey("rounds", VocabularyKeyDataType.Integer));
            this.Valuation = this.Add(new VocabularyKey("valuation", VocabularyKeyDataType.Number));
            this.FundingTotal = this.Add(new VocabularyKey("fundingTotal", VocabularyKeyDataType.Number));
            this.CurrentRound = this.Add(new VocabularyKey("currentRound", VocabularyKeyDataType.Integer));
            this.IsIpo = this.Add(new VocabularyKey("isIpo", VocabularyKeyDataType.Boolean));
            this.Categories = this.Add(new VocabularyKey("categories", VocabularyKeyDataType.Text));


            this.NumberOfOpenPositions = this.Add(new VocabularyKey("numberOfOpenPositions", VocabularyKeyDataType.Integer));
            this.MostRecentInvestor = this.Add(new VocabularyKey("mostRecentInvestor", VocabularyKeyDataType.Integer));
        }

        public VocabularyKey Description { get; set; }
        public VocabularyKey Category { get; set; }
        public VocabularyKey Mission { get; set; }
        public VocabularyKey AcquiredCount { get; set; }
        public VocabularyKey Rounds { get; set; }
        public VocabularyKey Valuation { get; set; }
        public VocabularyKey FundingTotal { get; set; }
        public VocabularyKey CurrentRound { get; set; }
        public VocabularyKey IsIpo { get; set; }
        public VocabularyKey Categories { get; set; }
        public VocabularyKey NumberOfOpenPositions { get; set; }
        public VocabularyKey MostRecentInvestor { get; set; }
    }
}
