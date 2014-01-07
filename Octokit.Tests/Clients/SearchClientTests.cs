﻿using System;
using System.Threading.Tasks;
using NSubstitute;
using Octokit.Tests.Helpers;
using Xunit;
using System.Collections.Generic;

namespace Octokit.Tests.Clients
{
    /// <summary>
    /// Client tests mostly just need to make sure they call the IApiConnection with the correct 
    /// relative Uri. No need to fake up the response. All *those* tests are in ApiConnectionTests.cs.
    /// </summary>
    public class SearchClientTests
    {
        public class TheConstructor
        {
            [Fact]
            public void EnsuresNonNullArguments()
            {
                Assert.Throws<ArgumentNullException>(() => new SearchClient(null));
            }
        }

        public class TheSearchUsersMethod
        {
            [Fact]
            public void RequestsTheCorrectUrl()
            {
                var connection = Substitute.For<IApiConnection>();
                var client = new SearchClient(connection);
                client.SearchUsers(new SearchUsersRequest("something"));
                connection.Received().GetAll<User>(Arg.Is<Uri>(u => u.ToString() == "search/users"), Arg.Any<Dictionary<string, string>>());
            }

            [Fact]
            public async Task EnsuresNonNullArguments()
            {
                var client = new SearchClient(Substitute.For<IApiConnection>());
                AssertEx.Throws<ArgumentNullException>(async () => await client.SearchUsers(null));
            }
        }

        public class TheSearchRepoMethod
        {
            [Fact]
            public void RequestsTheCorrectUrl()
            {
                var connection = Substitute.For<IApiConnection>();
                var client = new SearchClient(connection);
                client.SearchRepo(new SearchRepositoriesRequest("something"));
                connection.Received().GetAll<Repository>(Arg.Is<Uri>(u => u.ToString() == "search/repositories"), Arg.Any<Dictionary<string, string>>());
            }

            [Fact]
            public async Task EnsuresNonNullArguments()
            {
                var client = new SearchClient(Substitute.For<IApiConnection>());
                AssertEx.Throws<ArgumentNullException>(async () => await client.SearchRepo(null));
            }

            [Fact]
            public void TestingTheSizeQualifier()
            {
                var connection = Substitute.For<IApiConnection>();
                var client = new SearchClient(connection);
                //check sizes for repos that are greater than 50 MB
                var request = new SearchRepositoriesRequest("github");
                request.Size = Range.GreaterThan(50);

                client.SearchRepo(request);

                connection.Received().GetAll<Repository>(Arg.Is<Uri>(u => u.ToString() == "search/repositories"), Arg.Any<Dictionary<string, string>>());
            }

            [Fact]
            public void TestingTheStarsQualifier()
            {
                var connection = Substitute.For<IApiConnection>();
                var client = new SearchClient(connection);
                //get repos whos stargazers are greater than 500
                var request = new SearchRepositoriesRequest("github");
                request.Stars = Range.GreaterThan(500);

                client.SearchRepo(request);

                connection.Received().GetAll<Repository>(Arg.Is<Uri>(u => u.ToString() == "search/repositories"), Arg.Any<Dictionary<string, string>>());
            }

            [Fact]
            public void TestingTheForksQualifier()
            {
                var connection = Substitute.For<IApiConnection>();
                var client = new SearchClient(connection);
                //get repos which has forks that are greater than 50
                var request = new SearchRepositoriesRequest("github");
                request.Forks = Range.GreaterThan(50);

                client.SearchRepo(request);

                connection.Received().GetAll<Repository>(Arg.Is<Uri>(u => u.ToString() == "search/repositories"), Arg.Any<Dictionary<string, string>>());
            }

            [Fact]
            public void TestingTheForkQualifier()
            {
                var connection = Substitute.For<IApiConnection>();
                var client = new SearchClient(connection);
                //search repos that contains rails and forks are included in the search
                var request = new SearchRepositoriesRequest("rails");
                request.Fork = ForkQualifier.IncludeForks;

                client.SearchRepo(request);

                connection.Received().GetAll<Repository>(Arg.Is<Uri>(u => u.ToString() == "search/repositories"), Arg.Any<Dictionary<string, string>>());
            }

            [Fact]
            public void TestingTheLangaugeQualifier()
            {
                var connection = Substitute.For<IApiConnection>();
                var client = new SearchClient(connection);
                //get repos whos language is Ruby
                var request = new SearchRepositoriesRequest("github");
                request.Language = Language.Ruby;

                client.SearchRepo(request);

                connection.Received().GetAll<Repository>(Arg.Is<Uri>(u => u.ToString() == "search/repositories"), Arg.Any<Dictionary<string, string>>());
            }

            [Fact]
            public void TestingTheInQualifier()
            {
                var connection = Substitute.For<IApiConnection>();
                var client = new SearchClient(connection);
                //get repos where the Description contains the test 'github'
                var request = new SearchRepositoriesRequest("github");
                request.In = new[] { InQualifier.Description };
                client.SearchRepo(request);

                connection.Received().GetAll<Repository>(Arg.Is<Uri>(u => u.ToString() == "search/repositories"), Arg.Any<Dictionary<string, string>>());
            }

            [Fact]
            public void TestingTheCreatedQualifier()
            {
                var connection = Substitute.For<IApiConnection>();
                var client = new SearchClient(connection);
                //get repos where the search contains 'github' and has been created after year jan 1 2011
                var request = new SearchRepositoriesRequest("github");
                request.Created = DateRange.GreaterThan(new DateTime(2011, 1, 1));

                client.SearchRepo(request);

                connection.Received().GetAll<Repository>(Arg.Is<Uri>(u => u.ToString() == "search/repositories"), Arg.Any<Dictionary<string, string>>());
            }

            [Fact]
            public void TestingTheUpdatedQualifier()
            {
                var connection = Substitute.For<IApiConnection>();
                var client = new SearchClient(connection);
                //get repos where the search contains 'github' and has been pushed before year jan 1 2013
                var request = new SearchRepositoriesRequest("github");
                request.Updated = DateRange.LessThan(new DateTime(2013, 1, 1));

                client.SearchRepo(request);

                connection.Received().GetAll<Repository>(Arg.Is<Uri>(u => u.ToString() == "search/repositories"), Arg.Any<Dictionary<string, string>>());
            }

            [Fact]
            public void TestingTheUserQualifier()
            {
                var connection = Substitute.For<IApiConnection>();
                var client = new SearchClient(connection);
                //get repos where the Description contains rails and user/org is 'github'
                var request = new SearchRepositoriesRequest("rails");
                request.User = "github";

                client.SearchRepo(request);

                connection.Received().GetAll<Repository>(Arg.Is<Uri>(u => u.ToString() == "search/repositories"), Arg.Any<Dictionary<string, string>>());
            }

            [Fact]
            public void TestingTheSortParameter()
            {
                var connection = Substitute.For<IApiConnection>();
                var client = new SearchClient(connection);
                //get repos where the Description contains rails and user/org is 'github'
                var request = new SearchRepositoriesRequest("rails");
                request.Sort = RepoSearchSort.Forks;

                client.SearchRepo(request);

                connection.Received().GetAll<Repository>(Arg.Is<Uri>(u => u.ToString() == "search/repositories"), Arg.Any<Dictionary<string, string>>());
            }
        }

        public class TheSearchIssuesMethod
        {
            [Fact]
            public void RequestsTheCorrectUrl()
            {
                var connection = Substitute.For<IApiConnection>();
                var client = new SearchClient(connection);
                client.SearchIssues(new SearchIssuesRequest("something"));
                connection.Received().GetAll<Issue>(Arg.Is<Uri>(u => u.ToString() == "search/issues"), Arg.Any<Dictionary<string, string>>());
            }

            [Fact]
            public async Task EnsuresNonNullArguments()
            {
                var client = new SearchClient(Substitute.For<IApiConnection>());
                AssertEx.Throws<ArgumentNullException>(async () => await client.SearchIssues(null));
            }

            [Fact]
            public void TestingTheTermParameter()
            {
                var connection = Substitute.For<IApiConnection>();
                var client = new SearchClient(connection);
                var request = new SearchIssuesRequest("pub");

                client.SearchIssues(request);

                connection.Received().GetAll<Issue>(Arg.Is<Uri>(u => u.ToString() == "search/issues"), Arg.Is<Dictionary<string, string>>(d => d["q"].StartsWith("pub")));
            }

            [Fact]
            public void TestingTheSortParameter()
            {
                var connection = Substitute.For<IApiConnection>();
                var client = new SearchClient(connection);
                var request = new SearchIssuesRequest("something");
                request.Sort = IssueSearchSort.Comments;

                client.SearchIssues(request);

                connection.Received().GetAll<Issue>(Arg.Is<Uri>(u => u.ToString() == "search/issues"), Arg.Is<Dictionary<string, string>>(d => d["sort"] == IssueSearchSort.Comments.ToString()));
            }

            [Fact]
            public void TestingTheOrderParameter()
            {
                var connection = Substitute.For<IApiConnection>();
                var client = new SearchClient(connection);
                var request = new SearchIssuesRequest("something");
                request.Sort = IssueSearchSort.Comments;
                request.Order = SortDirection.Descending;

                client.SearchIssues(request);

                connection.Received().GetAll<Issue>(Arg.Is<Uri>(u => u.ToString() == "search/issues"), Arg.Is<Dictionary<string, string>>(d => d["order"] == SortDirection.Descending.ToString()));
            }

            [Fact]
            public void TestingTheInQualifier()
            {
                var connection = Substitute.For<IApiConnection>();
                var client = new SearchClient(connection);
                var request = new SearchIssuesRequest("something");
                request.In = new[] { IssueInQualifier.Comment };

                client.SearchIssues(request);

                connection.Received().GetAll<Issue>(Arg.Is<Uri>(u=>u.ToString() == "search/issues"), Arg.Is<Dictionary<string, string>>(d=>d["q"].Contains(String.Format("in:{0}", IssueInQualifier.Comment))));
            }

            [Fact]
            public void TestingTheInQualifiers_Multiple()
            {
                var connection = Substitute.For<IApiConnection>();
                var client = new SearchClient(connection);
                var request = new SearchIssuesRequest("something");
                request.In = new[] { IssueInQualifier.Body, IssueInQualifier.Title };

                client.SearchIssues(request);

                connection.Received().GetAll<Issue>(Arg.Is<Uri>(u => u.ToString() == "search/issues"), Arg.Is<Dictionary<string, string>>(d => d["q"].Contains(String.Format("in:{0},{1}", IssueInQualifier.Body, IssueInQualifier.Title))));
            }

            [Fact]
            public void TestingTheTypeQualifier_Issue()
            {
                var connection = Substitute.For<IApiConnection>();
                var client = new SearchClient(connection);
                var request = new SearchIssuesRequest("something");
                request.Type = IssueTypeQualifier.Issue;

                client.SearchIssues(request);

                connection.Received().GetAll<Issue>(Arg.Is<Uri>(u => u.ToString() == "search/issues"), Arg.Is<Dictionary<string, string>>(d => d["q"].Contains(String.Format("type:{0}", IssueTypeQualifier.Issue))));
            }

            [Fact]
            public void TestingTheTypeQualifier_PR()
            {
                var connection = Substitute.For<IApiConnection>();
                var client = new SearchClient(connection);
                var request = new SearchIssuesRequest("something");
                request.Type = IssueTypeQualifier.PR;

                client.SearchIssues(request);

                connection.Received().GetAll<Issue>(Arg.Is<Uri>(u => u.ToString() == "search/issues"), Arg.Is<Dictionary<string, string>>(d => d["q"].Contains(String.Format("type:{0}", IssueTypeQualifier.PR))));
            }

            [Fact]
            public void TestingTheAuthorQualifier()
            {
                var connection = Substitute.For<IApiConnection>();
                var client = new SearchClient(connection);
                var request = new SearchIssuesRequest("something");
                request.Author = "alfhenrik";

                client.SearchIssues(request);

                connection.Received().GetAll<Issue>(Arg.Is<Uri>(u => u.ToString() == "search/issues"), Arg.Is<Dictionary<string, string>>(d => d["q"].Contains(String.Format("author:{0}", "alfhenrik"))));
            }

            [Fact]
            public void TestingTheAssigneeQualifier()
            {
                var connection = Substitute.For<IApiConnection>();
                var client = new SearchClient(connection);
                var request = new SearchIssuesRequest("something");
                request.Assignee = "alfhenrik";

                client.SearchIssues(request);

                connection.Received().GetAll<Issue>(Arg.Is<Uri>(u => u.ToString() == "search/issues"), Arg.Is<Dictionary<string, string>>(d => d["q"].Contains(String.Format("assignee:{0}", "alfhenrik"))));
            }

            [Fact]
            public void TestingTheMentionsQualifier()
            {
                var connection = Substitute.For<IApiConnection>();
                var client = new SearchClient(connection);
                var request = new SearchIssuesRequest("something");
                request.Mentions = "alfhenrik";

                client.SearchIssues(request);

                connection.Received().GetAll<Issue>(Arg.Is<Uri>(u => u.ToString() == "search/issues"), Arg.Is<Dictionary<string, string>>(d => d["q"].Contains(String.Format("mentions:{0}", "alfhenrik"))));
            }

            [Fact]
            public void TestingTheCommenterQualifier()
            {
                var connection = Substitute.For<IApiConnection>();
                var client = new SearchClient(connection);
                var request = new SearchIssuesRequest("something");
                request.Commenter = "alfhenrik";

                client.SearchIssues(request);

                connection.Received().GetAll<Issue>(Arg.Is<Uri>(u => u.ToString() == "search/issues"), Arg.Is<Dictionary<string, string>>(d => d["q"].Contains(String.Format("commenter:{0}", "alfhenrik"))));
            }

            [Fact]
            public void TestingTheInvolvesQualifier()
            {
                var connection = Substitute.For<IApiConnection>();
                var client = new SearchClient(connection);
                var request = new SearchIssuesRequest("something");
                request.Involves = "alfhenrik";

                client.SearchIssues(request);

                connection.Received().GetAll<Issue>(Arg.Is<Uri>(u => u.ToString() == "search/issues"), Arg.Is<Dictionary<string, string>>(d => d["q"].Contains(String.Format("involves:{0}", "alfhenrik"))));
            }

            [Fact]
            public void TestingTheStateQualifier_Open()
            {
                var connection = Substitute.For<IApiConnection>();
                var client = new SearchClient(connection);
                var request = new SearchIssuesRequest("something");
                request.State = ItemState.Open;

                client.SearchIssues(request);

                connection.Received().GetAll<Issue>(Arg.Is<Uri>(u => u.ToString() == "search/issues"), Arg.Is<Dictionary<string, string>>(d => d["q"].Contains(String.Format("state:{0}", ItemState.Open.ToString()))));
            }

            [Fact]
            public void TestingTheStateQualifier_Closed()
            {
                var connection = Substitute.For<IApiConnection>();
                var client = new SearchClient(connection);
                var request = new SearchIssuesRequest("something");
                request.State = ItemState.Closed;

                client.SearchIssues(request);

                connection.Received().GetAll<Issue>(Arg.Is<Uri>(u => u.ToString() == "search/issues"), Arg.Is<Dictionary<string, string>>(d => d["q"].Contains(String.Format("state:{0}", ItemState.Closed.ToString()))));
            }

            [Fact]
            public void TestingTheLabelsQualifier()
            {
                var connection = Substitute.For<IApiConnection>();
                var client = new SearchClient(connection);
                var request = new SearchIssuesRequest("something");
                request.Labels = new[] { "bug" };

                client.SearchIssues(request);

                connection.Received().GetAll<Issue>(Arg.Is<Uri>(u => u.ToString() == "search/issues"), Arg.Is<Dictionary<string, string>>(d => d["q"].Contains("label:bug")));
            }

            [Fact]
            public void TestingTheLabelsQualifier_Multiple()
            {
                var connection = Substitute.For<IApiConnection>();
                var client = new SearchClient(connection);
                var request = new SearchIssuesRequest("something");
                request.Labels = new[] { "bug", "feature" };

                client.SearchIssues(request);

                connection.Received().GetAll<Issue>(Arg.Is<Uri>(u => u.ToString() == "search/issues"), Arg.Is<Dictionary<string, string>>(d => d["q"].Contains("label:bug") && d["q"].Contains("label:feature")));
            }

            [Fact]
            public void TestingTheLanguageQualifier()
            {
                var connection = Substitute.For<IApiConnection>();
                var client = new SearchClient(connection);
                var request = new SearchIssuesRequest("something");
                request.Language = Language.CSharp;

                client.SearchIssues(request);

                connection.Received().GetAll<Issue>(Arg.Is<Uri>(u => u.ToString() == "search/issues"), Arg.Is<Dictionary<string, string>>(d => d["q"].Contains(String.Format("language:{0}", Language.CSharp.ToString()))));
            }
            
            [Fact]
            public void TestingTheLanguageQualifier_None()
            {
                var connection = Substitute.For<IApiConnection>();
                var client = new SearchClient(connection);
                var request = new SearchIssuesRequest("something");

                client.SearchIssues(request);

                connection.Received().GetAll<Issue>(Arg.Is<Uri>(u => u.ToString() == "search/issues"), Arg.Is<Dictionary<string, string>>(d => !d["q"].Contains("language:")));
            }

            [Fact(Skip = "This will fail due to incorrect date format from DateRange, fix available in pending PR #293")]
            public void TestingTheCreatedQualifier_GreaterThan()
            {
                var connection = Substitute.For<IApiConnection>();
                var client = new SearchClient(connection);
                var request = new SearchIssuesRequest("something");
                request.Created = DateRange.GreaterThan(new DateTime(2014, 1, 1));

                client.SearchIssues(request);

                connection.Received().GetAll<Issue>(Arg.Is<Uri>(u => u.ToString() == "search/issues"), Arg.Is<Dictionary<string, string>>(d => d["q"].Contains("created:>2014-01-01")));
            }

            [Fact(Skip = "This will fail due to incorrect date format from DateRange, fix available in pending PR #293")]
            public void TestingTheCreatedQualifier_GreaterThanOrEquals()
            {
                var connection = Substitute.For<IApiConnection>();
                var client = new SearchClient(connection);
                var request = new SearchIssuesRequest("something");
                request.Created = DateRange.GreaterThanOrEquals(new DateTime(2014, 1, 1));

                client.SearchIssues(request);

                connection.Received().GetAll<Issue>(Arg.Is<Uri>(u => u.ToString() == "search/issues"), Arg.Is<Dictionary<string, string>>(d => d["q"].Contains("created:>=2014-01-01")));
            }

            [Fact(Skip = "This will fail due to incorrect date format from DateRange, fix available in pending PR #293")]
            public void TestingTheCreatedQualifier_LessThan()
            {
                var connection = Substitute.For<IApiConnection>();
                var client = new SearchClient(connection);
                var request = new SearchIssuesRequest("something");
                request.Created = DateRange.LessThan(new DateTime(2014, 1, 1));

                client.SearchIssues(request);

                connection.Received().GetAll<Issue>(Arg.Is<Uri>(u => u.ToString() == "search/issues"), Arg.Is<Dictionary<string, string>>(d => d["q"].Contains("created:<2014-01-01")));
            }

            [Fact(Skip = "This will fail due to incorrect date format from DateRange, fix available in pending PR #293")]
            public void TestingTheCreatedQualifier_LessThanOrEquals()
            {
                var connection = Substitute.For<IApiConnection>();
                var client = new SearchClient(connection);
                var request = new SearchIssuesRequest("something");
                request.Created = DateRange.LessThanOrEquals(new DateTime(2014, 1, 1));

                client.SearchIssues(request);

                connection.Received().GetAll<Issue>(Arg.Is<Uri>(u => u.ToString() == "search/issues"), Arg.Is<Dictionary<string, string>>(d => d["q"].Contains("created:<=2014-01-01")));
            }

            [Fact(Skip = "This will fail due to incorrect date format from DateRange, fix available in pending PR #293")]
            public void TestingTheUpdatedQualifier_GreaterThan()
            {
                var connection = Substitute.For<IApiConnection>();
                var client = new SearchClient(connection);
                var request = new SearchIssuesRequest("something");
                request.Updated = DateRange.GreaterThan(new DateTime(2014, 1, 1));

                client.SearchIssues(request);

                connection.Received().GetAll<Issue>(Arg.Is<Uri>(u => u.ToString() == "search/issues"), Arg.Is<Dictionary<string, string>>(d => d["q"].Contains("updated:>2014-01-01")));
            }

            [Fact(Skip = "This will fail due to incorrect date format from DateRange, fix available in pending PR #293")]
            public void TestingTheUpdatedQualifier_GreaterThanOrEquals()
            {
                var connection = Substitute.For<IApiConnection>();
                var client = new SearchClient(connection);
                var request = new SearchIssuesRequest("something");
                request.Updated = DateRange.GreaterThanOrEquals(new DateTime(2014, 1, 1));

                client.SearchIssues(request);

                connection.Received().GetAll<Issue>(Arg.Is<Uri>(u => u.ToString() == "search/issues"), Arg.Is<Dictionary<string, string>>(d => d["q"].Contains("updated:>=2014-01-01")));
            }

            [Fact(Skip = "This will fail due to incorrect date format from DateRange, fix available in pending PR #293")]
            public void TestingTheUpdatedQualifier_LessThan()
            {
                var connection = Substitute.For<IApiConnection>();
                var client = new SearchClient(connection);
                var request = new SearchIssuesRequest("something");
                request.Updated = DateRange.LessThan(new DateTime(2014, 1, 1));

                client.SearchIssues(request);

                connection.Received().GetAll<Issue>(Arg.Is<Uri>(u => u.ToString() == "search/issues"), Arg.Is<Dictionary<string, string>>(d => d["q"].Contains("updated:<2014-01-01")));
            }

            [Fact(Skip = "This will fail due to incorrect date format from DateRange, fix available in pending PR #293")]
            public void TestingTheUpdatedQualifier_LessThanOrEquals()
            {
                var connection = Substitute.For<IApiConnection>();
                var client = new SearchClient(connection);
                var request = new SearchIssuesRequest("something");
                request.Updated = DateRange.LessThanOrEquals(new DateTime(2014, 1, 1));

                client.SearchIssues(request);

                connection.Received().GetAll<Issue>(Arg.Is<Uri>(u => u.ToString() == "search/issues"), Arg.Is<Dictionary<string, string>>(d => d["q"].Contains("updated:<=2014-01-01")));
            }

            [Fact]
            public void TestingTheCommentsQualifier_GreaterThan()
            {
                var connection = Substitute.For<IApiConnection>();
                var client = new SearchClient(connection);
                var request = new SearchIssuesRequest("something");
                request.Comments = Range.GreaterThan(10);

                client.SearchIssues(request);

                connection.Received().GetAll<Issue>(Arg.Is<Uri>(u => u.ToString() == "search/issues"), Arg.Is<Dictionary<string, string>>(d => d["q"].Contains("comments:>10")));
            }

            [Fact]
            public void TestingTheCommentsQualifier_GreaterThanOrEqual()
            {
                var connection = Substitute.For<IApiConnection>();
                var client = new SearchClient(connection);
                var request = new SearchIssuesRequest("something");
                request.Comments = Range.GreaterThanOrEquals(10);

                client.SearchIssues(request);

                connection.Received().GetAll<Issue>(Arg.Is<Uri>(u => u.ToString() == "search/issues"), Arg.Is<Dictionary<string, string>>(d => d["q"].Contains("comments:>=10")));
            }

            [Fact]
            public void TestingTheCommentsQualifier_LessThan()
            {
                var connection = Substitute.For<IApiConnection>();
                var client = new SearchClient(connection);
                var request = new SearchIssuesRequest("something");
                request.Comments = Range.LessThan(10);

                client.SearchIssues(request);

                connection.Received().GetAll<Issue>(Arg.Is<Uri>(u => u.ToString() == "search/issues"), Arg.Is<Dictionary<string, string>>(d => d["q"].Contains("comments:<10")));
            }

            [Fact]
            public void TestingTheCommentsQualifier_LessThanOrEqual()
            {
                var connection = Substitute.For<IApiConnection>();
                var client = new SearchClient(connection);
                var request = new SearchIssuesRequest("something");
                request.Comments = Range.LessThanOrEquals(10);

                client.SearchIssues(request);

                connection.Received().GetAll<Issue>(Arg.Is<Uri>(u => u.ToString() == "search/issues"), Arg.Is<Dictionary<string, string>>(d => d["q"].Contains("comments:<=10")));
            }

            [Fact]
            public void TestingTheCommentsQualifier_Range()
            {
                var connection = Substitute.For<IApiConnection>();
                var client = new SearchClient(connection);
                var request = new SearchIssuesRequest("something");
                request.Comments = new Range(10, 20);

                client.SearchIssues(request);

                connection.Received().GetAll<Issue>(Arg.Is<Uri>(u => u.ToString() == "search/issues"), Arg.Is<Dictionary<string, string>>(d => d["q"].Contains("comments:10..20")));
            }

            [Fact]
            public void TestingTheUserQualifier()
            {
                var connection = Substitute.For<IApiConnection>();
                var client = new SearchClient(connection);
                var request = new SearchIssuesRequest("something");
                request.User = "alfhenrik";

                client.SearchIssues(request);

                connection.Received().GetAll<Issue>(Arg.Is<Uri>(u => u.ToString() == "search/issues"), Arg.Is<Dictionary<string, string>>(d => d["q"].Contains("user:alfhenrik")));
            }

            [Fact]
            public void TestingTheRepoQualifier()
            {
                var connection = Substitute.For<IApiConnection>();
                var client = new SearchClient(connection);
                var request = new SearchIssuesRequest("something");
                request.Repo = "octokit.net";

                client.SearchIssues(request);

                connection.Received().GetAll<Issue>(Arg.Is<Uri>(u => u.ToString() == "search/issues"), Arg.Is<Dictionary<string, string>>(d => d["q"].Contains("repo:octokit.net")));
            }
        }

        public class TheSearchCodeMethod
        {
            [Fact]
            public void RequestsTheCorrectUrl()
            {
                var connection = Substitute.For<IApiConnection>();
                var client = new SearchClient(connection);
                client.SearchCode(new SearchCodeRequest("something"));
                connection.Received().GetAll<SearchCode>(Arg.Is<Uri>(u => u.ToString() == "search/code"), Arg.Any<Dictionary<string, string>>());
            }

            [Fact]
            public async Task EnsuresNonNullArguments()
            {
                var client = new SearchClient(Substitute.For<IApiConnection>());
                AssertEx.Throws<ArgumentNullException>(async () => await client.SearchCode(null));
            }
        }
    }
}
