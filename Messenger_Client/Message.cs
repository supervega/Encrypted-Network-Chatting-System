using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpPrivacy;
using SharpPrivacy.Cipher;
using SharpPrivacy.Cipher.Math;
using SharpPrivacy.OpenPGP;
using SharpPrivacy.OpenPGP.Messages;
using System.Security.Cryptography;
using System.Collections;
using System.Windows.Forms;

namespace Messenger_Client
{
    public enum ExternalMsgTypes
    {
        ServerPk,ClientPk,Info,Disconnect,Transfer
    }

    public enum InternalMsgTypes
    {
        MessageTransfer, Registration, ValidName, InvalidName, Disconnect
    }

    public class Message
    {
        private string msg;
        private ExternalMsgTypes externalType;
        private InternalMsgTypes internalType;
        private string destination_Name = "";
        private string source_name = "";

        public Message()
        {
            msg = "";
            externalType = ExternalMsgTypes.Transfer;
            internalType = InternalMsgTypes.MessageTransfer;
        }

        #region Properties

        public string Msg
        {
            get
            {
                return msg;
            }
            set
            {
                msg = value;
            }
        }

        public ExternalMsgTypes ExternalType
        {
            get
            {
                return externalType;
            }
            set
            {
                externalType = value;
            }
        }

        public InternalMsgTypes InternalType
        {
            get
            {
                return internalType;
            }
            set
            {
                internalType = value;
            }
        }

        public string DestinationName
        {
            get
            {
                return destination_Name;
            }
            set
            {
                destination_Name = value;
            }
        }

        public string Source_Name
        {
            get
            {
                return source_name;
            }
            set
            {
                source_name = value;
            }
        }

        #endregion

        #region Functions

        public void ToObject(byte[] data)
        {
            externalType = (ExternalMsgTypes)BitConverter.ToInt32(data, 0);
            int ToLenght = BitConverter.ToInt32(data, 4);
            UnicodeEncoding UE = new UnicodeEncoding();
            destination_Name = UE.GetString(data, 8, ToLenght);
            int msgLenght = BitConverter.ToInt32(data, 8+ToLenght);
            msg = UE.GetString(data, 12+ToLenght,msgLenght);            
        }

        public byte[] ToBinary()
        {            
            byte[] data = new byte[8000];

            UnicodeEncoding UE = new UnicodeEncoding();
            BitConverter.GetBytes((int)externalType).CopyTo(data, 0);
            BitConverter.GetBytes(UE.GetByteCount(destination_Name)).CopyTo(data, 4);
            UE.GetBytes(destination_Name).CopyTo(data, 8);
            BitConverter.GetBytes(UE.GetByteCount(msg)).CopyTo(data, 8+UE.GetByteCount(destination_Name));
            UE.GetBytes(msg).CopyTo(data, 12 + UE.GetByteCount(destination_Name));

            return data;
        }

        public void Encrypt(string MyPrivateKey,string DestinationPublicKey)
        {
            msg = (int)internalType+source_name.Length.ToString() + source_name + msg;

            try
            {
                //Message Encryption AES 128
                SharpPrivacy.Cipher.SymmetricAlgorithm SA = null;
                SA = CipherHelper.CreateSymAlgorithm(SymAlgorithms.AES128);
                SA.GenerateKey();
                SA.IV = SA.Key;
                SharpPrivacy.Cipher.ICryptoTransform Encryptor = SA.CreateEncryptor();
                UnicodeEncoding UE=new UnicodeEncoding();
                byte[] Message = new byte[msg.Length];
                for (int i = 0; i < msg.Length; i++)
                {
                    Message[i] = (byte)msg[i];
                }

                int addedbytes = 0;
                int Temp = Message.Length;
                while (Temp % 16 != 0)
                {
                    addedbytes++;
                    Temp++;
                }
                byte[] NewMessage = new byte[Message.Length + addedbytes];
                for (int i = 0; i < NewMessage.Length; i++)
                {
                    if (i < Message.Length)
                        NewMessage[i] = Message[i];
                    else
                        NewMessage[i] = (byte)' ';
                }
                byte[] EncryptedMessage = new byte[NewMessage.Length];
                Encryptor.TransformBlock(NewMessage, 0, NewMessage.Length, ref EncryptedMessage, 0);
                msg = Convert.ToBase64String(EncryptedMessage);

                //Key Encryption
                RSACryptoServiceProvider rsaCryptoServiceProvider = new RSACryptoServiceProvider(1024);
                rsaCryptoServiceProvider.FromXmlString(DestinationPublicKey);
                int keySize = 128;
                byte[] bytes = SA.Key;
                // The hash function in use by the .NET RSACryptoServiceProvider here is SHA1
                // int maxLength = ( keySize ) - 2 - ( 2 * SHA1.Create().ComputeHash( rawBytes ).Length );
                int maxLength = keySize - 42;
                int dataLength = bytes.Length;
                int iterations = dataLength / maxLength;
                StringBuilder stringBuilder = new StringBuilder();
                for (int i = 0; i <= iterations; i++)
                {
                    byte[] tempBytes = new byte[(dataLength - maxLength * i > maxLength) ? maxLength : dataLength - maxLength * i];
                    Buffer.BlockCopy(bytes, maxLength * i, tempBytes, 0, tempBytes.Length);
                    byte[] encryptedBytes = rsaCryptoServiceProvider.Encrypt(tempBytes, false);
                    // Be aware the RSACryptoServiceProvider reverses the order of encrypted bytes after encryption and before decryption.
                    // If you do not require compatibility with Microsoft Cryptographic API (CAPI) and/or other vendors.
                    // Comment out the next line and the corresponding one in the DecryptString function.
                    Array.Reverse(encryptedBytes);
                    // Why convert to base 64?
                    // Because it is the largest power-of-two base printable using only ASCII characters
                    stringBuilder.Append(Convert.ToBase64String(encryptedBytes));
                }
                string EncryptedKey = stringBuilder.ToString();

                //House-Keeping                
                byte[] messagebytes = Convert.FromBase64String(msg);
                byte[] keybytes = Convert.FromBase64String(EncryptedKey);
                byte[] newMsgbytes = new byte[messagebytes.Length + keybytes.Length + 8];
                int messageLen = messagebytes.Length;
                int keyLen = keybytes.Length;

                BitConverter.GetBytes(keyLen).CopyTo(newMsgbytes, 0);
                BitConverter.GetBytes(messageLen).CopyTo(newMsgbytes, 4);
                keybytes.CopyTo(newMsgbytes, 8);
                messagebytes.CopyTo(newMsgbytes, keybytes.Length + 8);

                //Message Signing
                RSACryptoServiceProvider rsaCryptoServiceProvider2 = new RSACryptoServiceProvider(1024);
                rsaCryptoServiceProvider2.FromXmlString(MyPrivateKey);
                SHA1Managed sha1 = new SHA1Managed();
                byte[] hash = sha1.ComputeHash(newMsgbytes);
                byte[] signtature = rsaCryptoServiceProvider2.SignHash(hash, CryptoConfig.MapNameToOID("SHA1"));
                byte[] TotalMsg = new byte[newMsgbytes.Length + 128];
                Array.Copy(signtature, 0, TotalMsg, 0, 128);
                Array.Copy(newMsgbytes, 0, TotalMsg, 128, newMsgbytes.Length);
                msg = Convert.ToBase64String(TotalMsg);
            }
            catch (Exception ex)
            {
                ;
            }
        }

        public bool Decrypt(string MyPrivateKey, string SourcePublicKey)
        {
            //Decrypt Process
            
            try
            {
                //Message Signing
                RSACryptoServiceProvider rsaCryptoServiceProvider = new RSACryptoServiceProvider(1024);
                rsaCryptoServiceProvider.FromXmlString(SourcePublicKey);
                SHA1Managed sha1 = new SHA1Managed();                
                byte[] data = Convert.FromBase64String(msg);
                byte[] signature = new byte[128];
                byte[] messagebytes = new byte[data.Length - 128];
                Array.Copy(data, 0, signature, 0, 128);
                Array.Copy(data, 128, messagebytes, 0, messagebytes.Length);

                byte[] hash = sha1.ComputeHash(messagebytes);
                if (!rsaCryptoServiceProvider.VerifyHash(hash, CryptoConfig.MapNameToOID("SHA1"), signature))
                {
                    //MessageBox.Show("Couldn't verify the message, please verify your certificate.");
                    return false;
                }            

                //House-Keeping            
                int keyLen = BitConverter.ToInt32(messagebytes, 0);
                int messageLen = BitConverter.ToInt32(messagebytes, 4);
                byte[] keybytes = new byte[keyLen];
                byte[] msgbytes = new byte[messageLen];

                Array.Copy(messagebytes, 8, keybytes, 0, keyLen);
                Array.Copy(messagebytes, 8 + keyLen, msgbytes, 0, messageLen);

                //Key Decryption
                RSACryptoServiceProvider rsaCryptoServiceProvider2 = new RSACryptoServiceProvider(1024);
                rsaCryptoServiceProvider2.FromXmlString(MyPrivateKey);
                int base64BlockSize = ((1024 / 8) % 3 != 0) ? (((1024 / 8) / 3) * 4) + 4 : ((1024 / 8) / 3) * 4;
                string TempStr = Convert.ToBase64String(keybytes);
                int iterations = TempStr.Length / base64BlockSize;
                ArrayList arrayList = new ArrayList();
                for (int i = 0; i < iterations; i++)
                {
                    byte[] encryptedBytes = Convert.FromBase64String(TempStr.Substring(base64BlockSize * i, base64BlockSize));
                    Array.Reverse(encryptedBytes);
                    arrayList.AddRange(rsaCryptoServiceProvider2.Decrypt(encryptedBytes, false));
                }
                string AES_KEY = Convert.ToBase64String(arrayList.ToArray(Type.GetType("System.Byte")) as byte[]);

                //Decrypt AES 128
                byte[] DecryptedKey = Convert.FromBase64String(AES_KEY);
                SharpPrivacy.Cipher.SymmetricAlgorithm SA = null;
                SA = CipherHelper.CreateSymAlgorithm(SymAlgorithms.AES128);
                SA.Key = DecryptedKey;
                SA.IV = SA.Key;

                SharpPrivacy.Cipher.ICryptoTransform Decryptor = SA.CreateDecryptor();
                byte[] Result = new byte[msgbytes.Length];
                Decryptor.TransformBlock(msgbytes, 0, msgbytes.Length, ref Result, 0);

                UnicodeEncoding UE = new UnicodeEncoding();
                msg = UnicodeEncoding.ASCII.GetString(Result);
                string Temp = Convert.ToBase64String(Result);
                string temp2 = UnicodeEncoding.ASCII.GetString(Result);
                string temp3 = UnicodeEncoding.Unicode.GetString(Result);
                string temp4 = UnicodeEncoding.UTF32.GetString(Result);
                string temp5 = UnicodeEncoding.UTF8.GetString(Result);

                if (msg.Length > 0)
                {
                    internalType = (InternalMsgTypes)Convert.ToInt32(msg[0].ToString());
                    int source_Lenght = Convert.ToInt32(msg[1].ToString());
                    source_name = msg.Substring(2, source_Lenght);
                    msg = msg.Substring(source_Lenght + 2, msg.Length - source_Lenght - 2);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
            return true;
        }

        #endregion
    }
}


        
