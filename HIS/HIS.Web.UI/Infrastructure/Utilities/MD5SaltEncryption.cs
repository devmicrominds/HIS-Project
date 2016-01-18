using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace HIS.Web.UI
{
    public class MD5SaltEncryption
    {
        private byte[] EncStringBytes;
        private UTF8Encoding Encoder = new UTF8Encoding();

        private MD5CryptoServiceProvider MD5Hasher = new MD5CryptoServiceProvider();

        /*
         e.g 
         * 
         *  var hash = md5saltEncryption.Encrypt ("9999999" + "9999999");
            var x = md5saltEncryption.Verify ("9999999" + "9999999",hash);
         * 		  
         */
        //
        // TODO: Add constructor logic here
        //
        public MD5SaltEncryption()
        {
        }

        //Encrptes the string in MD5 when passed as a string
        public string Encrypt(string EncString)
        {
            //Variable Declarations
            Random RanGen = new Random();
            string RanString = string.Empty;
            string MD5String = string.Empty;
            string RanSaltLoc = string.Empty;

            //Generates a Random number of under 4 digits
            while (RanString.Length < 2)
            {
                RanString = RanString + RanGen.Next(0, 9);
            }

            //Converts the String to bytes
            EncStringBytes = Encoder.GetBytes(EncString + RanString);

            //Generates the MD5 Byte Array
            EncStringBytes = MD5Hasher.ComputeHash(EncStringBytes);

            //Affixing Salt information into the MD5 hash
            MD5String = BitConverter.ToString(EncStringBytes);
            MD5String = MD5String.Replace("-", string.Empty);

            MD5String = EightCharRandom(MD5String, RanString);

            //Returns the MD5 encrypted string to the calling object
            return MD5String;
        }

        //Verifies the String entered matches the MD5 Hash
        public bool Verify_Old(string S, string Hash)
        {
            //Variable Declarations
            int SaltAddress = 0;
            string SaltID = string.Empty;
            string NewHash = string.Empty;

            //Finds the Salt Address and Removes the Salt Address from the string
            SaltAddress = int.Parse(Hash.Substring(22, 2));
            Hash = Hash.Remove(22, 2);

            //Finds the SaltID and removes it from the string
            SaltID = Hash.Substring(SaltAddress, 4);
            Hash = Hash.Remove(SaltAddress, 4);

            //Converts the string passed to us to Bytes
            EncStringBytes = Encoder.GetBytes(S + SaltID);

            //Encryptes the string passed to us with the salt
            EncStringBytes = MD5Hasher.ComputeHash(EncStringBytes);

            //Converts the Hash to a string
            NewHash = BitConverter.ToString(EncStringBytes);
            NewHash = NewHash.Replace("-", string.Empty);

            //NewHash = EightCharRandom(NewHash, SaltID);

            //Tests the new has against the one passed to us
            if (NewHash == Hash)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //Verifies the String entered matches the MD5 Hash
        public bool Verify(string S, string Hash)
        {
            //Variable Declarations
            int SaltAddress = 0;
            string SaltID = string.Empty;
            string NewHash = string.Empty;

            SaltID = Hash.Substring(6, 2);

            EncStringBytes = Encoder.GetBytes(S + SaltID);

            //Encryptes the string passed to us with the salt
            EncStringBytes = MD5Hasher.ComputeHash(EncStringBytes);

            //Converts the Hash to a string
            NewHash = BitConverter.ToString(EncStringBytes);
            NewHash = NewHash.Replace("-", string.Empty);

            string FirstAdd = Hash.Substring(0, 1) + NewHash.Substring(int.Parse(Hash.Substring(0, 1)), 1);
            string SecAdd = Hash.Substring(2, 1) + NewHash.Substring(int.Parse(Hash.Substring(2, 1)) + 10, 1);
            string ThirdAdd = Hash.Substring(4, 1) + NewHash.Substring(int.Parse(Hash.Substring(4, 1)) + 20, 1);
            NewHash = FirstAdd + SecAdd + ThirdAdd + SaltID;

            //Tests the new has against the one passed to us
            if (NewHash == Hash)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public string normalMD5(string EncString)
        {
            string MD5String = string.Empty;

            //Converts the String to bytes
            EncStringBytes = Encoder.GetBytes(EncString);

            //Generates the MD5 Byte Array
            EncStringBytes = MD5Hasher.ComputeHash(EncStringBytes);

            //Affixing Salt information into the MD5 hash
            MD5String = BitConverter.ToString(EncStringBytes);
            MD5String = MD5String.Replace("-", null);

            return MD5String;
        }

        public string EightCharRandom(string hash, string ranStr)
        {
            string selectedChar = "";
            int pivot = 0;
            Random keys = new Random();

            for (int i = 1; i <= 3; i++)
            {
                if (i == 1)
                {
                    pivot = 0;
                }
                else
                {
                    pivot = 10 * (i - 1);
                }

                int stringLocation = keys.Next(0, 9);
                selectedChar += stringLocation.ToString() + hash.Substring(pivot + stringLocation, 1);
            }

            selectedChar = selectedChar + ranStr;
            return selectedChar;
            //7CBBD15217 D2D32288FA F81316B0EA 723A9F66
            //0EAC6CF01A C399DEDA25 91274A42A1 585EBB34
            //5C         6D         44
        }
    }
}