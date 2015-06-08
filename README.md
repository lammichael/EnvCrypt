EnvCrypt Core Library
===

EnvCrypt is a C# crendentials manager providing encryption of crendentials, and a simple API for your applications to decrypt and use them at runtime.  The [RSA](http://en.wikipedia.org/wiki/RSA_%28cryptosystem%29) asymmetric and [AES](http://en.wikipedia.org/wiki/Advanced_Encryption_Standard) symmetric encryption algorithms are currently supported.  PlainText is also supported.

The aim of EnvCrypt is to provide a way to keep your environment's sensitive crendentials private in an efficient and reliable way without the hassle of creating an online service.  EnvCrypt uses the file system, and it is advised to keep it to the local file system for reliability.

To use EnvCrypt you need to have a workflow similar to this:
  1. Understand what you want to keep in EnvCrypt and which parts you want to encrypt.
  2. Create an __EnvCrypt Key__ file.
  3. Use EnvCrypt to encrypt (some of) these details.  EnvCrypt will add these to an __EnvCrypt DAT__ file.
  4. If you decide to use asymmetric encryption,
    1. give the _public key_ to developers and production support to use to add new encrypted entries to the EnvCrypt.
    2. store the _private key_ on the server where your applications run and restrict read access to only the functional account your applications run as.

You can use the supported AES symmetric encryption method, but this removes the benefit of being able to allow anyone to add to your DAT file without knowing what the already encrypted values are.

Distribution of EnvCrypt Files
---
![Where you can put your EnvCrypt files](https://github.com/lammichael/EnvCrypt.Core/blob/master/docs/EnvCrypt-FileDistribution.png)


The Key File
---
For asymmetric encryption two (2) files are generated: the private key (used for decryption and encryption), and the public key (used for encryption only).  For symmetric encryption, only one key for decryption and encryption is generated.

The DAT file
---
All entries in the DAT file are contained within a category. You can only store C# strings in each entry.


Other Features
===
Audit of Performed Decryptions
---
If you have many applications using one or many EnvCrypt DAT files on a server, you may want to know the impact of changing DAT entries, or you may want to investigate what processes are requesting using EnvCrypt.  EnvCrypt provides an extension point for you to log decryption requests.  There is a sample logger which can write every request, including the keys used and the entry category & name, to a file containing the UTC timestamp and the name of the executable that made the request.

What EnvCrypt doesn't do
===
* Provide an online service, but you can create one around EnvCrypt. E.G., Creating a service to transmit private keys for the client to use with EnvCrypt to decrypt.
* Token level authentication.