using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NationalCookies.Data
{
    public class CosmosDBConnector
    {
        // The Cosmos DB endpoint and access key
        private readonly string _endpointUri;
        private readonly string _accessKey;

        // The name of the database and collections
        private readonly string _databaseName;
        private readonly string _cookieCollectionName;
        private readonly string _orderCollectionName;

        private static readonly string _cookiePartitionKey = "/Name";
        private static readonly string _orderPartitionKey = "/Status";

        private readonly DocumentClient _client;


        public CosmosDBConnector(string endpointUri,
                                 string accessKey,
                                 string databaseName,
                                 string cookieCollectionName,
                                 string orderCollectionName)
        {
            this._endpointUri = endpointUri;
            this._accessKey = accessKey;
            this._databaseName = databaseName;
            this._cookieCollectionName = cookieCollectionName;
            this._orderCollectionName = orderCollectionName;

            this._client = new DocumentClient(new Uri(this._endpointUri), this._accessKey);

            ResourceResponse<Database> database = 
                this._client.CreateDatabaseIfNotExistsAsync(new Database { Id = this._databaseName }).Result;


            DocumentCollection orderCollection = new DocumentCollection
            {
                Id = this._orderCollectionName
            };
            orderCollection.PartitionKey.Paths.Add(_cookiePartitionKey);

            this._client.CreateDocumentCollectionAsync(
                UriFactory.CreateDatabaseUri(this._databaseName), orderCollection);


            DocumentCollection cookieCollection = new DocumentCollection
            {
                Id = this._cookieCollectionName
            };
            cookieCollection.PartitionKey.Paths.Add(_orderPartitionKey);

            this._client.CreateDocumentCollectionAsync(
                UriFactory.CreateDatabaseUri(this._databaseName), cookieCollection);
        }


        /// <summary>
        /// Execute a SQL query that gets all cookies
        /// </summary>
        public List<Cookie> RetrieveAllCookies()
        {
            List<Cookie> cookies = new List<Cookie>();

            // Set some common query options
            FeedOptions queryOptions = new FeedOptions { MaxItemCount = -1, EnableCrossPartitionQuery = true };

            // Run a simple query via LINQ. 
            // Cosmos DB indexes all properties, so queries can be completed efficiently and with low latency.
            IQueryable<Cookie> cookieQuery = this._client.CreateDocumentQuery<Cookie>(
                UriFactory.CreateDocumentCollectionUri(
                    this._databaseName,
                    this._cookieCollectionName), queryOptions);


            if (cookieQuery.AsEnumerable().FirstOrDefault() == null)
            {
                InitializeCookies();

                cookieQuery = this._client.CreateDocumentQuery<Cookie>(
                     UriFactory.CreateDocumentCollectionUri(
                         this._databaseName,
                         this._cookieCollectionName), queryOptions);
            }

            // The query is executed synchronously here, 
            // But can also be executed asynchronously via the IDocumentQuery<T> interface
            cookies = cookieQuery.ToList();

            return cookies;
        }

        /// <summary>
        /// Execute a SQL query that gets an cookie by id
        /// </summary>
        private Cookie RetrieveCookieById(Guid cookieId)
        {
            FeedOptions queryOptions = new FeedOptions { MaxItemCount = -1, EnableCrossPartitionQuery = true };

            //retrieve the cookie by Id
            Cookie cookie = this._client.CreateDocumentQuery<Cookie>(
                UriFactory.CreateDocumentCollectionUri(
                    this._databaseName,
                    this._cookieCollectionName), queryOptions)
                    .Where(c => c.Id == cookieId)
                    .AsEnumerable()
                    .FirstOrDefault();

            return cookie;
        }

        /// <summary>
        /// Populates Cosmos DB with cookies
        /// </summary>
        private void InitializeCookies()
        {
            //define the cookie objects
            Cookie cookieChololateChip = new Cookie
            {
                Id = Guid.NewGuid(),
                ImageUrl = "https://intcookie.azureedge.net/cdn/cookie-cc.jpg",
                Name = "Chololate Chip",
                Price = 1.2
            };

            Cookie cookieButterCookie = new Cookie
            {
                Id = Guid.NewGuid(),
                ImageUrl = "https://intcookie.azureedge.net/cdn/cookie-bc.jpg",
                Name = "Butter Cookie",
                Price = 1.0
            };

            Cookie cookieMacaroon = new Cookie
            {
                Id = Guid.NewGuid(),
                ImageUrl = "https://intcookie.azureedge.net/cdn/cookie-mc.jpg",
                Name = "Macaroons",
                Price = 0.9
            };

            //add cookies to Cosmos DB
            CreateDocument(this._cookieCollectionName, cookieChololateChip);
            CreateDocument(this._cookieCollectionName, cookieButterCookie);
            CreateDocument(this._cookieCollectionName, cookieMacaroon);
        }

        /// <summary>
        /// Creates a new order and adds a cookie to it
        /// </summary>
        /// <param name="cookieId">The id of the cookie to add to the order</param>
        public void AddCookieToOrder(Guid cookieId)
        {
            // Set some common query options
            FeedOptions queryOptions = new FeedOptions { MaxItemCount = -1, EnableCrossPartitionQuery = true };

            // Run a simple query via LINQ. 
            // Cosmos DB indexes all properties, so queries can be completed efficiently and with low latency.
            IQueryable<Order> orderQuery = this._client.CreateDocumentQuery<Order>(
                UriFactory.CreateDocumentCollectionUri(
                    this._databaseName,
                    this._orderCollectionName), queryOptions)
                    .Where(o => o.Status == "New");

            Order currentOrder = orderQuery.AsEnumerable().FirstOrDefault();

            if (currentOrder != null)
            {
                //loop through the lines of the order
                //and check if the cookie that we want to add is
                //already in there
                var orderLineExists = false;
                foreach (OrderLine lines in currentOrder.OrderLines)
                {
                    if (lines.Cookie.Id == cookieId)
                    {
                        lines.Quantity++;
                        orderLineExists = true;

                        currentOrder.Price += lines.Cookie.Price;
                    }
                }

                //if the cookie is new in this order
                if (!orderLineExists)
                {
                    //get the cookie
                    Cookie cookie = RetrieveCookieById(cookieId);

                    //add it to a new orderline
                    currentOrder.OrderLines.Add(new OrderLine
                    {
                        Cookie = cookie,
                        Quantity = 1
                    });

                    currentOrder.Price += cookie.Price;
                }

                this._client.ReplaceDocumentAsync(
                    UriFactory.CreateDocumentUri(
                    this._databaseName,
                    this._orderCollectionName,
                    currentOrder.Id.ToString()),
                    currentOrder);
            }
            else
            {
                //if there is no order with status new
                //create one
                currentOrder = new Order
                {
                    Id = Guid.NewGuid(),
                    Date = DateTimeOffset.Now,
                    Status = "New"
                };

                Cookie cookie = RetrieveCookieById(cookieId);

                currentOrder.OrderLines.Add(new OrderLine
                {
                    Cookie = cookie,
                    Quantity = 1
                });

                currentOrder.Price += cookie.Price;

                CreateDocument(this._orderCollectionName, currentOrder);
            }
        }

        /// <summary>
        /// Execute a SQL query that gets all orders
        /// </summary>
        public List<Order> RetrieveAllOrders()
        {
            List<Order> orders = new List<Order>();

            // Set some common query options
            FeedOptions queryOptions = new FeedOptions { MaxItemCount = -1, EnableCrossPartitionQuery = true };

            // Run a simple query via LINQ. 
            // Cosmos DB indexes all properties, so queries can be completed efficiently and with low latency.
            IQueryable<Order> orderQuery = this._client.CreateDocumentQuery<Order>(
                UriFactory.CreateDocumentCollectionUri(
                    this._databaseName,
                    this._orderCollectionName), queryOptions);

            // The query is executed synchronously here, 
            // But can also be executed asynchronously via the IDocumentQuery<T> interface
            orders = orderQuery.ToList();

            return orders;
        }

        /// <summary>
        /// Execute a SQL query that gets an order by id
        /// </summary>
        public Order RetrieveOrderById(Guid orderId)
        {
            FeedOptions queryOptions = new FeedOptions { MaxItemCount = -1, EnableCrossPartitionQuery = true };

            //retrieve the order by Id
            Order order = this._client.CreateDocumentQuery<Order>(
                UriFactory.CreateDocumentCollectionUri(
                    this._databaseName,
                    this._orderCollectionName), queryOptions)
                    .Where(c => c.Id == orderId)
                    .AsEnumerable()
                    .FirstOrDefault();

            return order;
        }

        /// <summary>
        /// Remove an order by id
        /// </summary>
        public void CancelOrder(Guid orderId)
        {
            Order order = RetrieveOrderById(orderId);

            if (order != null)
            {
                this._client.DeleteDocumentAsync(
                     UriFactory.CreateDocumentUri(
                         this._databaseName,
                         this._orderCollectionName,
                         order.Id.ToString()),
                     new RequestOptions() { PartitionKey = new PartitionKey(Undefined.Value) });
            }
        }

        /// <summary>
        /// Change the status of an order by id
        /// </summary>
        public void PlaceOrder(Guid orderId)
        {
            Order order = RetrieveOrderById(orderId);

            if (order != null)
            {
                order.Status = "Placed";

                this._client.ReplaceDocumentAsync(
                    UriFactory.CreateDocumentUri(
                    this._databaseName,
                    this._orderCollectionName,
                    order.Id.ToString()),
                    order);
            }
        }

        /// <summary>
        /// Creates a new document in Cosmos DB in the given collection
        /// </summary>
        /// <param name="collectionname">Name of the Cosmos DB collection</param>
        /// <param name="document">Object to store in Cosmos DB</param>
        private void CreateDocument(string collectionname, object document)
        {
            this._client.CreateDocumentAsync(
                UriFactory.CreateDocumentCollectionUri(this._databaseName, collectionname), document);
        }
    }
}



