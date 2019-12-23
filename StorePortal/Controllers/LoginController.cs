using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using EncryptionLib;
using FinalProj;
using System.Net.Http;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

//MitchelKing
namespace FinalProj.Controllers
{

    [ApiController]
    public class LoginController : ControllerBase
    {
        static readonly IFormatter formatter = new BinaryFormatter();
        private readonly ILogger<LoginController> _logger;
        public LoginController(ILogger<LoginController> logger)
        {
            _logger = logger;
        }

        //objects deserialized
        //will make these an API call later
        public static List<User> usersList;

        #region API
        /// <summary>
        /// create account method
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [Route("create")]
        [HttpPost]
        public async Task<bool> CreateUser(IFormCollection user)
        {
            usersList = GetUsers();
            try
            {
                //salt and password hash strings
                String salt;
                String pass = PasswordManager.GeneratePasswordHash(user["pass"], out salt);
                User newUser = new User
                {
                    //set name, password hash and salt
                    gsName = user["name"],
                    gsPass = pass,
                    gsSalt = salt
                };
                usersList.Add(newUser);
                QuickSort(usersList, 0, usersList.Count-1);
                await SetUsers(usersList);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("fail" + e.Message);
                return false;
            }
        }

        /// <summary>
        /// serialize users from user list
        /// </summary>
        /// <param name="users"></param>
        /// <returns></returns>
        private Task SetUsers(List<User> users)
        {
            //users = Sort(users);
            return Task.Run(() =>
            {
                using (FileStream stream = new FileStream("users.dat", FileMode.Create, FileAccess.Write))
                {
                    foreach (User user in users)
                    {
                        formatter.Serialize(stream, user);
                    }
                }
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        //Login method
        [Route("login")]
        [HttpPost]
        public async Task<bool> Login(IFormCollection user)
        {
            usersList = GetUsers();
            QuickSort(usersList, 0, usersList.Count-1);
            bool result = await Validate(user["name"], user["pass"]);
            return result;
        }

        /// <summary>
        ///task to check form input to users list
        ///change to work with same USERNAME
        /// </summary>
        /// <param name="user"></param>
        /// <param name="pass"></param>
        /// <returns></returns>
        private Task<bool> Validate(String user, String pass)
        {
            return Task.Run(() =>
            {
                int mid;
                int min = 0;
                int max = usersList.Count-1;
                
                while (min <= max)
                {
                    mid = (max + min) / 2;
                    bool test = String.Compare(user,
                        usersList[mid].gsName, StringComparison.Ordinal) == 0;
                    if (test)
                    {
                        String hash;
                        String salt;

                        String compare = HashComputer.GetPasswordHashAndSalt(pass+usersList[mid].gsSalt);
                        bool test1 = compare == usersList[mid].gsPass;

                        if (PasswordManager.IsPasswordMatch(pass, usersList[mid].gsSalt,
                        usersList[mid].gsPass))
                        {
                            return true;
                        }
                        else
                        {
                            //fix this
                            return false;
                        }
                    }
                    else if (String.Compare(user,
                        usersList[mid].gsName, StringComparison.Ordinal) < 0)
                    {
                        max = mid - 1;
                    }
                    else
                    {
                        min = mid + 1;
                    }
                }
                return false;
               
            });
        }
        #endregion

        #region Utilities
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        //get users from deserialzed file
        private static List<User> GetUsers()
        {
            List<User> tempList = new List<User>();
            using (FileStream stream = new FileStream("users.dat", FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                while (stream.Position != stream.Length)
                {
                    User newUser = (User)formatter.Deserialize(stream);
                    tempList.Add(newUser);
                }
                //tempList = Sort(tempList);
                return tempList;
            }
        }

        //QuickSort A+
        private static void QuickSort(List<User> arr, int left,  int right)
        {
            if (left < right)
            {
                int pivot = Partition(arr, left, right);

                if (pivot > 1)
                {
                    QuickSort(arr, left, pivot - 1);
                }
                if(pivot +1 < right)
                {
                    QuickSort(arr, pivot + 1, right);
                }
            }
        }

        private static int Partition(List<User> arr, int left, int right)
        {
            User pivot = arr[left];
            while (true)
            {
                while(arr[left].compareTo(pivot)<0)
                {
                    left++;
                }
                while (arr[right].compareTo(pivot)>0)
                {
                    right--;
                }
                if (left < right)
                {
                    if (arr[left] == arr[right]) return right;

                    User temp = arr[left];
                    arr[left] = arr[right];
                    arr[right] = temp;
                }
                else
                {
                    return right;
                }
            }
        }

        //MergeSort cookeddd
        private static List<User> MergeSort(List<User> left, List<User> right)
        {
            List<User> sortedNodes = new List<User>();

            while (left.Count > 0 || right.Count > 0)
            {
                if (left.Count > 0 && right.Count > 0)
                {
                    if (left.First().compareTo(right.First()) <= 0)
                    {
                        sortedNodes.Add(left.First());
                        left.Remove(left.First());
                    }
                    else
                    {
                        sortedNodes.Add(right.First());
                        right.Remove(right.First());
                    }
                }
                else if (left.Count > 0)
                {
                    sortedNodes.Add(left.First());
                    left.Remove(left.First());
                }
                else if (right.Count > 0)
                {
                    sortedNodes.Add(right.First());
                    right.Remove(right.First());
                }
            }
            return sortedNodes;
        }

        private static List<User> Sort(List<User> userList)
        {
            if(usersList.Count <= 1)
            {
                return userList;
            }

            int high = userList.Count;
            List<User> left = new List<User>();
            List<User> right = new List<User>();

            int middle = high / 2;
            for (int i = 0; i < middle; i++)  //Dividing the unsorted list
            {
                left.Add(usersList[i]);
            }
            for (int i = middle; i < high; i++)
            {
                right.Add(userList[i]);
            }

            //left = Sort(left);
            //right = Sort(right);
            return MergeSort(left, right);
        }
        #endregion
    }
}
