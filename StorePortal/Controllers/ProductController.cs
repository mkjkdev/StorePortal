using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

//MitchelKing
namespace FinalProj.Controllers
{
    [ApiController]
    public class ProductController : ControllerBase
    {
        static IFormatter formatter = new BinaryFormatter();
        private readonly ILogger<ProductController> _logger;
        public ProductController(ILogger<ProductController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// productList is saved to binary
        /// </summary>
        public static List<Product> productList = new List<Product>();
        private static BinaryTree<Product> bTree = new BinaryTree<Product>();
        private static List<Node<Product>> nodes = new List<Node<Product>>();


        /// <summary>
        /// Binary Search from balanced BST
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [Route("search")]
        [HttpPost]
        public async Task<Product> searchProducts(IFormCollection key)
        {
            Product prod = await search(key["key"]);
            return prod;
        }

        private Task<Product> search(String key)
        {
            return Task.Run(() =>
            {
                Node<Product> result;
                result = bTree.Search(bTree.Root, key);
                return result.gsData;
            });

        }

        /// <summary>
        /// API for product list to front end
        /// Gets products, merge sorts products and builds balanced BST
        /// </summary>
        /// <returns></returns>
        [Route("getProducts")]
        [HttpGet]
        public async Task<List<Product>> ProductLoad()

        {
            List<Product> tempList = await GetProducts();
            bTree = await buildTree();
            productList = tempList;
            return tempList;
        }

        private Task<BinaryTree<Product>> buildTree()
        {
            return Task.Run(() =>
            {
                //bubble sort
                BinaryTree<Product>.Sort(nodes);
                //BinaryTree<Product>.mergeSort(nodes);
                bTree.Root = bTree.buildTreeUtil(nodes, 0, nodes.Count-1);
                return bTree;
            }); 
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private Task<List<Product>> GetProducts()
        {
            return Task.Run(() =>
            {
                List<Node<Product>> tempNodes = new List<Node<Product>>();
                List<Product> tempList = new List<Product>();
                using (FileStream stream = new FileStream("products.dat", FileMode.OpenOrCreate, FileAccess.ReadWrite))
                {
                    while (stream.Position != stream.Length)
                    {
                        Product newProduct = (Product)formatter.Deserialize(stream);
                        tempList.Add(newProduct);
                        tempNodes.Add(new Node<Product>(newProduct));
                    }
                    nodes = tempNodes;
                }
                return tempList;
            });
        }

        /// <summary>
        ////change to return IActionresult
        /// </summary>
        /// <param name="products"></param>
        /// <returns></returns>
        [Route("createProducts")]
        [HttpPost]
        public async Task<bool> CreateProduct(IFormCollection products)
        {
            Product tempProduct = new Product();
            int.TryParse(products["quantity"], out int quantity);
            double.TryParse(products["price"], out double dprice);

            tempProduct.name = products["name"];
            tempProduct.quantity = quantity;
            tempProduct.dprice = dprice;
            productList.Add(tempProduct);

            await SetProducts(productList);

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="products"></param>
        /// <returns></returns>
        private Task SetProducts(List<Product> products)
        {
            return Task.Run(() =>
            {
                using (FileStream stream = new FileStream("products.dat", FileMode.Create, FileAccess.Write))
                {
                    foreach (Product prod in products)
                    {
                        formatter.Serialize(stream, prod);
                    }
                }
            });
        }
    }
}
