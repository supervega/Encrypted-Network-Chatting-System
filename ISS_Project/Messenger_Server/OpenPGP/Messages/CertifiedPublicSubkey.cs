﻿//
// This file is part of the source code distribution of SharpPrivacy.
// SharpPrivacy is an Open Source OpenPGP implementation and can be 
// found at http://www.sharpprivacy.net
// It is released under Gnu General Public License and can be used 
// and modified as long as the result is released under GPL too. 
// For a copy of the GPL, please go to www.gnu.org/copyleft/gpl.html 
//
// CertifiedPublicSubkey.cs: 
// 	Class for handling public subkeys and their signatures.
//
// Author:
//	Daniel Fabian (df@sharpprivacy.net)
//
//
// Version: 0.1.0 (initial release)
//
// Changelog:
//	- 02.03.2003: Created this file.
//	- 01.06.2003: Added this header for the first beta release.
//
// (C) 2003, Daniel Fabian
//
using System;
using System.Windows.Forms;
using SharpPrivacy.Cipher;
using System.Collections;

namespace SharpPrivacy.OpenPGP.Messages {
	
	/// <summary>
	/// CertifedPublicSubkey is a wrapper class for subkeys and
	/// the according revocation and/or keybinding signatures.
	/// It makes it easier to work with certified subkeys.
	/// </summary>
	/// <remarks>
	/// CertifedPublicSubkey is a wrapper class for subkeys and
	/// the according revocation and/or keybinding signatures.
	/// It makes it easier to work with certified subkeys.
	/// </remarks>
	public class CertifiedPublicSubkey {
		private PublicKeyPacket pkpSubkey;
		private SignaturePacket spKeyBindingSignature;
		private SignaturePacket spRevocationSignature;
		
		/// <summary>
		/// Gets or sets the subkey in the current context.
		/// </summary>
		/// <remarks>No remarks</remarks>
		/// <value>The subkey in the current context.</value>
		public PublicKeyPacket Subkey {
			get {
				return pkpSubkey;
			}
			set {
				pkpSubkey = value;
			}
		}
		
		/// <summary>
		/// Gets or sets a keybindingsignature that binds the
		/// subkey to the primary key.
		/// </summary>
		/// <remarks>No remarks</remarks>
		/// <value>A keybindingsignature that binds the
		/// subkey to the primary key.</value>
		public SignaturePacket KeyBindingSignature {
			get {
				return spKeyBindingSignature;
			}
			set {
				spKeyBindingSignature = value;
			}
		}
		
		/// <summary>
		/// Gets or sets a revocationsignature of the current subkey.
		/// If the subkey has not been revoced, it returns null.
		/// </summary>
		/// <remarks>No remarks</remarks>
		/// <value>A revocationsignature of the current subkey.
		/// If the subkey has not been revoced, it returns 
		/// null.</value>
		public SignaturePacket RevocationSignature {
			get {
				return spRevocationSignature;
			}
			set {
				spRevocationSignature = value;
			}
		}
		
		/// <summary>
		/// Creates a new CertifiedPublicSubkey with all null properties.
		/// </summary>
		/// <remarks>No remarks</remarks>
		public CertifiedPublicSubkey() {
			this.Subkey = null;
			this.KeyBindingSignature = null;
			this.RevocationSignature = null;
			
		}
		
		/// <summary>
		/// Generates the certified public subkey out of the properties
		/// in this.
		/// </summary>
		/// <returns>Returns a byte array containing the openpgp encoded
		/// representation of the certified public subkey.</returns>
		/// <remarks>No remarks</remarks>
		public byte[] Generate() {
			byte[] bSubKey = this.Subkey.Generate();
			byte[] bKeyBindingSignature = this.KeyBindingSignature.Generate();
			byte[] bRevocationSignature = new byte[0];
			
			if (this.RevocationSignature != null) {
				bRevocationSignature = this.RevocationSignature.Generate();
			}
			
			byte[] bData = new byte[bSubKey.Length + bKeyBindingSignature.Length + bRevocationSignature.Length];
			Array.Copy(bSubKey, bData, bSubKey.Length);
			Array.Copy(bKeyBindingSignature, 0, bData, bSubKey.Length, bKeyBindingSignature.Length);
			Array.Copy(bRevocationSignature, 0, bData, bSubKey.Length + bKeyBindingSignature.Length, bRevocationSignature.Length);
			
			return bData;
		}
		
		public void VerifyKeyBindingSignature(PublicKeyPacket pkpPrimaryKey) {
			if (this.KeyBindingSignature.Version == SignaturePacketVersionNumbers.v4) {
				byte[] bSubKey = new byte[pkpSubkey.Body.Length + 3];
				bSubKey[0] = 0x99;
				bSubKey[1] = (byte)((pkpSubkey.Body.Length >> 8) & 0xFF);
				bSubKey[2] = (byte)(pkpSubkey.Body.Length & 0xFF);
				Array.Copy(pkpSubkey.Body, 0, bSubKey, 3, pkpSubkey.Body.Length);
				
				byte[] bPrimaryKey = new byte[pkpPrimaryKey.Body.Length + 3];
				bPrimaryKey[0] = 0x99;
				bPrimaryKey[1] = (byte)((pkpPrimaryKey.Body.Length >> 8) & 0xFF);
				bPrimaryKey[2] = (byte)(pkpPrimaryKey.Body.Length & 0xFF);
				Array.Copy(pkpPrimaryKey.Body, 0, bPrimaryKey, 3, pkpPrimaryKey.Body.Length);
				
				byte[] bData = new byte[bPrimaryKey.Length + bSubKey.Length];
				Array.Copy(bSubKey, 0, bData, 0, bSubKey.Length);
				Array.Copy(bPrimaryKey, 0, bData, bSubKey.Length, bPrimaryKey.Length);
				
				this.KeyBindingSignature.Verify(bData, pkpPrimaryKey);
			}
		}
		
		public void SignKeyBindingSignature(PublicKeyPacket pkpPrimaryKey, SecretKeyPacket skpPrimaryKey, string strPassphrase) {
			byte[] bSubKey = new byte[pkpSubkey.Body.Length + 3];
			bSubKey[0] = 0x99;
			bSubKey[1] = (byte)((pkpSubkey.Body.Length >> 8) & 0xFF);
			bSubKey[2] = (byte)(pkpSubkey.Body.Length & 0xFF);
			Array.Copy(pkpSubkey.Body, 0, bSubKey, 3, pkpSubkey.Body.Length);
			
			byte[] bPrimaryKey = new byte[pkpPrimaryKey.Body.Length + 3];
			bPrimaryKey[0] = 0x99;
			bPrimaryKey[1] = (byte)((pkpPrimaryKey.Body.Length >> 8) & 0xFF);
			bPrimaryKey[2] = (byte)(pkpPrimaryKey.Body.Length & 0xFF);
			Array.Copy(pkpPrimaryKey.Body, 0, bPrimaryKey, 3, pkpPrimaryKey.Body.Length);
			
			byte[] bData = new byte[bPrimaryKey.Length + bSubKey.Length];
			Array.Copy(bSubKey, 0, bData, 0, bSubKey.Length);
			Array.Copy(bPrimaryKey, 0, bData, bSubKey.Length, bPrimaryKey.Length);
			
			SignaturePacket spKeyBindingSig = new SignaturePacket();
			spKeyBindingSig.Version = SignaturePacketVersionNumbers.v4;
			spKeyBindingSig.HashAlgorithm = HashAlgorithms.SHA1;
			spKeyBindingSig.KeyID = pkpPrimaryKey.KeyID;
			spKeyBindingSig.SignatureType = SignatureTypes.SubkeyBindingSignature;
			
			spKeyBindingSig.Sign(bData, skpPrimaryKey, strPassphrase);
			this.KeyBindingSignature = spKeyBindingSig;
		}
		
		
	}
}
