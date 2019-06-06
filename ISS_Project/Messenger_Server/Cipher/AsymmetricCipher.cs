//
// This file is part of the source code distribution of SharpPrivacy.
// SharpPrivacy is an Open Source OpenPGP implementation and can be 
// found at http://www.sharpprivacy.net
// It is released under Gnu General Public License and can be used 
// and modified as long as the result is released under GPL too. 
// For a copy of the GPL, please go to www.gnu.org/copyleft/gpl.html 
//
// AsymmetricCipher.cs: 
// 	Abstract class that defines what functions an asymmetric cipher
//	should implement.
//
// Author:
//	Daniel Fabian (df@sharpprivacy.net)
//
//
// Version: 0.1.0 (initial release)
//
// Changelog:
//	- 11.01.2003: Created this file.
//	- 01.06.2003: Added this header for the first beta release.
//
// (C) 2003, Daniel Fabian
//
using System;
using SharpPrivacy.OpenPGP;
using SharpPrivacy.Cipher.Math;

namespace SharpPrivacy.Cipher {
	
	public abstract class AsymmetricCipher: object {
		
		public abstract BigInteger[][] Generate(int nbits);
		
		public abstract BigInteger[] Encrypt(BigInteger biPlain, PublicKeyPacket pkpKey);
		
		public abstract BigInteger Decrypt(BigInteger[] biCipher, SecretKeyPacket spkKey, string strPassphrase);
		
		public abstract BigInteger[] Sign(BigInteger biHash, SecretKeyPacket spkKey, string strPassphrase);
		
		public abstract bool Verify(BigInteger[] biSignature, BigInteger biHash, PublicKeyPacket pkpKey);
		
	}
}
