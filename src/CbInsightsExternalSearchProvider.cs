// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FacebookGraphExternalSearchProvider.cs" company="Clued In">
//   Copyright Clued In
// </copyright>
// <summary>
//   Defines the FacebookGraphExternalSearchProvider type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;

using CluedIn.Core;
using CluedIn.Core.Data;
using CluedIn.Core.Data.Parts;
using CluedIn.ExternalSearch.Filters;
using CluedIn.ExternalSearch.Providers.CbInsights.Models;
using RestSharp;

namespace CluedIn.ExternalSearch.Providers.CbInsights
{
    /// <summary>The facebook graph external search provider.</summary>
    /// <seealso cref="CluedIn.ExternalSearch.ExternalSearchProviderBase" />
    public class CbInsightsExternalSearchProvider : ExternalSearchProviderBase
    {
        /**********************************************************************************************************
         * FIELDS
         **********************************************************************************************************/

        /// <summary>The shared API tokens</summary>
        private List<string> sharedApiTokens = new List<string>()
            {
                // ConfigurationManager.AppSettings["Providers.ExternalSearch.Facebook.ApiToken"];

                // TIW
                // "EAAWNms1kKNIBAJeYSAt9kfRp9N4Jib8NA1hRcZAwFX6eG3q0W1rNpSIlHYGus47vEHgvNPcGPCt9v2xKfrZA4ZCvXg4PnWGdRZAymlcedZB6rbeIIZAKOiUfaRMwRHGWkfhTCZC1ZAE341kiL0OZC3ZCetZCZCVgjDwQQRkZD",

                // MSH
                "EAAWNms1kKNIBADbgSiN3IBft9bLbZBvkrZAPepZBOMidgVtIu7UQTDDhwZAtcMISuBZAC4ZBE51wyn3j3pUsZBDvQbavGGZAnR5JZBF0bGV26PSZBTUlJmf6RclAQZApFMYZAZC8lByYal34UwJllZBBSZBAM5Fy94zrjZCOxg0ZD",
            };

        /// <summary>The shared API tokens index</summary>
        private int sharedApiTokensIdx = 0;

        /**********************************************************************************************************
         * CONSTRUCTORS
         **********************************************************************************************************/

        /// <summary>
        /// Initializes a new instance of the <see cref="FacebookGraphExternalSearchProvider" /> class.
        /// </summary>
        public CbInsightsExternalSearchProvider()
            : base(Constants.ExternalSearchProviders.CbInsightsId, EntityType.Organization)
        {
        }

        /**********************************************************************************************************
         * METHODS
         **********************************************************************************************************/

        /// <summary>Builds the queries.</summary>
        /// <param name="context">The context.</param>
        /// <param name="request">The request.</param>
        /// <returns>The search queries.</returns>
        public override IEnumerable<IExternalSearchQuery> BuildQueries(ExecutionContext context, IExternalSearchRequest request)
        {
            yield break;

            #region TODO CbInsightsExternalSearchProvider.BuildQueries(ExecutionContext context, IExternalSearchRequest request) is disabled in code ... review

            //if (!this.Accepts(request.EntityMetaData.EntityType))
            //    yield break;

            //var existingResults = request.GetQueryResults<FacebookResponse>(this).ToList();

            //Func<string, bool> nameFilter = value => OrganizationFilters.NameFilter(context, value) || existingResults.Any(r => string.Equals(r.Data.name, value, StringComparison.InvariantCultureIgnoreCase));

            //// Query Input
            //var entityType       = request.EntityMetaData.EntityType;
            //var organizationName = request.QueryParameters.GetValue(CluedIn.Core.Data.Vocabularies.Vocabularies.CluedInOrganization.OrganizationName, new HashSet<string>());
            //var facebookUrl      = request.QueryParameters.GetValue(CluedIn.Core.Data.Vocabularies.Vocabularies.CluedInOrganization.Social.Facebook, new HashSet<string>());

            //if (!string.IsNullOrEmpty(request.EntityMetaData.Name))
            //    organizationName.Add(request.EntityMetaData.Name);
            //if (!string.IsNullOrEmpty(request.EntityMetaData.DisplayName))
            //    organizationName.Add(request.EntityMetaData.DisplayName);

            //var namesFromUrls = new HashSet<string>();

            //Uri facebook;

            //foreach (var possibleUrl in facebookUrl)
            //{
            //    if (Uri.IsWellFormedUriString(possibleUrl, UriKind.Absolute))
            //    {
            //        if (Uri.TryCreate(possibleUrl, UriKind.Absolute, out facebook))
            //        {
            //            var lastSegment = facebook.Segments.Last();
            //            lastSegment = Regex.Replace(lastSegment, "/+$", string.Empty);

            //            if (!string.IsNullOrEmpty(lastSegment))
            //                namesFromUrls.Add(lastSegment);
            //        }
            //    }
            //    else
            //    {
            //        namesFromUrls.Add(possibleUrl);
            //    }
            //}

            //if (namesFromUrls.Any())
            //{
            //    var values = organizationName.Where(n => n != null);

            //    foreach (var value in values.Where(v => !nameFilter(v)))
            //        yield return new ExternalSearchQuery(this, entityType, ExternalSearchQueryParameter.Name, value);
            //}
            //else if (organizationName.Any())
            //{
            //    var values = organizationName.Where(n => n != null)
            //                                 .GetOrganizationNameVariants()
            //                                 .Select(NameNormalization.Normalize)
            //                                 .GetFacebookNameVariants()
            //                                 .ToHashSet();

            //    foreach (var value in values.Where(v => !nameFilter(v)))
            //        yield return new ExternalSearchQuery(this, entityType, ExternalSearchQueryParameter.Name, value);
            //}

            #endregion
        }

        /// <summary>Executes the search.</summary>
        /// <param name="context">The context.</param>
        /// <param name="query">The query.</param>
        /// <returns>The results.</returns>
        public override IEnumerable<IExternalSearchQueryResult> ExecuteSearch(ExecutionContext context, IExternalSearchQuery query)
        {
            var name = query.QueryParameters[ExternalSearchQueryParameter.Name].FirstOrDefault();

            if (string.IsNullOrEmpty(name))
                yield break;

            name = HttpUtility.UrlEncode(name);

            var client = new RestClient("https://graph.facebook.com/v2.8");

            string sharedApiToken;

            lock (this)
            {
                sharedApiToken = this.sharedApiTokens[this.sharedApiTokensIdx++];

                if (this.sharedApiTokensIdx >= this.sharedApiTokens.Count)
                    this.sharedApiTokensIdx = 0;
            }

            var request = new RestRequest(string.Format("{0}?access_token={1}&fields=about,name,affiliation,app_id,app_links,artists_we_like,attire,awards,band_interests,band_members,best_page,bio,birthday,booking_agent,built,business,category,category_list,company_overview,contact_address,context,cover,culinary_team,current_location,description,description_html,directed_by,display_subtext,emails,engagement,fan_count,featured_video,features,food_styles,founded,general_info,general_manager,genre,global_brand_page_name,global_brand_root_id,hometown,hours,impressum,influences,is_always_open,is_community_page,is_permanently_closed,link,location,mission,overall_star_rating,parent_page,parking,payment_options,personal_info,personal_interests,phone,place_type,press_contact,products,public_transit,username,voip_info,website", name, sharedApiToken), Method.GET);

            var response = client.ExecuteTaskAsync<FacebookResponse>(request).Result;

            if (response.StatusCode == HttpStatusCode.OK)
            {
                if (response.Data != null)
                    yield return new ExternalSearchQueryResult<FacebookResponse>(query, response.Data);
            }
            else if (response.StatusCode == HttpStatusCode.NoContent || response.StatusCode == HttpStatusCode.NotFound)
                yield break;
            else if (response.ErrorException != null)
                throw new AggregateException(response.ErrorException.Message, response.ErrorException);
            else
                throw new ApplicationException("Could not execute external search query - StatusCode:" + response.StatusCode + "; Content: " + response.Content);
        }

        /// <summary>Builds the clues.</summary>
        /// <param name="context">The context.</param>
        /// <param name="query">The query.</param>
        /// <param name="result">The result.</param>
        /// <param name="request">The request.</param>
        /// <returns>The clues.</returns>
        public override IEnumerable<Clue> BuildClues(ExecutionContext context, IExternalSearchQuery query, IExternalSearchQueryResult result, IExternalSearchRequest request)
        {
            var resultItem = result.As<FacebookResponse>();

            var code = this.GetOriginEntityCode(resultItem);

            var clue = new Clue(code, context.Organization);

            this.PopulateMetadata(clue.Data.EntityData, resultItem);

            return new[] { clue };
        }

        /// <summary>Gets the primary entity metadata.</summary>
        /// <param name="context">The context.</param>
        /// <param name="result">The result.</param>
        /// <param name="request">The request.</param>
        /// <returns>The primary entity metadata.</returns>
        public override IEntityMetadata GetPrimaryEntityMetadata(ExecutionContext context, IExternalSearchQueryResult result, IExternalSearchRequest request)
        {
            var resultItem = result.As<FacebookResponse>();
            return this.CreateMetadata(resultItem);
        }

        /// <summary>Gets the preview image.</summary>
        /// <param name="context">The context.</param>
        /// <param name="result">The result.</param>
        /// <param name="request">The request.</param>
        /// <returns>The preview image.</returns>
        public override IPreviewImage GetPrimaryEntityPreviewImage(ExecutionContext context, IExternalSearchQueryResult result, IExternalSearchRequest request)
        {
            return null;
        }

        /// <summary>Creates the metadata.</summary>
        /// <param name="resultItem">The result item.</param>
        /// <returns>The metadata.</returns>
        private IEntityMetadata CreateMetadata(IExternalSearchQueryResult<FacebookResponse> resultItem)
        {
            var metadata = new EntityMetadataPart();

            this.PopulateMetadata(metadata, resultItem);

            return metadata;
        }

        /// <summary>Gets the origin entity code.</summary>
        /// <param name="resultItem">The result item.</param>
        /// <returns>The origin entity code.</returns>
        private EntityCode GetOriginEntityCode(IExternalSearchQueryResult<FacebookResponse> resultItem)
        {
            return new EntityCode(EntityType.Organization, this.GetCodeOrigin(), resultItem.Data.id);
        }

        /// <summary>Gets the code origin.</summary>
        /// <returns>The code origin</returns>
        private CodeOrigin GetCodeOrigin()
        {
            return CodeOrigin.CluedIn.CreateSpecific("facebookGraph");
        }

        /// <summary>Populates the metadata.</summary>
        /// <param name="metadata">The metadata.</param>
        /// <param name="resultItem">The result item.</param>
        private void PopulateMetadata(IEntityMetadata metadata, IExternalSearchQueryResult<FacebookResponse> resultItem)
        {
            var code = this.GetOriginEntityCode(resultItem);

            metadata.EntityType       = EntityType.Organization;
            metadata.Name             = resultItem.Data.name;
            metadata.Description      = resultItem.Data.description;
            metadata.OriginEntityCode = code;

            metadata.Codes.Add(code);

        }
    }
}
